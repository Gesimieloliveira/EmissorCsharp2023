using System;
using System.Xml.Serialization;
using DFe.Utils;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;

namespace FusionCore.Extencoes
{
    public static class ExtZeusPis
    {
        public static string GetCodigo(this CSTPIS pis)
        {
            var xml = pis.ObterAtributo<XmlEnumAttribute>();

            if (xml == null)
            {
                throw new Exception($"Não foi possível obter o código do PIS em {nameof(ExtZeusPis)}");
            }

            return xml.Name;
        }
    }
}