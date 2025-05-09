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
            if (string.IsNullOrEmpty(LongName))
            {
                return "Unknown Argument";
            }

            return ShortName == '\0'
                ? $"--{LongName.ToLower()}"
                : $"--{LongName.ToLower()} [-{ShortName}]";
        }
    }
}
