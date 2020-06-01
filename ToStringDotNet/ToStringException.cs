using System;

namespace ToStringDotNet
{
    public class ToStringException: ApplicationException
    {
        public ToStringException(string message) 
            : base(message)
        {
        }
    }
}
