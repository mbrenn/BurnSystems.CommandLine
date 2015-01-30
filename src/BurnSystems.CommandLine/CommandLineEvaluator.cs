
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
    using BurnSystems.CommandLine.Helper;

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
        /// Stores the errors which were created during parsing
        /// </summary>
        private List<string> errors = new List<string>();

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
                if (!this.isParsed)
                {
                    this.Parse();
                }

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
                if (!this.isParsed)
                {
                    this.Parse();
                }

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
        /// <param name="info">Information to be added</param>
        public void AddArgumentInfo(ArgumentInfo info)
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
            this.Parse();

            if (this.errors.Count > 0)
            {
                this.ShowUsageAndException();
                return false;
            }

            if (this.NamedArguments.ContainsKey("help"))
            {
                this.ShowUsage();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks, if we have an error during parsing
        /// </summary>
        private void CheckForErrors()
        {
            if (!this.isParsed)
            {
                throw new InvalidOperationException("ParseOrShowUsage() is not called");
            }

            if ( this.errors.Count > 0 )
            {
                throw new InvalidOperationException("Errors occured during parsing");
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

                if (argument.StartsWith("--", StringComparison.Ordinal))
                {
                    var argumentName = argument.Substring(2);
                    n = AddValueToNamedArgument(n, argumentName);
                }
                else if (argument[0] == '-')
                {
                    var argumentName = argument.Substring(1);

                    foreach (var cChar in argumentName)
                    {
                        var info = this.argumentInfos.Where(x => x.ShortName == cChar).FirstOrDefault();
                        if (info == null || !info.HasValue)
                        {
                            this.NamedArguments[argumentName] = "1";
                        }
                        else if (!info.HasValue)
                        {
                            this.NamedArguments[info.LongName] = "1";
                        }
                        else
                        {
                            if (argumentName.Length > 1)
                            {
                                this.AddError("Shortname " + cChar + " has a value and is used with other options");
                            }

                            n = this.AddValueToNamedArgument(n, info.LongName);
                        }
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
        /// Adds a value to a named argument
        /// </summary>
        /// <param name="n">Position, where the argument name was found</param>
        /// <param name="argumentName">Name of the argument being used</param>
        /// <returns>The new position to be used for the parsing</returns>
        private int AddValueToNamedArgument(int n, string argumentName)
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
                    this.AddError(
                        "Value missing for parameter: " + argumentName);
                }
                else
                {
                    this.namedArguments[argumentName] = arguments[n];
                }
            }
            return n;
        }

        /// <summary>
        /// Adds an error to the parsing
        /// </summary>
        /// <param name="error">Error to be added</param>
        public void AddError(string error)
        {
            this.errors.Add(error);
        }

        /// <summary>
        /// Shows the exception and the usage argument
        /// </summary>
        /// <param name="exc">Exception being used</param>
        private void ShowUsageAndException()
        {
            using (var writer = new StringWriter())
            {
                this.WriteIntroduction(writer);
                this.WriteException(writer);
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
                this.WriteIntroduction(writer);
                this.WriteUsage(writer);

                Console.WriteLine(writer.GetStringBuilder().ToString());
            }
        }

        public void WriteIntroduction(TextWriter writer)
        {
            var options = string.Empty;

            if (this.argumentInfos.Count > 0)
            {
                options = " {options}";
            }

            var assembly = Assembly.GetEntryAssembly();

            if (assembly != null)
            {
                writer.WriteLine(
                    string.Format("{0}{1}",
                        Path.GetFileName(Assembly.GetEntryAssembly().Location),
                        options));
            }
        }

        public void WriteUsage(TextWriter writer)
        {   
            // No arguments, no information
            if (this.ArgumentInfos.Count() == 0)
            {
                return;
            }

            // Gets the maximum length of the arguments
            var maxLength = this.ArgumentInfos.Max(x => x.LongName.Length);

            writer.WriteLine();
            writer.WriteLine("Arguments: ");
            foreach (var argumentInfo in this.argumentInfos)
            {
                writer.WriteLine(
                    string.Format(
                        "    --{0}{1}",
                        StringManipulation.PaddingRight(argumentInfo.LongName, maxLength + 4),
                        argumentInfo.HelpText));
            }
        }

        public void WriteException(TextWriter writer)
        {
            if (this.errors.Count > 0)
            {
                writer.WriteLine();
                writer.WriteLine("An error occured during parsing:");
            }

            foreach (var error in this.errors)
            {
                writer.WriteLine("  " + error);
            }
        }
    }
}
