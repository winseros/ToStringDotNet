using System;
using System.Collections.Generic;
using Xunit;

namespace ToStringDotNet.Test
{
    public class ToStringFormatterTestObjects
    {
        public class TestObject1<T>
        {
            public TestObject1(T prop1)
            {
                this.Prop1 = prop1;
            }

            [ToString]
            public T Prop1 { get; private set; }

            public string Prop2 { get; set; }
        }

        public class TestObject2
        {
            [ToString(1)]
            public string Prop0 { get; set; }

            [ToString(2)]
            public string Prop1 { get; set; }

            [ToString(3)]
            public string Prop2 { get; set; }

            [ToString(1)]
            public string Prop3 { get; set; }
        }

        [Fact]
        public void Test_Format_Handles_Null()
        {
            string str = ToStringFormatter.Format((string)null);
            const string expected = "null";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_String()
        {
            var obj = new TestObject1<string>("some-value");
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop1\":\"some-value\"}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Byte()
        {
            var obj = new TestObject1<byte>(1);
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop1\":1}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Int()
        {
            var obj = new TestObject1<int>(1);
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop1\":1}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Float()
        {
            var obj = new TestObject1<float>(1.01f);
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop1\":1.01}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Bool()
        {
            var obj = new TestObject1<bool>(true);
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop1\":True}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_DateTime()
        {
            var obj = new TestObject1<DateTime>(new DateTime(2015, 5, 15, 12, 5, 20, DateTimeKind.Utc));
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop1\":\"2015-05-15T12:05:20.0000000Z\"}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_TimeSpan()
        {
            var obj = new TestObject1<TimeSpan>(TimeSpan.FromDays(1));
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop1\":\"1:0:00:00\"}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Multiple_Fields()
        {
            var obj = new TestObject2
            {
                Prop0 = "p0",
                Prop1 = "p1",
                Prop2 = "p2",
                Prop3 = "p3"
            };
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop2\":\"p2\",\"Prop1\":\"p1\",\"Prop0\":\"p0\",\"Prop3\":\"p3\"}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Null_Strings()
        {
            var obj = new TestObject2();
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop2\":null,\"Prop1\":null,\"Prop0\":null,\"Prop3\":null}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Objects()
        {
            var obj = new TestObject1<TestObject1<string>>(new TestObject1<string>("val"));
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop1\":{\"Prop1\":\"val\"}}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Null_Objects()
        {
            var obj = new TestObject1<TestObject1<string>>(null);
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop1\":null}";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Enumerable_Props()
        {
            var obj = new TestObject1<IEnumerable<string>>(new []{"1", "2", "3"});
            string str = ToStringFormatter.Format(obj);
            const string expected = "{\"Prop1\":[\"1\",\"2\",\"3\"]}";
            Assert.Equal(expected, str);
        }
    }
}
