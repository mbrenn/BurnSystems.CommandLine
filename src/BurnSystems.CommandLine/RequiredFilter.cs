using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.CommandLine
{
    /// <summary>
    /// Indicates that an argument is required. If argument is not given, an exception will be thrown
    /// </summary>
    public class RequiredFilter : ICommandLineFilter
    {
        /// <summary>
        /// Initializes a new instance of the RequiresArgument class.
        /// </summary>
        /// <param name="name">Name of the argument to be required</param>
        /// <param name="message">Message to be send out</param>
        public RequiredFilter()
        {
        }

        void ICommandLineFilter.BeforeParsing(Parser evaluator)
        {
        }

        void ICommandLineFilter.AfterParsing(Parser evaluator)
        {
            foreach (var argument in evaluator.ArgumentInfos)
            {
                if (argument.IsRequired
                    && !evaluator.NamedArguments.ContainsKey ( argument.LongName ))
                {
                    evaluator.AddError(
                        "Required Argument '" + argument.LongName + "' is not given");
                }
            }
        }
    }
}
