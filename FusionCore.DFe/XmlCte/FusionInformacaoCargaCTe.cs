using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionInformacaoCargaCTe
    {
        private string _outrasCaracteristicas;
        private decimal _valorTotalCarga;
        private decimal? _valorAverbacao;

        [XmlIgnore]
        public decimal ValorTotalCarga
        {
            get => _valorTotalCarga;
            set => _valorTotalCarga = value;
        }

        [XmlElement(ElementName = "vCarga")]
        public string ValorTotalCargaProxy
        {
            get => _valorTotalCarga.ToString("F2", new NumberFormatInfo { NumberDecimalSeparator = "." });

            set
            {
                const NumberStyles style = NumberStyles.AllowDecimalPoint;
                var format = new NumberFormatInfo { NumberDecimalSeparator = "." };

                if (decimal.TryParse(value, style, format, out var valorTotalCarga))
                {
                    ValorTotalCarga = valorTotalCarga;
                }
            }
        }

        [XmlElement(ElementName = "proPred")]
        public string NomeProdutoPredominante { get; set; }

        [XmlElement(ElementName = "xOutCat")]
        public string OutrasCaracteristicas
        {
            get => _outrasCaracteristicas;
            set => _outrasCaracteristicas = value;
        }

        [XmlElement(ElementName = "infQ")]
        public List<FusionInformacoesQuantidadeDaCargaCTe> InformacoesQuantidadeDaCarga { get; set; }

        [XmlIgnore]
        public decimal? ValorAverbacao
        {
            get => _valorAverbacao;
            set => _valorAverbacao = value;
        }

        [XmlElement(ElementName = "vCargaAverb")]
        public string ValorAverbacaoCargaProxy
        {
            get => _valorAverbacao?.ToString("F2", new NumberFormatInfo { NumberDecimalSeparator = "." });

            set
            {
                const NumberStyles style = NumberStyles.AllowDecimalPoint;
                var format = new NumberFormatInfo { NumberDecimalSeparator = "." };

                if (decimal.TryParse(value, style, format, out var valorAverbacao))
                {
                    ValorAverbacao = valorAverbacao;
                }
            }
        }


        public bool OutrasCaracteristicasSpecified => !string.IsNullOrEmpty(_outrasCaracteristicas);

        public FusionInformacaoCargaCTe()
        {
            InformacoesQuantidadeDaCarga = new List<FusionInformacoesQuantidadeDaCargaCTe>();
        }
    }
}