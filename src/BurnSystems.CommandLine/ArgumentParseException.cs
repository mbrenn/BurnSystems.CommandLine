using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.CommandLine
{
    public class ArgumentParseException : Exception
    {
        public ArgumentParseException()
        {
        }

        public ArgumentParseException(string message)
            : base(message)
        {
        }

        public ArgumentParseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
