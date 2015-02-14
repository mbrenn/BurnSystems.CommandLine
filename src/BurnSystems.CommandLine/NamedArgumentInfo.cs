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

        /// <summary>
        /// Converts the named argument to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.LongName))
            {
                return "Unknown Argument";
            }
            else
            {
                if (this.ShortName == '\0')
                {
                    return string.Format(
                        "--{0}",
                        this.LongName.ToLower());
                }
                else
                {
                    return string.Format(
                        "--{0} [-{1}]",
                        this.LongName.ToLower(),
                        this.ShortName);
                }
            }
        }
    }
}
