using System;
using System.Xml;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Fiscal.Contratos.Retorno
{
    public sealed class NfeRetorno
    {
        private XmlDocument _xmlEnvio;
        private XmlDocument _xmlRetorno;
        private XmlDocument _xmlRetornoCompleto;
        public string Envio { get; set; }
        public string Retorno { get; set; }
        public string RetornoCompleto { get; set; }

        public XmlDocument XmlEnvio
        {
            get
            {
                if (_xmlEnvio != null) return _xmlEnvio;

                _xmlEnvio = new XmlDocument();

                _xmlEnvio.LoadXml(Envio);

                return _xmlEnvio;
            }
        }

        public XmlDocument XmlRetorno
        {
            get
            {
                if (_xmlRetorno != null) return _xmlRetorno;

                _xmlRetorno = new XmlDocument();

                _xmlRetorno.LoadXml(Retorno);

                return _xmlRetorno;
            }
        }

        public XmlDocument XmlRetornoCompleto
        {
            get
            {
                if (_xmlRetornoCompleto != null) return _xmlRetornoCompleto;

                _xmlRetornoCompleto = new XmlDocument();

                _xmlRetornoCompleto.LoadXml(RetornoCompleto);

                return _xmlRetornoCompleto;
            }
        }

        public static string PegarValorDeXml(string campo, XmlDocument xml)
        {
            try
            {
                return ((XmlNode) xml.GetElementsByTagName(campo).FirstOrNull()).InnerText;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string PegarValorDeXml(string campo, XmlNode xml)
        {
            try
            {
                var xmlElement = xml[campo];
                return xmlElement?.InnerXml ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}