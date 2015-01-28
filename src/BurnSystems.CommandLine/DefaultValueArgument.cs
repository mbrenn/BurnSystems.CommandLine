using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.CommandLine
{
    /// <summary>
    /// Defines an optional argument
    /// </summary>
    public class DefaultValueArgument : ICommandLineDefinition
    {
        /// <summary>
        /// Name of the optional argument to be set
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Value of the optional argument to be set
        /// </summary>
        public object Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the optional argument for commandline evaluator
        /// </summary>
        /// <param name="name">Name of the argument</param>
        /// <param name="value">Value, if argument is not set</param>
        public DefaultValueArgument(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        void ICommandLineDefinition.BeforeParsing(CommandLineEvaluator evaluator)
        {
            evaluator.NamedArguments[this.Name] = this.Value.ToString();
        }

        void ICommandLineDefinition.AfterParsing(CommandLineEvaluator evaluator)
        {
        }
    }
}
