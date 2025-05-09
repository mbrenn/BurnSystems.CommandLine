namespace BurnSystems.CommandLine
{
    /// <summary>
    /// Stores the information to a specific argument, 
    /// that will be automatically parsed
    /// </summary>
    internal abstract class ArgumentInfo
    {
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
