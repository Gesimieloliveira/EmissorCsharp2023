using System;
using System.Xml.Serialization;
using DFe.Utils;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;

namespace FusionCore.Extencoes
{
    public static class ExtZeusCofins
    {
        public static string GetCodigo(this CSTCOFINS cofins)
        {
            var xml = cofins.ObterAtributo<XmlEnumAttribute>();

            if (xml == null)
            {
                throw new Exception($"Não foi possível obter o código do COFINS em {nameof(ExtZeusCofins)}");
            }

            return xml.Name;
        }
    }
}