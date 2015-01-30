
namespace BurnSystems.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using BurnSystems.CommandLine;
    using System.IO;
    using System.Diagnostics;
    using System.Reflection;

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
        /// Stores the argument information
        /// </summary>
        private List<ArgumentInfo> argumentInfos = new List<ArgumentInfo>();

        /// <summary>
        /// Named arguments
        /// </summary>
        private Dictionary<string, string> namedArguments =
            new Dictionary<string, string>();

        /// <summary>
        /// Stores the definitions, which will be replaced
        /// </summary>
        private List<ICommandLineFilter> filters =
            new List<ICommandLineFilter>();

        /// <summary>
        /// Stores the value whether the parsing already had been executed
        /// </summary>
        private bool isParsed;

        /// <summary>
        /// Stores the arguments as given in the constructor
        /// </summary>
        private string[] arguments;

        /// <summary>
        /// Returns the argument information
        /// </summary>
        public IEnumerable<ArgumentInfo> ArgumentInfos
        {
            get { return this.argumentInfos; }
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

        /// <summary>
        /// Initializes a new instance of the CommandLineEvaluator class.
        /// </summary>
        /// <param name="arguments">List of program arguments</param>
        public CommandLineEvaluator(string[] arguments)
        {
            Contract.Assert(arguments != null);
            Contract.EndContractBlock();

            this.arguments = arguments;

            this.filters.Add(new DefaultValueFilter());
            this.filters.Add(new RequiredFilter());
        }

        /// <summary>
        /// Initializes a new instance of the CommandLineEvaluator class.
        /// </summary>
        /// <param name="arguments">List of program arguments</param>
        /// <param name="definitions">List of definitions, that will define the argument structure to be parsed</param>
        public CommandLineEvaluator(string[] arguments, IEnumerable<ICommandLineFilter> definitions)
            : this(arguments)
        {
            this.filters.AddRange(definitions);
        }

        /// <summary>
        /// Adds the information for one argument
        /// </summary>
        /// <param name="info">Information the be added</param>
        public void Add(ArgumentInfo info)
        {
            this.argumentInfos.Add(info);
        }

        /// <summary>
        /// Adds a filter to the evaluator. The filter will be called before and 
        /// after the parsing
        /// </summary>
        /// <param name="filter"></param>
        public void AddFilter(ICommandLineFilter filter)
        {
            this.filters.Add(filter);
        }

        /// <summary>
        /// Parses the arguments and shows the usage, if it the parsing did not complete.
        /// True, if parsing was successful
        /// </summary>
        public bool ParseOrShowUsage()
        {
            try
            {
                this.Parse();
            }
            catch (ArgumentParseException exc)
            {
                this.ShowUsageAndException(exc);
                return false;
            }

            if (this.NamedArguments.ContainsKey("help"))
            {
                this.ShowUsage();
                return false;
            }

            return true;
        }

        private void ParseIfNotParsed()
        {
            if (!this.isParsed)
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

            foreach (var filter in filters)
            {
                filter.BeforeParsing(this);
            }

            // The actual parsing
            for(var n = 0; n < arguments.Length; n++)
            {
                var argument = arguments[n];
                if (string.IsNullOrEmpty(argument))
                {
                    // Do nothing, when the argument is empty
                    continue;
                }

                string argumentName = null;
                if (argument.StartsWith("--", StringComparison.Ordinal))
                {
                    argumentName = argument.Substring(2);
                }
                else if (argument[0] == '-')
                {
                    argumentName = argument.Substring(1);
                }

                if (argumentName != null)
                {
                    // Supports the named arguments with values
                    var info = 
                        this.argumentInfos.Where(x => x.LongName == argumentName).FirstOrDefault();

                    if (info == null || !info.HasValue)
                    {
                        this.namedArguments[argumentName] = "1";
                    }
                    else
                    {
                        n++;
                        if (arguments.Length <= n)
                        {
                            throw new ArgumentParseException(
                                "Value missing for parameter: " + argumentName);
                        }

                        this.namedArguments[argumentName] = arguments[n];
                    }
                }
                else
                {
                    this.unnamedArguments.Add(argument);
                }
            }

            foreach (var filter in filters)
            {
                filter.AfterParsing(this);
            }
        }

        /// <summary>
        /// Shows the exception and the usage argument
        /// </summary>
        /// <param name="exc">Exception being used</param>
        private void ShowUsageAndException(ArgumentParseException exc)
        {
            using (var writer = new StringWriter())
            {
                this.WriteException(writer, exc);
                this.WriteUsage(writer);

                Console.WriteLine(writer.GetStringBuilder().ToString());
            }
        }

        /// <summary>
        /// Shows the usage
        /// </summary>
        private void ShowUsage()
        {
            using (var writer = new StringWriter())
            {
                this.WriteUsage(writer);

                Console.WriteLine(writer.GetStringBuilder().ToString());
            }
        }

        public void WriteException(TextWriter writer, ArgumentParseException exc)
        {
            writer.WriteLine(exc.Message);
        }

        public void WriteUsage(TextWriter writer)
        {
            foreach (var argumentInfo in this.argumentInfos)
            {
                writer.WriteLine(
                    string.Format("{0}: {1}",
                    argumentInfo.LongName,
                    argumentInfo.HelpText));
            }
        }
    }
}
