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
                var propertyType = property.PropertyType;

                // Per default, a named argument info is created
                var argumentInfo = new NamedArgumentInfo();
                argumentInfo.LongName = property.Name;

                if (propertyType == typeof(bool))
                {
                    argumentInfo.HasValue = false;
                    this.AddAction(
                        argumentInfo,
                        (value) =>
                        {
                            property.SetValue(
                                value,
                                ConvertToBoolean(this.parser.NamedArguments[argumentInfo.LongName]));
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
                                this.parser.NamedArguments[argumentInfo.LongName]);
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
                                    this.parser.NamedArguments[argumentInfo.LongName],
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
                                    this.parser.NamedArguments[argumentInfo.LongName],
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
                                    this.parser.NamedArguments[argumentInfo.LongName],
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
                                    this.parser.NamedArguments[argumentInfo.LongName],
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
        public T ParseIntoObject()
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
                    this.parser.AddError("Error occured during parsing of '"+ action.ArgumentInfo.ToString() + ": " + exc.Message);
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