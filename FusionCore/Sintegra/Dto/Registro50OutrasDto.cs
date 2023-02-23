using System;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.EntradaOutras;

namespace FusionCore.Sintegra.Dto
{
    public class Registro50OutrasDto : IRegistro50Dto
    {
        public string Cnpj { get; set; }
        public string Cpf { get; set; }

        public string GetDocumentoUnico()
        {
            if (Cpf.IsNotNullOrEmpty()) return Cpf;

            return Cnpj;
        }

        public string GetInscricaoEstadual()
        {
            return InscricaoEstadual.IsNullOrEmpty() ? "ISENTO" : InscricaoEstadual;
        }

        public DateTime GetEmissaoRecebimento()
        {
            if (EmissaoEm != null) return EmissaoEm.Value;

            return RecebimentoEm.Value;
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

        public int GetNumero()
        {
            return TrataNumeroSintegra.Trata(Numero);
        }

        public int GetCfop()
        {
            return int.Parse(Cfop);
        }

        public string GetEmitente()
        {
            if (TipoEmitente == TipoEmitente.Proprio)
                return "P";

            return "T";
        }

        public decimal GetValorTotal()
        {
            return ValorTotal;
        }

        public decimal GetBaseCalculoIcms()
        {
            return BaseCalculo;
        }

        public decimal GetValorIcms()
        {
            return ValorIcms;
        }

        public decimal? GetValorOutras()
        {
            return GetValorTotal();
        }

        public decimal GetAliquotaIcms()
        {
            return AliquotaIcms;
        }

        public string InscricaoEstadual { get; set; }
        public DateTime? EmissaoEm { get; set; }
        public DateTime? RecebimentoEm { get; set; }
        public string SiglaUf { get; set; }
        public ModeloDocumentoOutro ModeloDocumento { get; set; }
        public int Serie { get; set; }
        public int Numero { get; set; }
        public string Cfop { get; set; }
        public TipoEmitente TipoEmitente { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal ValorIcms { get; set; }
        public decimal AliquotaIcms { get; set; }
        public SituacaoFiscal Situacao { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorDespesasAcessorias { get; set; }
        public decimal ValorSeguro { get; set; }
        public string CodigoCst { get; set; }

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

        public decimal GetValorSt()
        {
            return 0;
        }
    }
}