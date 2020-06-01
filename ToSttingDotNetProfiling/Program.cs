using System;
using ToStringDotNet;

namespace ToStringDotNetProfiling
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var i = 0; i < 1000000; i++)
            {
                Program.FormatRandom();
            }
        }

        private static void FormatRandom()
        {
            int rnd = new Random().Next(1, 4);
            switch (rnd)
            {
                case 1:
                    ToStringFormatter.Format(TestClass1.WithProps());
                    break;
                case 2:
                    ToStringFormatter.Format(TestClass2.WithProps());
                    break;
                case 3:
                    ToStringFormatter.Format(TestClass3.WithProps());
                    break;
                case 4:
                    ToStringFormatter.Format(TestClass4.WithProps());
                    break;
            }
        }
    }
}
