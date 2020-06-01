using ToStringDotNet;

namespace ToStringDotNetProfiling
{
    public class TestClass1
    {
        public static TestClass1 WithProps()
        {
            return new TestClass1
            {
                Prop1 = "p1",
                Prop2 = "p2",
                Prop3 = 1
            };
        }

        [ToString]
        public string Prop1 { get; set; }

        [ToString]
        public string Prop2 { get; set; }

        [ToString]
        public int Prop3 { get; set; }
    }
}
