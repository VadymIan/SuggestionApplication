using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SuggestionApplication.Application.DTO;
using SuggestionApplication.Application.Exceptions;
using SuggestionApplication.Application.Infrastructure;
using SuggestionApplication.Application.Models;
using SuggestionApplication.Application.Persistance;
using SuggestionApplication.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SuggestionApplication.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        private readonly JwtSettings _jwtSettings;

        public UserService(IUserRepository userRepository, 
                           ILogger<UserService> logger, 
                           IMapper mapper,
                           IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<User> Register(RegisterUserViewModel registerUserVM)
        {
            var user = _mapper.Map<User>(registerUserVM);

            CreatePasswordHash(registerUserVM.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return await _userRepository.AddAsync(user);
        }

        public async Task<string> Login(LoginUserViewModel loginUser)
        {
            var user = await _userRepository.GetUserByEmail(loginUser.EmailAddress);
            if (user is null)
            {
                throw new NotFoundException(nameof(User), loginUser.EmailAddress);
            }

            if (!VerifyPassword(loginUser.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new BadRequestException("Incorret input data");
            }

            var token = GenerateToken(user);

            return token;
        }

        public async Task UpdateUser(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.EmailAddress)
            };

            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public async Task<User> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadRequestException($"Incorret input data : {nameof(id)}");
            }

            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
            {
                throw new NotFoundException(nameof(User), id);
            }

            return user;
        }

        public async Task ComfirmEmail(Guid userId)
        {
            var user = await GetUserById(userId);
            user.EmailIsConfirmed = true;

            await _userRepository.UpdateAsync(user);
        }
    }
}
