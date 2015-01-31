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
    internal class NamedArgumentInfo : ArgumentInfo
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
    }
}
