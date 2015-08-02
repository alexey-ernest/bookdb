namespace BookDb.Exceptions
{
    public class ConflictException : BookException
    {
        public ConflictException(string message)
            : base(message)
        {
        }
    }
}