using FluentValidation;

namespace SaaS_App.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public List<FieldError> Errors { get; set; } = new List<FieldError>();
        public class FieldError
        {
            public required string FieldName { get; set; }
            public required string Error { get; set; }
        }
    }
}
