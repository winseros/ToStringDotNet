using ToStringDotNet;

namespace ToStringDotNetProfiling
{
    [ToStringInheritance(ToStringInheritance.Inherit)]
    public class TestClass4: TestClass1
    {
        public new static TestClass4 WithProps()
        {
            return new TestClass4
            {
                Prop1 = "p1",
                Prop2 = "p2",
                Prop3 = 1
            };
        }
    }
}