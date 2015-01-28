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
            var filter = new DefaultValueArgument(name, value);
            evaluator.AddDefinition(filter);

            return evaluator;
        }

        public static CommandLineEvaluator Requires(this CommandLineEvaluator evaluator, string name)
        {
            var filter = new RequiredArgument(name);
            evaluator.AddDefinition(filter);

            return evaluator;
        }
    }
}
