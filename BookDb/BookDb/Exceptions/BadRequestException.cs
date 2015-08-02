namespace BookDb.Exceptions
{
    public class BadRequestException : BookException
    {
        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}