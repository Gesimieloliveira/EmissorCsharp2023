using System;
using System.Xml.Serialization;
using DFe.Utils;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.Extencoes
{
    public static class ExtZeusIcms
    {
        public static string GetCodigo(this Csticms icms)
        {
            var xml = icms.ObterAtributo<XmlEnumAttribute>();

            if (xml == null)
            {
                throw new Exception($"Não foi possível obter o código do ICMS em {nameof(ExtZeusIcms)}");
            }

            return xml.Name;
        }

        public static string GetCodigo(this Csosnicms csosn)
        {
            var xml = csosn.ObterAtributo<XmlEnumAttribute>();

            if (xml == null)
            {
                throw new Exception($"Não foi possível obter o código do ICMS em {nameof(ExtZeusIcms)}");
            }

            return xml.Name;
        }
    }
}