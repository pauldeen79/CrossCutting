using System.Xml;

namespace CrossCutting.Utilities.XmlDocumentTransformation.Interfaces
{
    public interface IXmlDocumentTransformComponent<in T>
    {
        void Transform(XmlDocument result, T parameters);
    }
}
