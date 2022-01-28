using System;
using Xunit;

namespace ToStringDotNet.Test
{
    public class ToStringFormatterTestNullables
    {
        [Fact]
        public void Test_Format_Handles_Byte()
        {
            byte? data = 1;
            string str = ToStringFormatter.Format(data);
            const string expected = "1";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Int()
        {
            int? data = 1;
            string str = ToStringFormatter.Format(data);
            const string expected = "1";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Float()
        {
            float? data = 1.01f;
            string str = ToStringFormatter.Format(data);
            const string expected = "1.01";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Bool()
        {
            bool? data = true;
            string str = ToStringFormatter.Format(data);
            const string expected = "True";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_DateTime()
        {
            DateTime? data = new DateTime(2015, 5, 15, 12, 5, 20, DateTimeKind.Utc);
            string str = ToStringFormatter.Format(data);
            const string expected = "\"2015-05-15T12:05:20.0000000Z\"";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_TimeSpan()
        {
            TimeSpan? data = TimeSpan.FromDays(1);
            string str = ToStringFormatter.Format(data);
            const string expected = "\"1:0:00:00\"";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Guid()
        {
            Guid? data = Guid.Parse("f0f003d1-892f-401f-a5e6-eda283ec8052");
            string str = ToStringFormatter.Format(data);
            const string expected = "\"f0f003d1-892f-401f-a5e6-eda283ec8052\"";
            Assert.Equal(expected, str);
        }

        [Fact]
        public void Test_Format_Handles_Nulls()
        {
            Guid? data = null;
            string str = ToStringFormatter.Format(data);
            const string expected = "null";
            Assert.Equal(expected, str);
        }
    }
}
