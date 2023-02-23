using System;
using FusionCore.FusionAdm.EntradaOutras;

namespace FusionCore.Sintegra.Dto
{
    public class Registro70CteEntradaDto : IRegistro70Dto
    {
        public string DocumentoUnico { get; set; }
        public string InscricaoEstadual { get; set; }
        public ModeloDocumentoCteEntrada ModeloDocumento { get; set; }
        public DateTime DataEmissao { get; set; }
        public string SiglaUf { get; set; }
        public int Serie { get; set; }
        public int Subserie { get; set; }
        public int Numero { get; set; }
        public string Cfop { get; set; }
        public decimal ValorTotalDocumentoFiscal { get; set; }
        public decimal BaseCalculoIcms { get; set; }
        public decimal ValorIcms { get; set; }
        public SituacaoFiscal Situacao { get; set; }
        public string Cst { get; set; }


        public string GetDocumentoUnico()
        {
            return DocumentoUnico;
        }

        public string GetInscricaoEstadual()
        {
            return InscricaoEstadual;
        }

        public DateTime GetEmissaoRecebimento()
        {
            return DataEmissao;
        }

        public string GetUf()
        {
            return SiglaUf;
        }

        public int GetModelo()
        {
            return (int) ModeloDocumento;
        }

        public string GetSerie()
        {
            return Serie.ToString("D3");
        }

        public string GetSubSerie()
        {
            return Subserie.ToString("D3");
        }

        public int GetNumero()
        {
            return TrataNumeroSintegra.Trata(Numero);
        }

        public int GetCfop()
        {
            return int.Parse(Cfop);
        }

        public decimal GetValorTotal()
        {
            return ValorTotalDocumentoFiscal;
        }

        public decimal GetBaseCalculoIcms()
        {
            return BaseCalculoIcms;
        }

        public decimal GetValorIcms()
        {
            return ValorIcms;
        }

        public decimal? GetValorOutras()
        {
            if (Cst == "900"
                || Cst == "90"
                || Cst == "00"
                || Cst == "10"
                || Cst == "20"
                || Cst == "70") return null;

            return GetValorTotal();
        }

        public string GetSituacaoNotaFiscal()
        {
            switch (Situacao)
            {
                case SituacaoFiscal.Normal:
                    return "N";
                case SituacaoFiscal.Cancelado:
                    return "S";
                case SituacaoFiscal.ExtemporaneoNormal:
                    return "E";
                case SituacaoFiscal.ExtemporaneoCancelado:
                    return "X";
                case SituacaoFiscal.Denegada:
                    return "2";
                case SituacaoFiscal.Inutilizado:
                    return "4";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int GetCifFob()
        {
            return 2;
        }
    }
}