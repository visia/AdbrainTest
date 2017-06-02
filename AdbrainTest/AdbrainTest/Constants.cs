using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AdbrainTest
{
    public class Constants
    {
        private static string currPath = System.IO.Directory.GetCurrentDirectory();

        public static string dictionaryPath = Path.Combine(currPath, @"Dictionary.txt");

        public static string inputPath = Path.Combine(currPath, @"input.txt");

        public static int longestWordLength = 10;
    }
}
