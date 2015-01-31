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
                var namedArgument = argument as NamedArgumentInfo;
                var unnamedArgument = argument as UnnamedArgumentInfo;

                if (argument.DefaultValue != null)
                {
                    // Checks for a named argument
                    if (namedArgument != null)
                    {
                        evaluator.NamedArguments[namedArgument.LongName] = argument.DefaultValue;
                    }

                    // Checks for unnamed arguments
                    if (unnamedArgument != null)
                    {
                        if (evaluator.UnnamedArguments.Count == unnamedArgument.Index)
                        {
                            evaluator.UnnamedArguments.Add(unnamedArgument.DefaultValue);
                        }
                        else
                        {
                            evaluator.AddError("Argument at given position default value cannot be added: " + unnamedArgument.Index);
                        }
                    }
                }
            }
        }

        void ICommandLineFilter.AfterParsing(Parser evaluator)
        {
        }
    }
}
