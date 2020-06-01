using Xunit;

namespace ToStringDotNet.Test
{
    public class ToStringFormatterTestInheritance
    {
        public class TestClass1
        {
            [ToString]
            public string Prop1 { get; set; }

            public string Prop2 { get; set; }
        }

        [ToStringInheritance(ToStringInheritance.Inherit)]
        public class TestClass2: TestClass1
        {
            [ToString]
            public string Prop3 { get; set; }
        }

        [ToStringInheritance(ToStringInheritance.NotInherit)]
        public class TestClass3: TestClass1
        {
            [ToString]
            public string Prop3 { get; set; }
        }

        [Fact]
        public void Test_Format_Prints_Inherited_Props()
        {
            string str = ToStringFormatter.Format(new TestClass2{Prop1 = "p1", Prop2 = "p2", Prop3 = "p3"});
            const string expected = "{\"Prop1\":\"p1\",\"Prop3\":\"p3\"}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Does_Not_Print_Not_Inherited_Props()
        {
            string str = ToStringFormatter.Format(new TestClass3{Prop1 = "p1", Prop2 = "p2", Prop3 = "p3"});
            const string expected = "{\"Prop3\":\"p3\"}";
            Assert.Equal(expected, str);
        }
    }
}
