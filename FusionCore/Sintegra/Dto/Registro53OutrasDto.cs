using System;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.EntradaOutras;

namespace FusionCore.Sintegra.Dto
{
    public class Registro53OutrasDto : IRegistro53Dto
    {
        public string Cnpj { get; set; }
        public string Cpf { get; set; }
        public string InscricaoEstadual { get; set; }
        public DateTime EmissaoEm { get; set; }
        public DateTime RecebimentoEm { get; set; }
        public string SiglaUf { get; set; }
        public ModeloDocumentoOutro ModeloDocumento { get; set; }
        public int Serie { get; set; }
        public int Numero { get; set; }
        public string Cfop { get; set; }
        public TipoEmitente TipoEmitente { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal BaseCalculoSt { get; set; }
        public decimal ValorIcmsSt { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorDespesasAcessorias { get; set; }
        public decimal ValorSeguro { get; set; }
        public SituacaoFiscal Situacao { get; set; }
        public string CodigoCst { get; set; }


        public string GetDocumentoUnico()
        {
            return Cnpj.IsNotNullOrEmpty() ? Cnpj : Cpf;
        }

        public string GetInscricaoEstadual()
        {
            return InscricaoEstadual.IsNullOrEmpty() ? "ISENTO" : InscricaoEstadual;
        }

        public DateTime GetEmissaoRecebimento()
        {
            return RecebimentoEm;
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
            return Convert.ToInt32(Cfop);
        }

        public string GetEmitente()
        {
            return "T";
        }

        public decimal GetBaseCalculoIcmsSt()
        {
            return BaseCalculoSt;
        }

        public decimal GetIcmsRetido()
        {
            return 0;
        }

        public decimal GetDespesasAcessorias()
        {
            return ValorFrete + ValorDespesasAcessorias + ValorSeguro;
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

        public string GetCodigoAntecipacao()
        {
            return "3";
        }
    }
}