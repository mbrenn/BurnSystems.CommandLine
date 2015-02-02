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
            var type = 1;
            if (type == 1)
            {
                var evaluator = new Parser(args)
                    .WithArgument("input", hasValue: true, helpText: "Secret", isRequired: true, shortName: 'i')
                    .WithArgument("output", hasValue: true, shortName: 'o')
                    .WithArgument(0, helpText: "Input file", isRequired: true)
                    .WithArgument(1, helpText: "Output file");

                if (evaluator.ParseOrShowUsage())
                {
                    Console.WriteLine("Success");
                }
            }
            else if (type == 2)
            {
                var arguments = Parser.ParseIntoOrShowUsage<ProgramArguments>(args);
                if (arguments != null)
                {
                    Console.WriteLine("Input: " + arguments.Input);
                    Console.WriteLine("Output: " + arguments.Output);
                    Console.WriteLine("Verbose: " + arguments.Verbose);
                    Console.WriteLine("Duration: " + arguments.Duration);
                }
            }
        }
    }
}
