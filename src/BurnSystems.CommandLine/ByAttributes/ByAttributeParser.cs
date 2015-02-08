using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.CommandLine.ByAttributes
{
    /// <summary>
    /// Creates a parses by an object class and returns the values within the
    /// object by using the result of the parsing
    /// </summary>
    public class ByAttributeParser<T> where T: new()
    {
        /// <summary>
        /// Stores a value, which is set to true, after the call <c>PrepareParser</c>.
        /// </summary>
        private Parser parser = null;

        /// <summary>
        /// Stores the list of action, that will be 
        /// executed when the FillObject method will be called
        /// </summary>
        private List<ParseAction> actions = null;

        /// <summary>
        /// Creates a parser instance and configures the parser for the properties in the given class T.
        /// In addition, action methods are created, which support the method ParseIntoObject
        /// </summary>
        /// <param name="args">Arguments to be parsed</param>
        /// <param name="filters">Filters to be added to the parser. May be null, to add no additional
        /// filters</param>
        /// <returns>The created and preconfigured parser</returns>
        public Parser PrepareParser(string[] args, IEnumerable<ICommandLineFilter> filters = null)
        {
            this.parser = new Parser(args);
            this.actions = new List<ParseAction>();

            var type = typeof(T);
            if (type.IsPrimitive)
            {
                throw new InvalidOperationException("Parsing into primitives is not supported");
            }

            // Go through the properties
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {   
                // Per default, a named argument info is created
                var argumentInfo = new NamedArgumentInfo() as ArgumentInfo;

                // Gets the attribute
                var attributeNamedArgument = property.GetCustomAttribute(typeof(NamedArgumentAttribute)) as NamedArgumentAttribute;
                var attributeUnnamedArgument = property.GetCustomAttribute(typeof(UnnamedArgumentAttribute)) as UnnamedArgumentAttribute;
                ArgumentInfoAttribute attributeArgument = 
                    attributeNamedArgument == null ? 
                        attributeUnnamedArgument as ArgumentInfoAttribute :
                        attributeNamedArgument as ArgumentInfoAttribute;

                // Defines the getter to retrieve the data
                Func<string> getter;

                if (argumentInfo is NamedArgumentInfo)
                {
                    var namedArgumentInfo = argumentInfo as NamedArgumentInfo;
                    namedArgumentInfo.LongName = property.Name;

                    if (attributeNamedArgument != null)
                    {
                        if (!string.IsNullOrEmpty(attributeNamedArgument.LongName))
                        {
                            namedArgumentInfo.LongName = attributeNamedArgument.LongName;
                        }

                        if (attributeNamedArgument.ShortName != '\0')
                        {
                            namedArgumentInfo.ShortName = attributeNamedArgument.ShortName;
                        }
                    }

                    getter = () => parser.NamedArguments[namedArgumentInfo.LongName];
                }
                else if (argumentInfo is UnnamedArgumentInfo)
                {
                    var unnamedArgumentInfo = argumentInfo as UnnamedArgumentInfo;
                    if (attributeUnnamedArgument != null)
                    {
                        if (attributeUnnamedArgument.Index != 0)
                        {
                            unnamedArgumentInfo.Index = attributeUnnamedArgument.Index;
                        }
                    }

                    getter = () => parser.UnnamedArguments[unnamedArgumentInfo.Index];
                }
                else
                {
                    throw new NotImplementedException("Unexpected argument info type: " + argumentInfo.GetType().ToString());
                }

                // Parses the rest of the attribute
                if (attributeArgument != null)
                {
                    if (!string.IsNullOrEmpty(attributeNamedArgument.HelpText))
                    {
                        argumentInfo.HelpText = attributeNamedArgument.HelpText;
                    }

                    if (!string.IsNullOrEmpty(attributeNamedArgument.DefaultValue))
                    {
                        argumentInfo.DefaultValue = attributeNamedArgument.DefaultValue;
                    }

                    argumentInfo.IsRequired = attributeNamedArgument.IsRequired;
                }

                // Now convert the type
                var propertyType = property.PropertyType;
                if (propertyType == typeof(bool))
                {
                    argumentInfo.HasValue = false;
                    this.AddAction(
                        argumentInfo,
                        (value) =>
                        {
                            property.SetValue(
                                value,
                                ConvertToBoolean(getter()));
                        });
                }
                else if (propertyType == typeof(string))
                {
                    argumentInfo.HasValue = true;
                    this.AddAction(
                        argumentInfo,
                        (value) =>
                        {
                            property.SetValue(
                                value,
                                getter());
                        });
                }
                else if (propertyType == typeof(int))
                {
                    argumentInfo.HasValue = true;
                    this.AddAction(
                        argumentInfo,
                        (value) =>
                        {
                            property.SetValue(
                                value,
                                Convert.ToInt32(
                                    getter(),
                                    CultureInfo.InvariantCulture));
                        });
                }
                else if (propertyType == typeof(double))
                {
                    argumentInfo.HasValue = true;
                    this.AddAction(
                        argumentInfo,
                        (value) =>
                        {
                            property.SetValue(
                                value,
                                Convert.ToDouble(
                                    getter(),
                                    CultureInfo.InvariantCulture));
                        });
                }
                else if (propertyType == typeof(long))
                {
                    argumentInfo.HasValue = true;
                    this.AddAction(
                        argumentInfo,
                        (value) =>
                        {
                            property.SetValue(
                                value,
                                Convert.ToInt64(
                                    getter(),
                                    CultureInfo.InvariantCulture));
                        });
                }
                else if (propertyType == typeof(TimeSpan))
                {
                    argumentInfo.HasValue = true;
                    this.AddAction(
                        argumentInfo,
                        (value) =>
                        {
                            property.SetValue(
                                value,
                                TimeSpan.FromSeconds(Convert.ToInt32(
                                    getter(),
                                    CultureInfo.InvariantCulture)));
                        });
                }
                else
                {
                    throw new InvalidOperationException("Unsupported type in the instance: " + propertyType.FullName);
                }

                parser.AddArgumentInfo(argumentInfo);
            }

            return this.parser;
        }

        private void AddAction(ArgumentInfo info, Action<T> action)
        {
            this.actions.Add(new ParseAction(info, action));
        }

        /// <summary>
        /// Converts a string to a boolean by using some default arguments
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>true, if the value represents a boolean with a value of true</returns>
        private static bool ConvertToBoolean(string value)
        {
            return value == "1" || value == "y" || value == "True";
        }

        /// <summary>
        /// Parses the retrieved object into the a new given object
        /// </summary>
        /// <returns>The object, receiving the arguments</returns>
        public T FillObject()
        {
            if (this.parser == null)
            {
                throw new InvalidOperationException("PrepareParser was not called before");
            }

            T result = new T();
            foreach (var action in this.actions)
            {
                try
                {
                    action.Action(result);
                }
                catch (Exception exc)
                {
                    this.parser.AddError("Error occured during parsing of '"+ action.ArgumentInfo.ToString() + "': " + exc.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Defines the actions to be parsed
        /// </summary>
        private class ParseAction
        {
            public ArgumentInfo ArgumentInfo
            {
                get;
                set;
            }

            public Action<T> Action
            {
                get;
                set;
            }

            public ParseAction(ArgumentInfo info, Action<T> action)
            {
                this.ArgumentInfo = info;
                this.Action = action;
            }
        }
    }
}