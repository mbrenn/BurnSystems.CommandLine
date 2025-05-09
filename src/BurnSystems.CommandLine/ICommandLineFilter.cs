namespace BurnSystems.CommandLine
{
    /// <summary>
    /// This interface is used for all definitions being used in CommandLineEvaluator
    /// </summary>
    public interface ICommandLineFilter
    {
        /// <summary>
        /// This method will be called before the arguments will be parsed
        /// </summary>
        /// <param name="evaluator">Evaluator to be used</param>
        void BeforeParsing(Parser evaluator);

        /// <summary>
        /// This method will be called after the arguments will be parsed
        /// </summary>
        /// <param name="evaluator">Evaluator to be used</param>
        void AfterParsing(Parser evaluator);
    }
}
