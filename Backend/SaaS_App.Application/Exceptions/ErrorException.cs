namespace SaaS_App.Application.Exceptions
{
    public class ErrorException : Exception
    {
        public string _error { get; private set; }

        public ErrorException(string error)
        {
            _error = error;
        }
    }
}
