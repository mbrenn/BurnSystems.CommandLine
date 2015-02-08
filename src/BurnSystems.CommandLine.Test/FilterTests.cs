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
            var evaluator = new Parser(arguments)
                .WithArgument("g", defaultValue: "great");

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
            var evaluator = new Parser(arguments)
                .WithArgument("f", isRequired: true);

            Assert.That(evaluator.NamedArguments.Count(), Is.EqualTo(1));
            Assert.That(evaluator.NamedArguments.ContainsKey("g"), Is.False);
            Assert.That(evaluator.NamedArguments.ContainsKey("f"), Is.True);
            Assert.That(evaluator.UnnamedArguments.Count, Is.EqualTo(1));
            Assert.That(evaluator.UnnamedArguments[0], Is.EqualTo("file1.txt"));
        }

        [Test]
        public void TestRequiredFailNamedArgument()
        {
            var arguments = new string[] { "-f", "file1.txt" };
            var evaluator = new Parser(arguments)
                .WithArgument("g", isRequired: true);

            Assert.That(evaluator.ParseOrShowUsage(), Is.False);
        }

        [Test]
        public void TestRequiredRequire1ExistingUnnamedArgument()
        {
            var arguments = new string[] { "-f", "file1.txt" };
            var evaluator = new Parser(arguments)
                .WithArgument(1, isRequired: true);

            Assert.That(evaluator.NamedArguments.Count(), Is.EqualTo(1));
            Assert.That(evaluator.NamedArguments.ContainsKey("f"), Is.True);
            Assert.That(evaluator.UnnamedArguments.Count, Is.EqualTo(1));
            Assert.That(evaluator.UnnamedArguments[0], Is.EqualTo("file1.txt"));
        }

        [Test]
        public void TestRequiredRequire1NonExistingUnnamedArgument()
        {
            var arguments = new string[] { "-f", "file1.txt" };
            var evaluator = new Parser(arguments)
                .WithArgument(2, isRequired: true);

            Assert.That(evaluator.ParseOrShowUsage(), Is.False);
        }

        [Test]
        public void TestRequiredFailNamedArgumentWithValue()
        {
            var arguments = new string[] { "-f", "file1.txt" };
            var evaluator = new Parser(arguments)
                .WithArgument("f", hasValue: true)
                .WithArgument(1, isRequired: true);

            Assert.That(evaluator.ParseOrShowUsage(), Is.False);
        }
    }
}
