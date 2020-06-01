using System;
using ToStringDotNet;

namespace ToStringDotNetProfiling
{
    public class TestClass2
    {
        public static TestClass2 WithProps()
        {
            return new TestClass2
            {
                Prop1 = new DateTime(),
                Prop2 = new TimeSpan(),
                Prop3 = true
            };
        }

        [ToString]
        public DateTime Prop1 { get; set; }

        [ToString]
        public TimeSpan Prop2 { get; set; }

        [ToString]
        public bool Prop3 { get; set; }
    }
}