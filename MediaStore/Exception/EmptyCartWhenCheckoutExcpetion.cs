using System;
using MediaStore.Exceptions;

public class EmptyCartWhenCheckoutException : CartException
{
    public EmptyCartWhenCheckoutException() { }
    public EmptyCartWhenCheckoutException(string message) : base(message) { }
    public EmptyCartWhenCheckoutException(string message, System.Exception inner) : base(message, inner) { }

}