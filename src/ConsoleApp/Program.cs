using BurnSystems.CommandLine;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var type = 2;
            if (type == 1)
            {
                Console.WriteLine("Program is being executed via NON-ATTRIBUTE configuration");

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
                Console.WriteLine("Program is being executed via ATTRIBUTE configuration");

                var argument = Parser.ParseIntoOrShowUsage<ProgramArguments>(args);
                if (argument != null)
                {
                    Console.WriteLine("Input: " + argument.Input);
                    Console.WriteLine("Output: " + argument.Output);
                    Console.WriteLine("Verbose: " + argument.Verbose);
                    Console.WriteLine("Duration: " + argument.Duration);
                }
            }
        }
    }
}
