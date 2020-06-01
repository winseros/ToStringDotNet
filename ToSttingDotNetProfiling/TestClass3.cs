using ToStringDotNet;

namespace ToStringDotNetProfiling
{
    public class TestClass3
    {
        public static TestClass3 WithProps()
        {
            return new TestClass3
            {
                Prop1 = 1.0f,
                Prop2 = 2.5,
                Prop3 = TestClass1.WithProps()
            };
        }

        [ToString]
        public float Prop1 { get; set; }

        [ToString]
        public double Prop2 { get; set; }

        [ToString]
        public TestClass1 Prop3 { get; set; }
    }
}