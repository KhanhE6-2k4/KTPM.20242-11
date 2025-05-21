using System;
using MediaStore.Exceptions;

public class DeliveryAddressNoSupportException : OrderException
{
    public DeliveryAddressNoSupportException() { }
    public DeliveryAddressNoSupportException(string message) : base(message) { }
    public DeliveryAddressNoSupportException(string message, System.Exception inner) : base(message, inner) { }

}