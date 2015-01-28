using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.CommandLine
{
    /// <summary>
    /// This interface is used for all definitions being used in CommandLineEvaluator
    /// </summary>
    public interface ICommandLineDefinition
    {
        /// <summary>
        /// This method will be called before the arguments will be parsed
        /// </summary>
        /// <param name="evaluator">Evaluator to be used</param>
        void BeforeParsing(CommandLineEvaluator evaluator);

        /// <summary>
        /// This method will be called after the arguments will be parsed
        /// </summary>
        /// <param name="evaluator">Evaluator to be used</param>
        void AfterParsing(CommandLineEvaluator evaluator);
    }
}
