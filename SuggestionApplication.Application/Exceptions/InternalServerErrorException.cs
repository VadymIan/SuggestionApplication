namespace SuggestionApplication.Application.Exceptions
{
    public class InternalServerErrorException : ApplicationException
    {
        public InternalServerErrorException(string message) : base(message)
        {

        }
    }
}
