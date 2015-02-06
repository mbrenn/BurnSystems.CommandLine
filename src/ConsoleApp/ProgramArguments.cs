using BurnSystems.CommandLine.ByAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ProgramArguments
    {
        [NamedArgument(IsRequired=true)]
        public string Input
        {
            get;
            set;
        }

        public string Output
        {
            get;
            set;
        }

        public bool Verbose
        {
            get;
            set;
        }

        public int Duration
        {
            get;
            set;
        }
    }
}
