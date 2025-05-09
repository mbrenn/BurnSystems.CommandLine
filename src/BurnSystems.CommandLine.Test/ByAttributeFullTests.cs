using BurnSystems.CommandLine.ByAttributes;
using NUnit.Framework;

namespace BurnSystems.CommandLine.Test
{
    [TestFixture]
    public class ByAttributeFullTests
    {
        [Test]
        public void TestNoAttributes()
        {
            var args = new[] { "--Input", "input.txt", "--Output", "output.txt" };

            var result = Parser.ParseIntoOrShowUsage<NoAttributes>(args);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullDetail, Is.False);
            Assert.That(result.Verbose, Is.False);
            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.Output, Is.EqualTo("output.txt"));

            args = ["--Input", "input.txt", "--Output", "output.txt", "--Verbose"];

            result = Parser.ParseIntoOrShowUsage<NoAttributes>(args);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullDetail, Is.False);
            Assert.That(result.Verbose, Is.True);
            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.Output, Is.EqualTo("output.txt"));
        }

        [Test]
        public void TestSmallLettersAttributes()
        {
            var args = new[] { "--input", "input.txt", "--output", "output.txt" };

            var result = Parser.ParseIntoOrShowUsage<NoAttributes>(args);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullDetail, Is.False);
            Assert.That(result.Verbose, Is.False);
            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.Output, Is.EqualTo("output.txt"));

            args = new[] { "--input", "input.txt", "--output", "output.txt", "--Verbose" };

            result = Parser.ParseIntoOrShowUsage<NoAttributes>(args);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullDetail, Is.False);
            Assert.That(result.Verbose, Is.True);
            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.Output, Is.EqualTo("output.txt"));
        }

        [Test]
        public void TestNoAttributesWithShortName()
        {
            var args = new[] { "--Input", "input.txt", "--Output", "output.txt", "-vf" };

            var result = Parser.ParseIntoOrShowUsage<AttributesShortName>(args);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullDetail, Is.True);
            Assert.That(result.Verbose, Is.True);
            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.Output, Is.EqualTo("output.txt"));

            args = new[] { "--Input", "input.txt", "--Output", "output.txt", "-f" };

            result = Parser.ParseIntoOrShowUsage<AttributesShortName>(args);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullDetail, Is.True);
            Assert.That(result.Verbose, Is.False);
            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.Output, Is.EqualTo("output.txt"));
        }

        [Test]
        public void TestNoAttributesWithDefaultValue()
        {
            var args = new[] { "--Input", "input.txt", "-vf" };

            var result = Parser.ParseIntoOrShowUsage<AttributesDefaultValue>(args);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullDetail, Is.True);
            Assert.That(result.Verbose, Is.True);
            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.Output, Is.EqualTo("no.txt"));

            args = new[] { "--Input", "input.txt", "--Output", "output.txt", "-f" };

            result = Parser.ParseIntoOrShowUsage<AttributesDefaultValue>(args);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullDetail, Is.True);
            Assert.That(result.Verbose, Is.True);
            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.Output, Is.EqualTo("output.txt"));
        }

        [Test]
        public void TestUnnamedArgumentAttribute()
        {
            var args = new[] { "input.txt", "output.txt", "-vf" };
            var result = Parser.ParseIntoOrShowUsage<UnnamedArguments>(args);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullDetail, Is.True);
            Assert.That(result.Verbose, Is.True);
            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.Output, Is.EqualTo("output.txt"));

            args = new[] { "input.txt", "-f" };
            result = Parser.ParseIntoOrShowUsage<UnnamedArguments>(args);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullDetail, Is.True);
            Assert.That(result.Verbose, Is.False);
            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.Output, Is.Null.Or.Empty);

            args = new[] { "input.txt", "-v" };
            result = Parser.ParseIntoOrShowUsage<UnnamedArguments>(args);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullDetail, Is.False);
            Assert.That(result.Verbose, Is.True);
            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.Output, Is.Null.Or.Empty);

            args = new[] { "-f" };
            result = Parser.ParseIntoOrShowUsage<UnnamedArguments>(args);
            Assert.That(result, Is.Null);
        }

        public class NoAttributes
        {
            public string Input { get; set; }

            public string Output { get; set; }

            public bool Verbose { get; set; }

            public bool FullDetail { get; set; }
        }

        public class AttributesShortName
        {
            public string Input { get; set; }

            public string Output { get; set; }

            [NamedArgument(ShortName = 'v')]
            public bool Verbose { get; set; }

            [NamedArgument(ShortName = 'f')]
            public bool FullDetail { get; set; }
        }

        public class AttributesDefaultValue
        {
            public string Input { get; set; }

            [NamedArgument(DefaultValue = "no.txt")]
            public string Output { get; set; }

            [NamedArgument(DefaultValue = "1")]
            public bool Verbose { get; set; }

            [NamedArgument(ShortName = 'f')]
            public bool FullDetail { get; set; }
        }

        public class UnnamedArguments
        {
            [UnnamedArgument(Index = 0, IsRequired = true)]
            public string Input { get; set; }

            [UnnamedArgument(Index = 1, IsRequired = false)]
            public string Output { get; set; }

            [NamedArgument(ShortName = 'v')]
            public bool Verbose { get; set; }

            [NamedArgument(ShortName = 'f')]
            public bool FullDetail { get; set; }
        }
    }
}
