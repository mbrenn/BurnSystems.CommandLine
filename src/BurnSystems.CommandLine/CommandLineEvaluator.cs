
namespace BurnSystems.CommandLine
{
    using System;
    using System.Collections.Generic;
    using BurnSystems.CommandLine;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Evaluates the command line
    /// </summary>
    public class CommandLineEvaluator
    {
        /// <summary>
        /// Nichtbenannte Argument
        /// </summary>
        private List<string> unnamedArguments =
            new List<string>();

        /// <summary>
        /// Benannte Argumente
        /// </summary>
        private Dictionary<string, string> namedArguments =
            new Dictionary<string, string>();

        private List<ICommandLineDefinition> definitions =
            new List<ICommandLineDefinition>();

        /// <summary>
        /// Stores the value whether the parsing already had been executed
        /// </summary>
        private bool isParsed;

        /// <summary>
        /// Stores the arguments as given in the constructor
        /// </summary>
        private string[] arguments;

        /// <summary>
        /// Initializes a new instance of the CommandLineEvaluator class.
        /// </summary>
        /// <param name="arguments">List of program arguments</param>
        public CommandLineEvaluator(string[] arguments)
        {
            Contract.Assert(arguments != null);
            Contract.EndContractBlock();

            this.arguments = arguments;
        }

        /// <summary>
        /// Initializes a new instance of the CommandLineEvaluator class.
        /// </summary>
        /// <param name="arguments">List of program arguments</param>
        /// <param name="definitions">List of definitions, that will define the argument structure to be parsed</param>
        public CommandLineEvaluator(string[] arguments, IEnumerable<ICommandLineDefinition> definitions)
            : this(arguments)
        {
            this.definitions.AddRange(definitions);
        }

        /// <summary>
        /// Adds a filter to the evaluator. The filter will be called before and 
        /// after the parsing
        /// </summary>
        /// <param name="definition"></param>
        public void AddDefinition(ICommandLineDefinition definition)
        {
            this.definitions.Add(definition);
        }

        private void ParseIfNotParsed()
        {
            if ( !this.isParsed)
            {
                this.Parse();
            }
        }

        /// <summary>
        /// Parses the arguments
        /// </summary>
        /// <param name="arguments">Arguments to be parsed</param>
        private void Parse()
        {
            if (this.isParsed)
            {
                throw new InvalidOperationException("The arguments have been parsed already");
            }

            this.isParsed = true;

            foreach (var definition in definitions)
            {
                definition.BeforeParsing(this);
            }

            // The actual parsing
            foreach (var argument in arguments)
            {
                if (string.IsNullOrEmpty(argument))
                {
                    // Do nothing, when the argument is empty
                    continue;
                }
                else if (argument.StartsWith("--", StringComparison.Ordinal))
                {
                    this.namedArguments[argument.Substring(2)] = "1";

                    continue;
                }
                else if (argument[0] == '-')
                {
                    int pos = argument.IndexOf('=');
                    if (pos == -1)
                    {
                        this.namedArguments[argument.Substring(1)] = "1";
                    }
                    else
                    {
                        this.namedArguments[argument.Substring(1, pos - 1)] =
                            argument.Substring(pos + 1);
                    }

                    continue;
                }
                else
                {
                    this.unnamedArguments.Add(argument);
                }
            }

            foreach (var definition in definitions)
            {
                definition.AfterParsing(this);
            }
        }

        /// <summary>
        /// Gets a list of unnamed arguments
        /// </summary>
        public List<string> UnnamedArguments
        {
            get
            {
                this.ParseIfNotParsed();
                return this.unnamedArguments;
            }
        }

        /// <summary>
        /// Gets a dictionary of named arguments
        /// </summary>
        public Dictionary<string, string> NamedArguments
        {
            get
            {
                this.ParseIfNotParsed();
                return this.namedArguments;
            }
        }
    }
}
