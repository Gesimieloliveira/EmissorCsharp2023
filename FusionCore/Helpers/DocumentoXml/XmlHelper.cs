using System.Collections.Generic;
using System.Xml;
using Microsoft.Language.Xml;
using NHibernate.Util;

namespace FusionCore.Helpers.DocumentoXml
{
    public class XmlHelper
    {
        private readonly XmlDocumentSyntax _parser;
        private readonly XmlDocument _xml;

        public XmlHelper(string xmlText)
        {
            _parser = Parser.ParseText(xmlText);

            _xml = new XmlDocument();
            _xml.LoadXml(xmlText);
        }

        public IXmlElementValue GetValueFromElement(string name, string parentName = null)
        {
            var found = GetValue(_parser.Elements, name, parentName);
            return found ?? new XmlElementValue();
        }

        private static IXmlElementValue GetValue(IEnumerable<IXmlElement> elements, string name, string parentName)
        {
            foreach (var e in elements)
            {
                if (parentName == null && e.Name == name)
                    return new XmlElementValue(e);

                if (e.Parent.Name == parentName && e.Name == name)
                    return new XmlElementValue(e);

                var node = e as XmlElementSyntax;
                var contentKind = node?.Content.Kind;

                if (contentKind != SyntaxKind.List && contentKind != SyntaxKind.XmlElement)
                    continue;

                IEnumerable<IXmlElement> childs = null;

                if (contentKind == SyntaxKind.List)
                    childs = node.Elements;

                if (contentKind == SyntaxKind.XmlElement)
                    childs = (node.Content as XmlElementSyntax)?.Elements;

                if (childs == null) continue;

                var value = GetValue(childs, name, parentName);
                if (value != null) return value;
            }

            return null;
        }

        public string PegaElemento(string nome)
        {
            var nodelist = _xml.GetElementsByTagName(nome);
            var first = (XmlNode) nodelist.FirstOrNull();

            return first?.InnerText;
        }
    }
}