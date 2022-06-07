using System;
using System.Net;
using Xunit;

namespace ToStringDotNet.Test
{
    public class ToStringFormatterTestScalars
    {
        [Fact]
        public void Test_Format_Handles_String()
        {
            string str = ToStringFormatter.Format("aaa");
            const string expected = "\"aaa\"";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Byte()
        {
            string str = ToStringFormatter.Format((byte)1);
            const string expected = "1";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Int()
        {
            string str = ToStringFormatter.Format(1);
            const string expected = "1";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Float()
        {
            string str = ToStringFormatter.Format(1.01f);
            const string expected = "1.01";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Bool()
        {
            string str = ToStringFormatter.Format(true);
            const string expected = "True";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_DateTime()
        {
            string str = ToStringFormatter.Format(new DateTime(2015, 5, 15, 12, 5, 20, DateTimeKind.Utc));
            const string expected = "\"2015-05-15T12:05:20.0000000Z\"";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_TimeSpan()
        {
            string str = ToStringFormatter.Format(TimeSpan.FromDays(1));
            const string expected = "\"1:0:00:00\"";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Enums()
        {
            string str = ToStringFormatter.Format(HttpStatusCode.Accepted);
            const string expected = "\"202=Accepted\"";
            Assert.Equal(expected, str);
        }
    }
}