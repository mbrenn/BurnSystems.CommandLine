using System;

namespace BurnSystems.CommandLine.ByAttributes
{
    /// <summary>
    /// Replicates the information from the argument info attribute
    /// </summary>
    public abstract class ArgumentInfoAttribute : Attribute
    {
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
