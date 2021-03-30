using CrossCutting.Utilities.XmlDocumentTransformation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CrossCutting.Utilities.XmlDocumentTransformation
{
    public class XDocumentTransformer<T>
    {
        private readonly IXDocumentTransformComponent<T>[] _transformers;

        public XDocumentTransformer(IEnumerable<IXDocumentTransformComponent<T>> transformers)
        {
            _transformers = transformers?.ToArray() ?? throw new ArgumentNullException(nameof(transformers));
        }

        public XDocument Transform(XDocument input, T parameters)
        {
            var result = new XDocument(input);

            foreach (var transformer in _transformers)
            {
                transformer.Transform(result, parameters);
            }

            return result;
        }
    }
}
