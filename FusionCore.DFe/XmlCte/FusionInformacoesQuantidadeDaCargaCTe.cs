using System;
using System.Globalization;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionInformacoesQuantidadeDaCargaCTe
    {
        [XmlElement(ElementName = "cUnid")]
        public FusionUnidadeMedidaCTe UnidadeMedida { get; set; }

        [XmlElement(ElementName = "tpMed")]
        public string DescricaoMedida { get; set; }

        [XmlIgnore]
        public decimal Quantidade { get; set; }

        [XmlElement(ElementName = "qCarga")]
        public string QuantidadeProxy
        {
            get { return Quantidade.ToString("F4", new NumberFormatInfo {NumberDecimalSeparator = "."}); }
            set
            {
                decimal quantidade;

                const NumberStyles style = NumberStyles.AllowDecimalPoint;
                var format = new NumberFormatInfo { NumberDecimalSeparator = "." };

                if (decimal.TryParse(value, style, format, out quantidade))
                {
                    Quantidade = quantidade;
                }
            }
        }
    }
}