using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.CommandLine.Test
{
    [TestFixture]
    public class CommandLineTests
    {
        [Test]
        public void TestEmpty()
        {
            var args = new string[] { };
            var evaluator = new Parser(args);
            Assert.That(evaluator.NamedArguments.Count, Is.EqualTo(0));
            Assert.That(evaluator.UnnamedArguments.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestUnnamedArguments()
        {
            var args = new string[] { "file1.txt", "file2.txt" };
            var evaluator = new Parser(args);
            Assert.That(evaluator.NamedArguments.Count, Is.EqualTo(0));
            Assert.That(evaluator.UnnamedArguments.Count, Is.EqualTo(2));
            Assert.That(evaluator.UnnamedArguments[0], Is.EqualTo("file1.txt"));
            Assert.That(evaluator.UnnamedArguments[1], Is.EqualTo("file2.txt"));
        }

        [Test]
        public void TestNamedArguments()
        {
            var args = new string[] { "-f", "-o" };
            var evaluator = new Parser(args);
            Assert.That(evaluator.NamedArguments.Count, Is.EqualTo(2));
            Assert.That(evaluator.UnnamedArguments.Count, Is.EqualTo(0));
            Assert.That(evaluator.NamedArguments.ContainsKey("f"), Is.True);
            Assert.That(evaluator.NamedArguments.ContainsKey("o"), Is.True);

            Assert.That(evaluator.NamedArguments.ContainsKey("a"), Is.False);
        }

        [Test]
        public void TestNamedAndUnnamedArguments()
        {
            var args = new string[] { "-f", "file1.txt", "-o", "file2.txt" };
            var evaluator = new Parser(args);
            Assert.That(evaluator.NamedArguments.Count, Is.EqualTo(2));
            Assert.That(evaluator.NamedArguments.ContainsKey("f"), Is.True);
            Assert.That(evaluator.NamedArguments.ContainsKey("o"), Is.True);
            Assert.That(evaluator.NamedArguments.ContainsKey("a"), Is.False);
            Assert.That(evaluator.UnnamedArguments.Count, Is.EqualTo(2));
            Assert.That(evaluator.UnnamedArguments[0], Is.EqualTo("file1.txt"));
            Assert.That(evaluator.UnnamedArguments[1], Is.EqualTo("file2.txt"));
        }
    }
}
