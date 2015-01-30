using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.CommandLine.Test
{
    [TestFixture]
    public class FilterTests
    {
        [Test]
        public void TestOptional()
        {
            var arguments = new string[] { "-f", "file1.txt" };
            var evaluator = new CommandLineEvaluator(arguments)
                .WithDefaultValue("g", "great");

            Assert.That(evaluator.NamedArguments.Count(), Is.EqualTo(2));
            Assert.That(evaluator.NamedArguments.ContainsKey("g"), Is.True);
            Assert.That(evaluator.NamedArguments.ContainsKey ( "f"), Is.True);
            Assert.That(evaluator.NamedArguments["g"], Is.EqualTo("great"));
            Assert.That(evaluator.UnnamedArguments.Count, Is.EqualTo(1));
            Assert.That(evaluator.UnnamedArguments[0], Is.EqualTo("file1.txt"));
        }

        [Test]
        public void TestRequiredSuccess()
        {
            var arguments = new string[] { "-f", "file1.txt" };
            var evaluator = new CommandLineEvaluator(arguments)
                .Requires("f");

            Assert.That(evaluator.NamedArguments.Count(), Is.EqualTo(1));
            Assert.That(evaluator.NamedArguments.ContainsKey("g"), Is.False);
            Assert.That(evaluator.NamedArguments.ContainsKey("f"), Is.True);
            Assert.That(evaluator.UnnamedArguments.Count, Is.EqualTo(1));
            Assert.That(evaluator.UnnamedArguments[0], Is.EqualTo("file1.txt"));
        }

        [Test]
        public void TestRequiredFail()
        {
            var arguments = new string[] { "-f", "file1.txt" };
            var evaluator = new CommandLineEvaluator(arguments)
                .Requires("g");

            Assert.Throws<ArgumentParseException>(() => evaluator.NamedArguments.Count());
        }
    }
}
