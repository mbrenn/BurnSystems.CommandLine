using BurnSystems.CommandLine.ByAttributes;
using NUnit.Framework;
using System.Linq;

namespace BurnSystems.CommandLine.Test
{
    [TestFixture]
    public class ByAttributeUnitTests
    {
        [Test]
        public void TestConversionOfAttributes()
        {
            var args = new string[] { };
            var attributeParser = new ByAttributeParser<Parameters>();
            var parser = attributeParser.PrepareParser(args);

            Assert.That(parser, Is.Not.Null);

            var infos = parser.ArgumentInfos.ToList();
            Assert.That(infos.Count(), Is.EqualTo(2));

            var namedArgumentInfos =
                infos.Where(x => x is NamedArgumentInfo).ToList();
            var unnamedArgumentInfos = 
                infos.Where(x => x is UnnamedArgumentInfo).ToList();

            Assert.That(unnamedArgumentInfos.Count(), Is.EqualTo(1));
            Assert.That(namedArgumentInfos.Count(), Is.EqualTo(1));

            var namedArgument = namedArgumentInfos.First() as NamedArgumentInfo;
            var unnamedArgument = unnamedArgumentInfos.First() as UnnamedArgumentInfo;

            Assert.That(namedArgument, Is.Not.Null);
            Assert.That(namedArgument.ShortName, Is.EqualTo('v'));
            Assert.That(namedArgument.DefaultValue, Is.EqualTo("1"));
            Assert.That(namedArgument.HelpText, Is.EqualTo("Input text"));
            Assert.That(unnamedArgument, Is.Not.Null);
            Assert.That(unnamedArgument.Index, Is.EqualTo(1));
            Assert.That(unnamedArgument.DefaultValue, Is.EqualTo("Default Value"));
            Assert.That(unnamedArgument.HelpText, Is.EqualTo("Input text"));
        }

        [Test]
        public void TestUnnamedParmeters()
        {
            var args = new[] { "input", "output" };

            var attributeParser = new ByAttributeParser<TwoUnnamedParameters>();
            var parser = attributeParser.PrepareParser(args);
            Assert.That(parser.UnnamedArgumentInfos.Count(), Is.EqualTo(2));
            Assert.That(parser.UnnamedArgumentInfos.ElementAt(0).Index, Is.EqualTo(0));
            Assert.That(parser.UnnamedArgumentInfos.ElementAt(1).Index, Is.EqualTo(1));

            Assert.That(parser.NamedArgumentInfos.Count(), Is.EqualTo(0));

            var result = attributeParser.FillObject();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Input, Is.EqualTo("input"));
            Assert.That(result.Output, Is.EqualTo("output"));

            // Try to provoke an error
            args = [];
            attributeParser = new ByAttributeParser<TwoUnnamedParameters>();
            parser = attributeParser.PrepareParser(args);
            var parseResult = parser.ParseOrShowUsage();
            result = attributeParser.FillObject();
            Assert.That(parseResult, Is.False);
            Assert.That(parser.Errors.Count, Is.EqualTo(1));
        }

        class Parameters
        {
            [NamedArgument(ShortName = 'v', HelpText = "Input text", DefaultValue = "1")]
            public bool Verbose { get; set; }

            [UnnamedArgument(Index = 1, HelpText = "Input text", DefaultValue = "Default Value")]
            public string Input { get; set; }
        }

        class TwoUnnamedParameters
        {
            [UnnamedArgument(IsRequired = true)]
            public string Input { get; set; }

            [UnnamedArgument(IsRequired = true)]
            public string Output { get; set; }
        }
    }
}
