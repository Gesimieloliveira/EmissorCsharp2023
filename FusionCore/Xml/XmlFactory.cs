using System.Xml;

namespace FusionCore.Xml
{
    public static class XmlFactory
    {
        public static XmlDocument Cria(string xmlString)
        {
            var document = new XmlDocument();
            document.LoadXml(xmlString);

            return document;
        }

        public static XmlDocumentFragment CriaFragment(XmlDocument document)
        {
            var fragment = document.CreateDocumentFragment();
            fragment.InnerXml = document.OuterXml;

            return fragment;
        }
    }
}