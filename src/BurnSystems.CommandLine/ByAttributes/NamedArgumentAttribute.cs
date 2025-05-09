namespace BurnSystems.CommandLine.ByAttributes
{
    /// <summary>
    /// Replicates the information from the NamedArgumentInfo so 
    /// it can be used within an attribute to configure the program arguments type
    /// </summary>
    public class NamedArgumentAttribute : ArgumentInfoAttribute
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
