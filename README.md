# BurnSystems.CommandLine

The commandline parser takes the arguments as given from the Main(string[] args)-function and will prepare them for easy access. It supports an argument syntax including switches, named arguments or filespecifications. 

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

The unnamed arguments, which are retrievable via **parser.UnnamedArguments as List<string>** will contain one element: abc.cfg

The named arguments, which are retrievable via **parser.NamedArguments as Dictionary<string, string>** will contain four elements: input="file1.txt", output="file2.txt" and o="1".

## Argument definition

### Named Argument with values

    consoleapp.exe --input input.txt --output output.txt
    
Per default, the commandline parser will return two named arguments ("input", "output") as "1" and will contain two unnamed arguments ("input.txt", "output.txt").

It is necessary to give the command line parser a hint, so it connects the input argument with the "input.txt".

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

To configure a default value for an option, when it is not set via application, the following extension method can be used: 

    var parser = new Parser(arguments)
        .WithDefaultValue("g", "great");
        
When there is no named argument, the named argument will contain the value "great". 
