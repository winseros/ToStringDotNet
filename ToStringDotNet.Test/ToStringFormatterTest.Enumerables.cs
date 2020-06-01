using System.Collections.Generic;
using Xunit;

namespace ToStringDotNet.Test
{
    public class ToStringFormatterTestEnumerables
    {
        public class TestObject1
        {
            [ToString]
            public string Prop0 { get; set; }
        }

        [Fact]
        public void Test_Format_Handles_List_Of_Objects()
        {
            string str = ToStringFormatter.Format(new List<TestObject1> {new TestObject1 {Prop0 = "s1"}, new TestObject1 {Prop0 = "s2"}, new TestObject1 {Prop0 = "s3"}});
            const string expected = "[{\"Prop0\":\"s1\"},{\"Prop0\":\"s2\"},{\"Prop0\":\"s3\"}]";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_List_Of_Scalars()
        {
            string str = ToStringFormatter.Format(new List<string>{"s1", "s2", "s3"});
            const string expected = "[\"s1\",\"s2\",\"s3\"]";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Array_Of_Objects()
        {
            string str = ToStringFormatter.Format(new[] {new TestObject1 {Prop0 = "s1"}, new TestObject1 {Prop0 = "s2"}, new TestObject1 {Prop0 = "s3"}});
            const string expected = "[{\"Prop0\":\"s1\"},{\"Prop0\":\"s2\"},{\"Prop0\":\"s3\"}]";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Array_Of_Scalars()
        {
            string str = ToStringFormatter.Format(new[] {"s1", "s2", "s3"});
            const string expected = "[\"s1\",\"s2\",\"s3\"]";
            Assert.Equal(expected, str);
        }
    }
}