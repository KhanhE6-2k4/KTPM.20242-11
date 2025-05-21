using System;

namespace MediaStore.Exceptions
{
    public class OrderException : Exception
    {
        public OrderException() : base() { }

        public OrderException(string message) : base(message) { }

        public OrderException(string message, Exception inner) : base(message, inner) { }
    }
}