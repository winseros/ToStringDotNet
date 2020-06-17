using System;

namespace ToStringDotNet
{
    public sealed class ToStringException: ApplicationException
    {
        public ToStringException(string message) 
            : base(message)
        {
        }
    }
}
