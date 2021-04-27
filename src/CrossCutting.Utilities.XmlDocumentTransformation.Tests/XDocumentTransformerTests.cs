using System;
using System.Xml.Linq;
using CrossCutting.Utilities.XmlDocumentTransformation;
using CrossCutting.Utilities.XmlDocumentTransformation.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Utilities.XDocumentTransformation.Tests
{
    public class XDocumentTransformerTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_On_Null_Argument()
        {
            this.Invoking(_ => new XDocumentTransformer<object>(transformers: null))
                .Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("source");
        }

        [Fact]
        public void Constructor_Does_Not_Throw_Exception_On_Empty_Argument()
        {
            this.Invoking(_ => new XDocumentTransformer<object>(transformers: Array.Empty<IXDocumentTransformComponent<object>>()))
                .Should().NotThrow();
        }

        [Fact]
        public void Constructor_Does_Not_Throw_Exception_On_Non_Empty_Argument()
        {
            this.Invoking(_ => new XDocumentTransformer<object>(transformers: new[] { new Mock<IXDocumentTransformComponent<object>>().Object }))
                .Should().NotThrow();
        }

        [Fact]
        public void Transform_Returns_Transformed_Document()
        {
            // Arrange
            var componentMock = new Mock<IXDocumentTransformComponent<object>>();
            componentMock.Setup(x => x.Transform(It.IsAny<XDocument>(), It.IsAny<object>()))
                         .Callback<XDocument, object>((doc, _) => doc.Element("Element").SetAttributeValue("processed", "true"));
            var input = new XDocument(new XElement("Element", new XAttribute("processed", "false")));
            var sut = new XDocumentTransformer<object>(new[] { componentMock.Object });

            // Act
            var result = sut.Transform(input, null);

            // Assert
            input.Element("Element").Attribute("processed").Value.Should().Be("false", "Source data should not have been modified");
            result.Element("Element").Attribute("processed").Value.Should().Be("true", "Output data should have been modified");
        }

        [Fact]
        public void Can_Transform_With_External_Data()
        {
            // Arrange
            var componentMock = new Mock<IXDocumentTransformComponent<string>>();
            componentMock.Setup(x => x.Transform(It.IsAny<XDocument>(), It.IsAny<string>()))
                         .Callback<XDocument, string>((doc, parameters) => doc.Element("Element").SetAttributeValue("processed", parameters));
            var input = new XDocument(new XElement("Element", new XAttribute("processed", "false")));
            var sut = new XDocumentTransformer<string>(new[] { componentMock.Object });

            // Act
            var result = sut.Transform(input, "true");

            // Assert
            input.Element("Element").Attribute("processed").Value.Should().Be("false", "Source data should not have been modified");
            result.Element("Element").Attribute("processed").Value.Should().Be("true", "Output data should have been modified");
        }

        [Fact]
        public void Can_Transform_With_External_Processing_Data()
        {
            // Arrange
            var componentMock = new Mock<IXDocumentTransformComponent<XDocument>>();
            componentMock.Setup(x => x.Transform(It.IsAny<XDocument>(), It.IsAny<XDocument>()))
                         .Callback<XDocument, XDocument>((doc, parameters) =>
                            {
                                var resultElement = new XElement("Result");
                                foreach (var dataSourceItem in parameters.Element("DataSource").Element("Items").Elements("Item"))
                                {
                                    foreach(var sourceItem in doc.Elements())
                                    {
                                        var newItem = new XElement(sourceItem);
                                        foreach (var attribute in dataSourceItem.Attributes())
                                        {
                                            newItem.SetAttributeValue(attribute.Name, attribute.Value);
                                        }
                                        resultElement.Add(newItem);
                                    }
                                }
                                doc.RemoveNodes();
                                doc.Add(resultElement);
                            });
            var input = new XDocument(new XElement("Element", new XAttribute("processed", "false")));
            var sut = new XDocumentTransformer<XDocument>(new[] { componentMock.Object });
            var processingDocument = new XDocument(new XElement("DataSource", new XElement("Items"
                , new XElement("Item", new XAttribute("attribute1", "value1"), new XAttribute("attribute2", "value2"))
                , new XElement("Item", new XAttribute("attribute1", "value3"), new XAttribute("attribute2", "value4"))
                )));

            // Act
            var result = sut.Transform(input, processingDocument);

            // Assert
            result.ToString().Should().Be(@"<Result>
  <Element processed=""false"" attribute1=""value1"" attribute2=""value2"" />
  <Element processed=""false"" attribute1=""value3"" attribute2=""value4"" />
</Result>");
        }

        [Fact]
        public void Can_Transform_With_Internal_Processing_Data()
        {
            // Arrange
            var componentMock = new Mock<IXDocumentTransformComponent<object>>();
            componentMock.Setup(x => x.Transform(It.IsAny<XDocument>(), It.IsAny<object>()))
                         .Callback<XDocument, object>((doc, _) =>
                         {
                             var resultElement = new XElement("Result");
                             foreach (var dataSourceItem in doc.Element("DataSource").Element("Items").Elements("Item"))
                             {
                                 foreach (var sourceItem in doc.Element("DataSource").Element("SourceData").Elements())
                                 {
                                     var newItem = new XElement(sourceItem);
                                     foreach (var attribute in dataSourceItem.Attributes())
                                     {
                                         newItem.SetAttributeValue(attribute.Name, attribute.Value);
                                     }
                                     resultElement.Add(newItem);
                                 }
                             }
                             doc.RemoveNodes();
                             doc.Add(resultElement);
                         });
            var input = new XDocument(new XElement("DataSource", new XElement("Items"
                , new XElement("Item", new XAttribute("attribute1", "value1"), new XAttribute("attribute2", "value2"))
                , new XElement("Item", new XAttribute("attribute1", "value3"), new XAttribute("attribute2", "value4"))
                ), new XElement("SourceData", new XElement("Element", new XAttribute("processed", "false")))));
            var sut = new XDocumentTransformer<object>(new[] { componentMock.Object });

            // Act
            var result = sut.Transform(input, null);

            // Assert
            result.ToString().Should().Be(@"<Result>
  <Element processed=""false"" attribute1=""value1"" attribute2=""value2"" />
  <Element processed=""false"" attribute1=""value3"" attribute2=""value4"" />
</Result>");
        }
    }
}
