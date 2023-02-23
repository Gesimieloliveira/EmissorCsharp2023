using System;
using System.Reflection;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteSubstituicao
    {
        public int CteId { get; set; }
        public Cte Cte { get; set; }
        public string ChaveSubstituido { get; set; }
        public string ChaveAnulacao { get; set; }

        public string ChaveNfePeloTomador { get; set; }
        public string ChaveCtePeloTomador { get; set; }

        public string DocumentoUnico { get; set; }
        public CteModeloDocumento ModeloDocumento { get; set; }
        public string Serie { get; set; }
        public string Subserie { get; set; }
        public string NumeroDocumentoFiscal { get; set; }
        public decimal Valor { get; set; }
        public DateTime EmitidoEm { get; set; }


        public bool IsTemTomador()
        {
            return ChaveNfePeloTomador.IsNotNullOrEmpty()
                   || ChaveCtePeloTomador.IsNotNullOrEmpty();
        }

        public bool IsTemReferenciaNF()
        {
            return DocumentoUnico.IsNotNullOrEmpty()
                   || Serie.IsNotNullOrEmpty()
                   || Subserie.IsNotNullOrEmpty()
                   || NumeroDocumentoFiscal.IsNotNullOrEmpty();
        }

        public string GetModelo()
        {
            FieldInfo fi = ModeloDocumento.GetType().GetField(ModeloDocumento.ToString());

            ModeloAttribute[] attributes = (ModeloAttribute[])fi.GetCustomAttributes(
                typeof(ModeloAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Modelo;
            else return ModeloDocumento.ToString();
        }
    }
}