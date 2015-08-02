namespace BookDb.Infrastructure.Exceptions
{
    public class NotFoundException : BookException
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}