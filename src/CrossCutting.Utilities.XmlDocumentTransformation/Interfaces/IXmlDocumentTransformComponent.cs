using System.Xml;

namespace CrossCutting.Utilities.XmlDocumentTransformation.Interfaces
{
    public interface IXmlDocumentTransformComponent<T>
    {
        void Transform(XmlDocument result, T parameters);
    }
}
