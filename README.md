# BurnSystems.CommandLine

The commandline parser takes the arguments as given from the Main(string[] args)-function and will prepare them for easy access. It supports an argument syntax including switches, named arguments or filespecifications. 

The commandline can be simply used: 

    public static void Main(string[] args) {
       var evaluator = new CommandLineEvaluator(args);
    }

The main syntax for an application without any argument-configuration is given as the following: 

    consoleapp.exe file1.txt file2.txt -o -p -a --abc

The unnamed arguments, which are retrievable via **evaluator.UnnamedArguments as List<string>** will contain two elements: file1.txt, file2.txt

The name arguments, which are retrievable via **evaluator.NamedArguments as Dictionary<string, string>** will contain four elements: o="1", p="1", a="1" and abc="1".

## Argument definition

### Named Argument with values (Not implemented until now)

    consoleapp.exe --input input.txt --output output.txt
    
Per default, the commandline parser will return two named arguments ("input", "output") as "1" and will contain two unnamed arguments ("input.txt", "output.txt").

It is necessary to give the command line parser a hint, so it connects the input argument with the "input.txt".

    public static void Main(string[] args) {
       var evaluator = new CommandLineEvaluator(args)
        .WithArgument("input", hasValue: true)
        .WithArgument("output", hasValue: true);
    }

This will make the evaluator return the following two named arguments: 

* input: input.txt
* output: output.txt

## Short options

By defining the short name for an option, which consists of one character, the length of the 
arguments can be shortened.

    var evaluator = new CommandLineEvaluator(args)
        .WithArgument("input", hasValue: true, shortName: 'i')
        .WithArgument("output", hasValue: true, shortName: 'o');

The following statement will be evaluated with the long names: 

    consoleapp.exe -i input.txt -o output.txt

* input: input.txt
* output: output.txt

## Mandatory Value

To configure a mandatory NamedArgument, the following extension method can be used:

    var evaluator = new CommandLineEvaluator(arguments)
        .Requires("f");
        
If the application is executed without the given attribute, an exception will be thrown.

## DefaultValue

To configure a default value for an option, when it is not set via application, the following extension method can be used: 

    var evaluator = new CommandLineEvaluator(arguments)
        .WithDefaultValue("g", "great");
        
When there is no named argument, the named argument will contain the value "great". 
