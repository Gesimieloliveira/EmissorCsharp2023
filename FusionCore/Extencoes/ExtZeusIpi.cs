using System;
using System.Xml.Serialization;
using DFe.Utils;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;

namespace FusionCore.Extencoes
{
    public static class ExtZeusIpi
    {
        public static string GetCodigo(this CSTIPI ipi)
        {
            var xml = ipi.ObterAtributo<XmlEnumAttribute>();

            if (xml == null)
            {
                throw new Exception($"Não foi possível obter o código do PIS em {nameof(ExtZeusIpi)}");
            }

            return xml.Name;
        }
    }
}