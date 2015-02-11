# BurnSystems.CommandLine

The commandline parser takes the arguments as given from the Main(string[] args)-function and will prepare
them for easy access. It supports an argument syntax including switches, named arguments or 
filespecifications. 

Installation via NuGet: 

> Install-Package BurnSystems.CommandLine 

# Attribute driven usage

The arguments in command line can directly be parsed into a .Net Object

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

    static void Main(string[] args)
    {
        var argument = Parser.ParseIntoOrShowUsage<ProgramArguments>(args);
        if (argument != null)
        {
            Console.WriteLine("Input: " + argument.Input);
            Console.WriteLine("Output: " + argument.Output);
            Console.WriteLine("Verbose: " + argument.Verbose);
            Console.WriteLine("Duration: " + argument.Duration);
        }
    }

## Call of application

    > ConsoleApp.exe input.txt output.txt -v -d 10

    Input: input.txt
    Output: output.txt
    Verbose: True
    Duration: 10

## Minumum configuration

The parameter class can also be used without or with a minimum of configuration.

    public class ProgramArguments
    {
        [UnnamedArgument()]
        public string Input { get; set; }

        [UnnamedArgument()]
        public string Output { get; set; }

        public bool Verbose { get; set; }

        public int Duration { get; set; }
    }

Properties without attribute will be considered as named arguments. The unnamed arguments
will be included in the order within the class. 

    > ConsoleApp.exe input.txt output.txt --verbose --duration 10

The properties within the attribute can be used to add some additional constraints and 
default values. 

When the application is called with invalid arguments or with the parameter '-?', 
an automatic usage text will be created and null will be returned from the method ParseIntoOrShowUsage.

    > ConsoleApp.exe -?

    ConsoleApp.exe {options}
    Version: 1.1.0.0

    The given arguments were incomplete:
      Not enough arguments were given. 2 arguments were expected

    Options:
        Argument 1        : Path which will be compiled
        Argument 2        : Path where compiled content will be stored.
        --verbose [-v]    : Prints out more information
        --duration [-d]   : Duration of the simulation

# Manual Usage

For manual configuration without using attributed .Net-Object, the Parser can also be used directly

The commandline can be simply used: 

        static void Main(string[] args)
        {
            var parser = new Parser(args)
                .WithArgument("input", hasValue: true, helpText: "Secret", isRequired: true)
                .WithArgument("output", hasValue: true);

            if (parser.ParseOrShowUsage())
            {
                Console.WriteLine(parser.NamedArguments["input"]);
                Console.WriteLine("Success");
            }
        }

The main syntax for an application without any argument-configuration is given as the following: 

    consoleapp.exe --input file1.txt --output file2.txt abc.cfg -o 

The unnamed arguments, which are retrievable via **parser.UnnamedArguments as List<string>** will contain
one element: abc.cfg

The named arguments, which are retrievable via **parser.NamedArguments as Dictionary<string, string>**
will contain four elements: input="file1.txt", output="file2.txt" and o="1".

## Argument definition

### Named Argument with values

    consoleapp.exe --input input.txt --output output.txt
    
Per default, the commandline parser will return two named arguments ("input", "output") as "1" and
will contain two unnamed arguments ("input.txt", "output.txt").

It is necessary to give the command line parser a hint, so it connects the input argument with the
"input.txt".

    public static void Main(string[] args) {
       var parser = new Parser(args)
        .WithArgument("input", hasValue: true)
        .WithArgument("output", hasValue: true);
    }

This will make the parser return the following two named arguments: 

* input: input.txt
* output: output.txt

## Short names

By defining the short name for an option, which consists of one character, the length of the 
arguments can be shortened.

    var parser = new Parser(args)
        .WithArgument("input", hasValue: true, shortName: 'i')
        .WithArgument("output", hasValue: true, shortName: 'o')
		.WithArgument("verbose", shortname: 'v')
		.WithArgument("standard", shortname: 's');

The following statement will be evaluated with the long names: 

    consoleapp.exe -i input.txt -o output.txt -sv

* input: input.txt
* output: output.txt
* verbose: 1
* standard: 1

## Mandatory Value

To configure a mandatory NamedArgument, the following extension method can be used:

    var parser = new Parser(arguments)
        .Requires("f");
        
If the application is executed without the given attribute, an exception will be thrown.

## DefaultValue

To configure a default value for an option, when it is not set via application, the following 
extension method can be used: 

    var parser = new Parser(arguments)
        .WithDefaultValue("g", "great");
        
When there is no named argument, the named argument will contain the value "great". 
