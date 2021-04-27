using System.Collections.Generic;
using System.Linq;
using System.Xml;
using CrossCutting.Utilities.XmlDocumentTransformation.Interfaces;

namespace CrossCutting.Utilities.XmlDocumentTransformation
{
    public class XmlDocumentTransformer<T>
    {
        private readonly IXmlDocumentTransformComponent<T>[] _transformers;

        public XmlDocumentTransformer(IEnumerable<IXmlDocumentTransformComponent<T>> transformers)
        {
            _transformers = transformers.ToArray();
        }

        public XmlDocument Transform(XmlDocument input, T parameters)
        {
            var result = new XmlDocument();
            result.LoadXml(input.OuterXml);

            foreach (var transformer in _transformers)
            {
                transformer.Transform(result, parameters);
            }

            return result;
        }
    }
}
