using System;
using MediaStore.Exceptions;

public class NoRushOrderItemExceptionException : OrderException
{
    public NoRushOrderItemExceptionException() { }
    public NoRushOrderItemExceptionException(string message) : base(message) { }
    public NoRushOrderItemExceptionException(string message, System.Exception inner) : base(message, inner) { }

}