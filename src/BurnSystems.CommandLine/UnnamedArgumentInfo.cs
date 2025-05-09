namespace BurnSystems.CommandLine
{
    /// <summary>
    /// Stores the information to a specific argument, 
    /// that will be automatically parsed
    /// </summary>
    internal class UnnamedArgumentInfo : ArgumentInfo
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

        public override string ToString()
        {
            return $"Argument {Index + 1}";
        }
    }
}
