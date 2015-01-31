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
                if (!argument.IsRequired)
                {
                    continue;
                }

                var namedArgument = argument as NamedArgumentInfo;
                var unnamedArgument = argument as UnnamedArgumentInfo;

                if (namedArgument != null)
                {
                    if (!evaluator.NamedArguments.ContainsKey(namedArgument.LongName))
                    {
                        evaluator.AddError(
                            "Required Argument '" + namedArgument.LongName + "' is not given");
                    }
                }

                if (unnamedArgument != null)
                {
                    if (unnamedArgument.Index >= evaluator.UnnamedArguments.Count)
                    {
                        if (unnamedArgument.Index == 0)
                        {
                            evaluator.AddError(
                                string.Format(
                                    "Not enough arguments were given. 1 argument was expected"));
                        }
                        else
                        {
                            evaluator.AddError(
                                string.Format(
                                    "Not enough arguments were given. {0} arguments were expected",
                                    unnamedArgument.Index + 1));
                        }
                    }
                }
            }
        }
    }
}
