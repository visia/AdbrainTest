using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AdbrainTest
{
    public class SolveMatrix
    {
        /// <summary>
        /// Solves input.txt using dictionary.txt as the dictionary
        /// Input is a matrix as follows:
        /// | A G O I C|
        /// | D E R U F|
        /// | M P N B H|
        /// | S T E F W|
        /// Output will be an IEnumerable containing all words found
        /// 
        /// Strategy: just brute force it...
        /// start from every possible position, check if it's a word, move to all possible positions, check if it's a word, recurse
        /// ending condition is when the word length limit is exceeded (set in Constants.cs)
        /// 
        /// assumes dictionary is lowercase
        /// assumes diagonals are not "adjacent" and do not count
        /// </summary>
        public static IEnumerable<string> solveMatrix()
        {
            string[][] input = Helpers.readInput(Constants.inputPath);

            for(int i=0; i < input.Length; i++)
            {
                for(int j=0; j < input[i].Length; j++)
                {
                    foreach(var solution in solveMatrixRecursive(input[i][j], j, i, input))
                    {
                        yield return solution;
                    }
                }
            }
        }

        /// <summary>
        /// To iterate, we need to hold the current string in memory, check if it's exceeded the length limit,
        /// check if it's a word, then go to all possible positions
        /// and recurse with that new currString
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> solveMatrixRecursive(string currString, int posX, int posY, string[][] input)
        {
            if(currString.Length > Constants.longestWordLength)
            {
                yield break;
            }

            if (SolveMatrix.dictionary.Contains(currString.ToLower()))
            {
                yield return currString;
            }

            // stay on same position
            foreach(var solution in solveMatrixRecursive(currString + input[posY][posX], posX, posY, input))
            {
                yield return solution;
            }

            // move 1 left
            if ((posX - 1) >= 0)
            {
                foreach(var solution in solveMatrixRecursive(currString + input[posY][posX - 1], posX-1, posY, input))
                {
                    yield return solution;
                }
            }

            // move 1 right
            if ((posX + 1) < input[posY].Length)
            {
                foreach (var solution in solveMatrixRecursive(currString + input[posY][posX + 1], posX + 1, posY, input))
                {
                    yield return solution;
                }
            }

            // move 1 up
            if ((posY - 1) >= 0)
            {
                foreach (var solution in solveMatrixRecursive(currString + input[posY - 1][posX], posX, posY - 1, input))
                {
                    yield return solution;
                }
            }

            // move 1 down
            if ((posY + 1) < input.Length)
            {
                foreach (var solution in solveMatrixRecursive(currString + input[posY + 1][posX], posX, posY + 1, input))
                {
                    yield return solution;
                }
            }
        }

        static HashSet<string> dictionary { get { return _dictionary.Value; } }
        private static Lazy<HashSet<string>> _dictionary = new Lazy<HashSet<string>>(() => { return Helpers.readDictionary(Constants.dictionaryPath); });


    }

    class Helpers
    {
        /// <summary>
        /// Reads a dictionary file and returns a hashset with all words in the dictionary (delimited by new line)
        /// </summary>
        public static HashSet<string> readDictionary(string path)
        {
            HashSet<string> ret = new HashSet<string>();

            TextReader tr = new StreamReader(path);
            string word;
            while ((word = tr.ReadLine()) != null)
            {
                try
                {
                    ret.Add(word);
                }
                catch (Exception e)
                {
                    tr.Close();
                    throw e; // dictionary formatted wrong?
                }
            }

            tr.Close();

            return ret;
        }

        /// <summary>
        /// Reads a file containing the input and returns a 2d string array with a letter in each slot
        /// input should be a text file with lines starting and ending with |, delimited by spaces
        /// </summary>
        public static string[][] readInput(string path)
        {
            List<string[]> lines = new List<string[]>();
            int numRows = 0;
            int numCols = 0;

            // read each line, trim the beginning and end, and split by spaces
            TextReader tr = new StreamReader(path);
            string row;

            while ((row = tr.ReadLine()) != null)
            {
                if (String.IsNullOrWhiteSpace(row))
                {
                    continue;
                }

                if (!row.StartsWith("| ") || !row.EndsWith("|"))
                {
                    tr.Close();
                    throw new Exception("Badly formatted input (line does not start or end with |)");
                }

                row = row.TrimStart('|').TrimStart(' ').TrimEnd('|');
                var splitRow = row.Split(' ');
                int spacesFound = splitRow.Length - 1;

                // ensure each column is a character
                for (int i=0; i < splitRow.Length; i++)
                {
                    if(splitRow[i].Length != 1 || !Char.IsLetter(splitRow[i][0]))
                    {
                        tr.Close();
                        throw new Exception("Badly formatted input (a column is more than 1 char long");
                    }
                }

                if (numRows == 0) // on the first iteration, set the number of columns
                {
                    numCols = spacesFound;
                }
                else
                {
                    if (numCols != spacesFound)
                    {
                        tr.Close();
                        throw new Exception("Badly formatted input (number of columns changes)");
                    }
                }

                lines.Add(splitRow);
                numRows++;
            }

            tr.Close();

            string[][] ret = new string[numRows][];
            for(int i=0; i < lines.Count; i++)
            {
                ret[i] = lines[i];
            }

            return ret;
        }
    }
}
