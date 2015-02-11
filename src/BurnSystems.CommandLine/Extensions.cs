using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.CommandLine
{
    public static class Extensions
    {
        /// <summary>
        /// Adds a named, key-driven argument to the configuration of the parser.
        /// </summary>
        /// <param name="parser">Parser to which the argument will be added</param>
        /// <param name="name">Long name of the argument under which the variable can be retrieved</param>
        /// <param name="hasValue">When this argument is true, the value of the argument will be queried after the 
        /// key within the command line. 
        /// <example>
        /// hasValue is false: 
        /// ConsoleApp.exe --input output 
        /// NamedArgument: Input => 1
        /// UnnamedArgument: 1 => output
        /// 
        /// hasValue is true: 
        /// ConsoleApp.exe --input output 
        /// NamedArgument: Input => output
        /// UnnamedArgument: none
        /// </example>
        /// </param>
        /// <param name="helpText">The helptext, that will be shown to the user, when he requests the usage</param>
        /// <param name="defaultValue">The default value of the argument, if no unnamed argument was given</param>
        /// <param name="shortName">Maps a short name of the argument to the long name. The argument can be </param>
        /// <param name="isRequired">true, if the argument is required, otherwise an exception is thrown</param>
        /// <returns>The parser as given in the method argument 'parser'.</returns>
        public static Parser WithArgument(
            this Parser parser,
            string name,
            bool hasValue = false,
            string helpText = "", 
            string defaultValue = null,
            char shortName = '\0', 
            bool isRequired = false)
        {
            var argument = new NamedArgumentInfo();
            argument.LongName = name;
            argument.ShortName = shortName;
            argument.IsRequired = isRequired;
            argument.HasValue = hasValue;
            argument.HelpText = helpText;
            argument.DefaultValue = defaultValue;

            parser.AddArgumentInfo(argument);

            return parser;
        }
        
        /// <summary>
        /// Adds an unnamed, ordinal argument to the configuration of the parser.
        /// </summary>
        /// <param name="parser">Parser to which the argument will be added</param>
        /// <param name="index">Index to which this argument within the command line will be mapped. The first argument starts with 0</param>
        /// <param name="helpText">The helptext, that will be shown to the user, when he requests the usage</param>
        /// <param name="defaultValue">The default value of the argument, if no unnamed argument was given</param>
        /// <param name="isRequired">true, if the argument is required, otherwise an exception is thrown</param>
        /// <returns>The parser as given in the method argument 'parser'.</returns>
        public static Parser WithArgument(
            this Parser parser,
            int index,
            string helpText = "",
            string defaultValue = null,
            bool isRequired = false)
        {
            var argument = new UnnamedArgumentInfo();
            argument.Index = index;
            argument.IsRequired = isRequired;
            argument.HelpText = helpText;
            argument.DefaultValue = defaultValue;

            parser.AddArgumentInfo(argument);

            return parser;
        }

        [Obsolete("Use WithArgument")]
        public static Parser WithDefaultValue(this Parser evaluator, string name, string value)
        {
            WithArgument(evaluator,
                name,
                defaultValue: value);

            return evaluator;
        }

        [Obsolete("Use WithArgument")]
        public static Parser WithDefaultValue(this Parser evaluator, int index, string value)
        {
            WithArgument(evaluator,
                index,
                defaultValue: value);

            return evaluator;
        }

        [Obsolete("Use WithArgument")]
        public static Parser Requires(this Parser evaluator, string name)
        {
            WithArgument(evaluator,
                name,
                isRequired: true);

            return evaluator;
        }

        [Obsolete("Use WithArgument")]
        public static Parser Requires(this Parser evaluator, int index)
        {
            WithArgument(evaluator,
                index,
                isRequired: true);

            return evaluator;
        }
    }
}
