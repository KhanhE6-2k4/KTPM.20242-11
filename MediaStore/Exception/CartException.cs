using System;

namespace MediaStore.Exceptions
{
    public class CartException : Exception
    {
        public CartException() : base() { }

        public CartException(string message) : base(message) { }

        public CartException(string message, Exception inner) : base(message, inner) { }
    }
}