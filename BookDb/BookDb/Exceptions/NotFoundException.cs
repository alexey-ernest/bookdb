namespace BookDb.Exceptions
{
    public class NotFoundException : BookException
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}