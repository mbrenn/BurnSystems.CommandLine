using System.Linq;

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
            // Stores 
            var maxArgumentsExceptionGiven = false;

            // Goes through the arguments
            foreach (var argument in evaluator.ArgumentInfos)
            {
                if (!argument.IsRequired)
                {
                    continue;
                }

                if (argument is NamedArgumentInfo namedArgument)
                {
                    if (!evaluator.NamedArguments.ContainsKey(namedArgument.LongName))
                    {
                        evaluator.AddError(
                            "Required Argument '" + namedArgument.LongName + "' is not given");
                    }
                }

                if (argument is UnnamedArgumentInfo unnamedArgument)
                {
                    if (unnamedArgument.Index >= evaluator.UnnamedArguments.Count
                        && !maxArgumentsExceptionGiven)
                    {
                        maxArgumentsExceptionGiven = true;
                        var maxIndex =
                            evaluator.ArgumentInfos
                                .Select(x => x as UnnamedArgumentInfo)
                                .Where(x => x != null)
                                .Max(x => x.Index);

                        if (maxIndex == 0)
                        {
                            evaluator.AddError(
                                "Not enough arguments were given. 1 argument was expected");
                        }
                        else
                        {
                            evaluator.AddError(
                                $"Not enough arguments were given. {maxIndex + 1} arguments were expected");
                        }
                    }
                }
            }
        }
    }
}
