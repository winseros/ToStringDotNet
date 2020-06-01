using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace ToStringDotNet
{
    [DebuggerDisplay("Name={Name}, Priority={Priority}")]
    internal class ToStringProp
    {
        public ToStringProp(PropertyInfo prop, byte priority)
        {
            this.Priority = priority;
            this.Name = prop.Name;
            this.Property = prop;
            this.PropType = prop.PropertyType;
        }

        public ToStringProp(FieldInfo field, byte priority)
        {
            this.Priority = priority;
            this.Name = field.Name;
            this.Field = field;
            this.PropType = field.FieldType;
        }

        public PropertyInfo? Property { get; set; }

        public FieldInfo? Field { get; set; }

        public Type PropType { get; set; }

        public byte Priority { get; set; }

        public string Name { get; set; }

        public Expression Access(Expression instance)
        {
            return this.Property == null 
                ? Expression.Field(instance, this.Field) 
                : Expression.Property(instance, this.Property);
        }
    }
}
