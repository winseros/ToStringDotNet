using System;

namespace ToStringDotNet
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ToStringInheritanceAttribute : Attribute
    {
        public ToStringInheritanceAttribute(ToStringInheritance inheritance)
        {
            this.Inheritance = inheritance;
        }

        public ToStringInheritance Inheritance { get; private set; }
    }
}