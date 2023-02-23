using System.ComponentModel;
using System.Globalization;
using Microsoft.Language.Xml;

namespace FusionCore.Helpers.DocumentoXml
{
    public class XmlElementValue : IXmlElementValue
    {
        private readonly IXmlElement _element;
        public bool HasValue => _element != null;

        public XmlElementValue(IXmlElement element = null)
        {
            _element = element;
        }

        public T GetValueOrDefault<T>()
        {
            if (_element?.Value == null)
            {
                return default(T);
            }

            var converter = TypeDescriptor.GetConverter(typeof (T));
            return (T) converter.ConvertFromString(_element.Value);
        }

        public decimal GetDecimalValue()
        {
            return _element?.Value == null
                ? decimal.Zero
                : decimal.Parse(_element.Value, new NumberFormatInfo {NumberDecimalSeparator = "."});
        }

        public string GetValueOrEmpty()
        {
            return GetValueOrDefault<string>() ?? string.Empty;
        }
    }
}