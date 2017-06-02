using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.IO;

namespace AdbrainTest
{
    // tests dealing with reading the input, making sure we don't read in bad input
    public class ReadInputTests
    {
        [Fact]
        public static void ReadValidInputTest()
        {
            string validInput = @"| A G O I C|
| D E R U F|
| M P N B H|
| S T E F W|
";
            var file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(validInput);
            file.Close();

            var readOutput = Helpers.readInput(Constants.inputPath);

            Assert.True(readOutput.Length == 4);
            Assert.True(readOutput[0].Length == 5);
        }

        [Fact]
        public static void InputTooManyChars()
        {
            // test more than 1 char in a spot
            string invalidInput = @"| A G O I Cc|
| D E R U F|
| M P N B H|
| S T E F W|
";
            var file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(invalidInput);
            file.Close();

            Assert.Throws<Exception>(() => Helpers.readInput(Constants.inputPath));
        }

        [Fact]
        public static void InputTooManyColumns()
        {
            // test uneven columns
            string invalidInput = @"| A G O I C c|
| D E R U F|
| M P N B H|
| S T E F W|
";
            var file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(invalidInput);
            file.Close();

            Assert.Throws<Exception>(() => Helpers.readInput(Constants.inputPath));
        }

        [Fact]
        public static void InputBlank()
        {
            // test blank space
            var invalidInput = @"
";
            var file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(invalidInput);
            file.Close();

            Assert.Throws<Exception>(() => Helpers.readInput(Constants.inputPath));
        }

        [Fact]
        public static void InputNumbers()
        {
            // test numeric characters
            var invalidInput = @"| A G O I 1|
| D E R U F|
| M P N B H|
| S T E F W|
";
            var file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(invalidInput);
            file.Close();

            Assert.Throws<Exception>(() => Helpers.readInput(Constants.inputPath));
        }
    }

    public class SolveMatrixTests
    {
        [Fact]
        public static void SolveOriginalMatrixTest()
        {
            string validInput = @"| A G O I C|
| D E R U F|
| M P N B H|
| S T E F W|
";
            var file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(validInput);
            file.Close();

            Constants.longestWordLength = 10;
            var result = SolveMatrix.solveMatrix().ToList();

            Assert.True(result.Count > 0);
            Assert.True(result.Contains("AAAA")); // check goes to same character
            Assert.True(result.Contains("ADAD")); // can move vertically
            Assert.True(result.Contains("AGADE")); // can move horizontally
            Assert.True(result.Contains("REDA")); // can start from a middle character
            Assert.True(result.Contains("TEN")); // can access bottom row
            Assert.True(result.Contains("FUR")); // can access right column
            Assert.True(!result.Contains("HUNT")); // diagonals/unconnected letters do not work
        }

        [Fact]
        public static void SolveOneColumnMatrixTest()
        {
            string validInput = @"| H|
| A|
| N|
| D|
";
            var file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(validInput);
            file.Close();

            Constants.longestWordLength = 10;
            var result = SolveMatrix.solveMatrix().ToList();

            Assert.True(result.Count > 0);
            Assert.True(result.Contains("HAND")); 
        }

        [Fact]
        public static void SolveOneRowMatrixTest()
        {
            string validInput = @"| H A N D|
";
            var file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(validInput);
            file.Close();

            Constants.longestWordLength = 10;
            var result = SolveMatrix.solveMatrix().ToList();

            Assert.True(result.Count > 0);
            Assert.True(result.Contains("HAND"));
        }

        /// <summary>
        /// Ensure the result for transposed matrices are the same
        /// </summary>
        [Fact]
        public static void CompareTransposedMatricesTest()
        {
            string validInput = @"| H A N D|
";
            var file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(validInput);
            file.Close();

            Constants.longestWordLength = 10;
            var result1 = SolveMatrix.solveMatrix().ToList();

            validInput = @"| H|
| A|
| N|
| D|
";
            file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(validInput);
            file.Close();

            var result2 = SolveMatrix.solveMatrix().ToList();

            Assert.True(result1.Count == result2.Count);
            foreach(var result in result1)
            {
                Assert.True(result2.Contains(result));
            }
        }

        /// <summary>
        /// Ensure the result for different matrices are different
        /// </summary>
        [Fact]
        public static void CompareDifferentMatricesTest()
        {
            string validInput = @"| H A N D|
";
            var file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(validInput);
            file.Close();

            Constants.longestWordLength = 10;
            var result1 = SolveMatrix.solveMatrix().ToList();

            validInput = @"| H A N D A|
";
            file = new StreamWriter(File.Open(Constants.inputPath, FileMode.Create));
            file.Write(validInput);
            file.Close();

            var result2 = SolveMatrix.solveMatrix().ToList();

            Assert.True(result1.Count != result2.Count);
        }
    }
}
