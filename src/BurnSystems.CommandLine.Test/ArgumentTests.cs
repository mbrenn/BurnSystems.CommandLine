using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.CommandLine.Test
{
    [TestFixture]
    public class ArgumentTests
    {
        [Test]
        public void TestNamedArgumentsWithValue()
        {
            var args = new string[] { "--input", "input.txt", "--output", "output.txt" };
            var evaluator = new Parser(args)
                 .WithArgument("input", hasValue: true)
                 .WithArgument("output", hasValue: true);

            Assert.That(evaluator.NamedArguments.Count, Is.EqualTo(2));
            Assert.That(evaluator.UnnamedArguments.Count, Is.EqualTo(0));

            Assert.That(evaluator.NamedArguments["input"], Is.EqualTo("input.txt"));
            Assert.That(evaluator.NamedArguments["output"], Is.EqualTo("output.txt"));
        }

        [Test]
        public void TestShortName()
        {
            var args = new string[] { "-i", "input.txt", "-o", "output.txt" };
            var evaluator = new Parser(args)
                .WithArgument("input", hasValue: true, shortName: 'i')
                .WithArgument("output", hasValue: true, shortName: 'o');

            Assert.That(evaluator.NamedArguments.Count, Is.EqualTo(2));
            Assert.That(evaluator.UnnamedArguments.Count, Is.EqualTo(0));

            Assert.That(evaluator.NamedArguments["input"], Is.EqualTo("input.txt"));
            Assert.That(evaluator.NamedArguments["output"], Is.EqualTo("output.txt"));
        }

        [Test]
        public void TestIncompleteNamedAndUnnamedArguments()
        {
            var args = new string[] { "--input", "input.txt", "--output" };
            var evaluator = new Parser(args)
                 .WithArgument("input", hasValue: true)
                 .WithArgument("output", hasValue: true);
            Assert.That(evaluator.ParseOrShowUsage(), Is.False);
        }

        [Test]
        public void TestUsageWithNamedArgument()
        {
            var args = new string[] { "--input", "input.txt", "--output" };
            var evaluator = new Parser(args)
                .WithArgument("input", hasValue: true, helpText: "Secret")
                .WithArgument("output", hasValue: true);

            using (var writer = new StringWriter())
            {
                evaluator.WriteUsage(writer);
                
                var usageText = writer.GetStringBuilder().ToString();
                Assert.That(usageText.Contains("input"), Is.True);
                Assert.That(usageText.Contains("output"), Is.True);
                Assert.That(usageText.Contains("Secret"), Is.True);
            }
        }

        [Test]
        public void TestUsageWithUnamedArgument()
        {
            var args = new string[] { };
            var evaluator = new Parser(args)
                .WithArgument(1, helpText: "Secret")
                .WithArgument(2);

            using (var writer = new StringWriter())
            {
                evaluator.WriteUsage(writer);

                var usageText = writer.GetStringBuilder().ToString();
                Assert.That(usageText.Contains("Secret"), Is.True);
            }
        }

        [Test]
        public void TestUsageWithNoArgument()
        {
            var args = new string[] { "--input", "input.txt", "--output" };
            var evaluator = new Parser(args);

            using (var writer = new StringWriter())
            {
                evaluator.WriteUsage(writer);

                var usageText = writer.GetStringBuilder().ToString();
                Assert.That(usageText.Length, Is.GreaterThan(0));
            }
        }
    }
}
