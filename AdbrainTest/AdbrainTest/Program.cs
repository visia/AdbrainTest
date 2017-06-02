using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdbrainTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // solve the matrix in input.txt using the dictionary in Dictionary.txt
            var result = SolveMatrix.solveMatrix();
            foreach(var solution in result)
            {
                Console.WriteLine(solution);
            }
        }
    }
}
