using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using CrossCutting.Utilities.XmlDocumentTransformation.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Utilities.XmlDocumentTransformation.Tests
{
    [ExcludeFromCodeCoverage]
    public class XmlDocumentTransformerTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_On_Null_Argument()
        {
            this.Invoking(_ => new XmlDocumentTransformer<object>(transformers: null))
                .Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("source");
        }

        [Fact]
        public void Constructor_Does_Not_Throw_Exception_On_Empty_Argument()
        {
            this.Invoking(_ => new XmlDocumentTransformer<object>(transformers: Array.Empty<IXmlDocumentTransformComponent<object>>()))
                .Should().NotThrow();
        }

        [Fact]
        public void Constructor_Does_Not_Throw_Exception_On_Non_Empty_Argument()
        {
            this.Invoking(_ => new XmlDocumentTransformer<object>(transformers: new[] { new Mock<IXmlDocumentTransformComponent<object>>().Object }))
                .Should().NotThrow();
        }

        [Fact]
        public void Transform_Returns_Transformed_Document()
        {
            // Arrange
            var componentMock = new Mock<IXmlDocumentTransformComponent<object>>();
            componentMock.Setup(x => x.Transform(It.IsAny<XmlDocument>(), It.IsAny<object>()))
                         .Callback<XmlDocument, object>((doc, _) => doc.DocumentElement.SetAttribute("processed", "true"));
            var input = new XmlDocument();
            input.LoadXml(@"<Element processed=""false"" />");
            var sut = new XmlDocumentTransformer<object>(new[] { componentMock.Object });

            // Act
            var result = sut.Transform(input, null);

            // Assert
            input.DocumentElement.Attributes.GetNamedItem("processed").Value.Should().Be("false", "Source data should not have been modified");
            result.DocumentElement.Attributes.GetNamedItem("processed").Value.Should().Be("true", "Output data should have been modified");
        }
    }
}
