using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.CommandLine.ByAttributes
{ 
    /// <summary>
    /// Replicates the information from the NamedArgumentInfo so 
    /// it can be used within an attribute to configure the program arguments type
    /// </summary>    
    public class UnnamedArgumentAttribute : ArgumentInfoAttribute
    {
        /// <summary>
        /// Gets or sets the index where the parameter is located. 
        /// If index is -1, the argument is given as a named argument. 
        /// </summary>
        public int Index
        {
            get;
            set;
        }
    }
}
