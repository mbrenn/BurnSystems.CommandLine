using BurnSystems.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var evaluator = new Parser(args)
                .WithArgument("input", hasValue: true, helpText: "Secret", isRequired: true)
                .WithArgument("output", hasValue: true)
                .WithArgument(0, helpText: "Input file", isRequired: true)
                .WithArgument(1, helpText: "Output file");

            if (evaluator.ParseOrShowUsage())
            {
                Console.WriteLine("Success");
            }
        }
    }
}
