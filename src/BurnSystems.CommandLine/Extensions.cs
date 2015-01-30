using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.CommandLine
{
    public static class Extensions
    {
        public static CommandLineEvaluator WithDefaultValue(this CommandLineEvaluator evaluator, string name, string value )
        {
            WithArgument(evaluator,
                name,
                defaultValue: value);

            return evaluator;
        }

        public static CommandLineEvaluator Requires(this CommandLineEvaluator evaluator, string name)
        {
            WithArgument(evaluator,
                name,
                isRequired: true);

            return evaluator;
        }

        public static CommandLineEvaluator WithArgument(
            this CommandLineEvaluator evaluator,
            string name,
            bool hasValue = false,
            string helpText = "", 
            string defaultValue = null,
            char shortName = '\0', 
            bool isRequired = false)
        {
            var argument = new ArgumentInfo();
            argument.LongName = name;
            argument.ShortName = shortName;
            argument.IsRequired = isRequired;
            argument.HasValue = hasValue;
            argument.HelpText = helpText;
            argument.DefaultValue = defaultValue;

            evaluator.AddArgumentInfo(argument);

            return evaluator;
            
        }
    }
}
