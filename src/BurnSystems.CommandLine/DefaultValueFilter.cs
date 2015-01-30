using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.CommandLine
{
    /// <summary>
    /// Defines an optional argument
    /// </summary>
    public class DefaultValueFilter : ICommandLineFilter
    {
        /// <summary>
        /// Initializes the optional argument for commandline evaluator
        /// </summary>
        /// <param name="name">Name of the argument</param>
        /// <param name="value">Value, if argument is not set</param>
        public DefaultValueFilter()
        {
        }

        void ICommandLineFilter.BeforeParsing(Parser evaluator)
        {
            foreach (var argument in evaluator.ArgumentInfos)
            {
                if (argument.DefaultValue != null)
                {
                    evaluator.NamedArguments[argument.LongName] = argument.DefaultValue;
                }
            }
        }

        void ICommandLineFilter.AfterParsing(Parser evaluator)
        {
        }
    }
}
