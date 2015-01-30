using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.CommandLine
{
    /// <summary>
    /// Stores the information to a specific argument, 
    /// that will be automatically parsed
    /// </summary>
    public class ArgumentInfo
    {
        public char ShortName
        {
            get;
            set;
        }

        public string LongName
        {
            get;
            set;
        }

        public bool HasValue
        {
            get;
            set;
        }

        public string HelpText
        {
            get;
            set;
        }

        public string DefaultValue
        {
            get;
            set;
        }

        public bool IsRequired
        {
            get;
            set;
        }
    }
}
