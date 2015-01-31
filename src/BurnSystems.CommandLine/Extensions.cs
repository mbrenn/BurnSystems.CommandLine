using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.CommandLine
{
    public static class Extensions
    {
        public static Parser WithDefaultValue(this Parser evaluator, string name, string value )
        {
            WithArgument(evaluator,
                name,
                defaultValue: value);

            return evaluator;
        }

        public static Parser Requires(this Parser evaluator, string name)
        {
            WithArgument(evaluator,
                name,
                isRequired: true);

            return evaluator;
        }

        public static Parser WithArgument(
            this Parser evaluator,
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

            evaluator.AddArgumentInfo(argument);

            return evaluator;
            
        }
    }
}
