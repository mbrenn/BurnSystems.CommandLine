using BurnSystems.CommandLine.ByAttributes;

namespace ConsoleApp
{
    public class ProgramArguments
    {
        [UnnamedArgument(IsRequired = true, HelpText = "Path which will be compiled")]
        public string Input { get; set; }

        [UnnamedArgument(IsRequired = true, HelpText = "Path where compiled content will be stored.")]
        public string Output { get; set; }

        [NamedArgument(ShortName = 'v', HelpText = "Prints out more information")]
        public bool Verbose { get; set; }

        [NamedArgument(ShortName = 'd', HelpText = "Duration of the simulation")]
        public int Duration { get; set; }
    }
}
