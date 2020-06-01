using System;

namespace ToStringDotNet
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ToStringAttribute : Attribute
    {
        public ToStringAttribute(byte priority = 0)
        {
            this.Priority = priority;
        }

        public byte Priority { get; }
    }
}