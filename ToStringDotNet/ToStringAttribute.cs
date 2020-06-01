using System;

namespace ToStringDotNet
{
    /// <summary>
    /// Marks the field/property as a member of the ToString() method implementation
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ToStringAttribute : Attribute
    {
        public ToStringAttribute(byte priority = 0)
        {
            this.Priority = priority;
        }

        /// <summary>
        /// Properties with higher priorities will be printed before the others
        /// </summary>
        public byte Priority { get; }
    }
}