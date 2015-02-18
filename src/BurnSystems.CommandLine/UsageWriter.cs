using BurnSystems.CommandLine.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.CommandLine
{
    /// <summary>
    /// Performs the usage writer
    /// </summary>
    internal class UsageWriter
    {
        /// <summary>
        /// Stores the parser
        /// </summary>
        private Parser parser;

        /// <summary>
        /// Initializes a new instance of the UsageWriter class.
        /// </summary>
        /// <param name="parser">Parser to be used</param>
        internal UsageWriter(Parser parser)
        {
            this.parser = parser;
        }

        /// <summary>
        /// Shows the exception and the usage argument
        /// </summary>
        /// <param name="exc">Exception being used</param>
        internal void ShowUsageAndException()
        {
            using (var writer = new StringWriter())
            {
                this.WriteIntroduction(writer);
                this.WriteErrors(writer);
                this.WriteUsage(writer);

                Console.WriteLine(writer.GetStringBuilder().ToString());
            }
        }

        /// <summary>
        /// Shows the usage
        /// </summary>
        internal void ShowUsage()
        {
            using (var writer = new StringWriter())
            {
                this.WriteIntroduction(writer);
                this.WriteUsage(writer);

                Console.WriteLine(writer.GetStringBuilder().ToString());
            }
        }

        internal void WriteIntroduction(TextWriter writer)
        {
            var options = string.Empty;

            if (this.parser.ArgumentInfos.Count() > 0)
            {
                options = " {options}";
            }

            var assembly = Assembly.GetEntryAssembly();

            if (assembly != null)
            {
                writer.WriteLine(
                    string.Format("{0}{1}",
                        Path.GetFileName(Assembly.GetEntryAssembly().Location),
                        options));

                writer.WriteLine("Version: {0}", assembly.GetName().Version.ToString());
            }
        }

        internal void WriteUsage(TextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("Options: ");

            // Finds the maximum length of the argument
            var argumentInfos = this.parser.ArgumentInfos.ToList();
            int maxLength = 0;

            if (argumentInfos.Count() > 0)
            {
                maxLength =
                    Math.Max(
                        maxLength,
                        argumentInfos.Max(x => x.ToString().Length));
            }

            // Gets the maximum length of the arguments
            foreach (var argumentInfo in argumentInfos
                .OrderBy(x => x is NamedArgumentInfo ? 1 : 0))
            {
                if (!string.IsNullOrEmpty(argumentInfo.HelpText))
                {
                    var argument = StringManipulation.PaddingRight(argumentInfo.ToString(), maxLength + 3);
                    writer.WriteLine(
                        IndentedFormat(
                            string.Format(
                                "    {0}: {1}",
                                argument,
                                argumentInfo.HelpText),
                        argument.Length + 6));
                }
                else
                {
                    writer.WriteLine(
                        string.Format(
                            "    {0}",
                            StringManipulation.PaddingRight(argumentInfo.ToString(), maxLength + 3)));
                }
            }
        }

        /// <summary>
        /// Converts the given text to an indented text
        /// </summary>
        /// <param name="text">Text to be converted</param>
        /// <param name="indent">Indentation to be done</param>
        /// <returns>Returned the indented text that can be outputed to the console</returns>
        private string IndentedFormat(string text, int indent, int width = 0)
        {
            if (width <= 0)
            {
                width = Console.IsErrorRedirected ? Console.BufferWidth : 80;
            }

            var buffer = new StringBuilder();

            // Getsthe text
            var indentTextBuffer = new StringBuilder();
            for (var n = 0; n < indent; n++)
            {
                indentTextBuffer.Append(' ');
            }

            var indentText = indentTextBuffer.ToString();

            // Now doing the text creation
            var currentPosition = 0;
            while (currentPosition < text.Length)
            {
                var lineWidth = currentPosition == 0 ? width : width - indent;
                var restLength = text.Length - currentPosition;

                var nextLength = Math.Min(lineWidth, restLength);

                if (currentPosition == 0)
                {
                    buffer.Append(
                        text.Substring(currentPosition, nextLength));
                }
                else
                {
                    buffer.Append(indentText + text.Substring(currentPosition, nextLength));
                }

                currentPosition += nextLength;
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Writes the errors given during the parsing
        /// </summary>
        /// <param name="writer">Writer to be used</param>
        public void WriteErrors(TextWriter writer)
        {
            if (this.parser.Errors.Count > 0)
            {
                writer.WriteLine();
                writer.WriteLine("The given arguments were incomplete:");
            }

            foreach (var error in this.parser.Errors)
            {
                writer.WriteLine("  " + error);
            }
        }
    }
}
