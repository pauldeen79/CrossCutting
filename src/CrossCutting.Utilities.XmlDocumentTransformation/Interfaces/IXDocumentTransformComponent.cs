using System.Xml.Linq;

namespace CrossCutting.Utilities.XmlDocumentTransformation.Interfaces
{
    public interface IXDocumentTransformComponent<T>
    {
        void Transform(XDocument result, T parameters);
    }
}
