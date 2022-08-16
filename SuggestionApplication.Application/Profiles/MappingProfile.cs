using AutoMapper;
using SuggestionApplication.Application.DTO;
using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, RegisterUserViewModel>().ReverseMap();
        }
    }
}
