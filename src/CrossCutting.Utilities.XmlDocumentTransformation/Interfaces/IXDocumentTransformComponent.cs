using System.Xml.Linq;

namespace CrossCutting.Utilities.XmlDocumentTransformation.Interfaces
{
    public interface IXDocumentTransformComponent<in T>
    {
        void Transform(XDocument result, T parameters);
    }
}
