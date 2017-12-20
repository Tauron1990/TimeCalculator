using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex regex = new Regex("(?<Lenght>\\d{2}?\\d) cm x (?<Weith>\\d{2}?\\d) cm");

            var temp = regex.Match("050 cm x 075 cm");
        }
    }
}
