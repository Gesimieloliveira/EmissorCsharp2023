using System;
using NFe.Classes.Informacoes.Cobranca;

namespace FusionCore.Core.Nfes.Xml.Componentes.Impl
{
    public class XmlDuplicata
    {
        public XmlDuplicata(dup dup)
        {
            Numero = dup.nDup;
            DataVencimento = dup.dVenc;
            Valor = dup.vDup;
        }

        public decimal Valor { get; set; }

        public DateTime? DataVencimento { get; set; }

        public string Numero { get; set; }
    }
}