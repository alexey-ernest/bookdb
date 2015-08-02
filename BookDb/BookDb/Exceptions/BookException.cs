using System;

namespace BookDb.Exceptions
{
    public class BookException : ApplicationException
    {
        public BookException(string message)
            : base(message)
        {
        }
    }
}