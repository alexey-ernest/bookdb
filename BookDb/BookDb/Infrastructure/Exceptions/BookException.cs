using System;

namespace BookDb.Infrastructure.Exceptions
{
    public class BookException : ApplicationException
    {
        public BookException(string message)
            : base(message)
        {
        }
    }
}