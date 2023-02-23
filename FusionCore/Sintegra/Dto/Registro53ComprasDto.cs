using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.Sintegra.Dto
{
    public class Registro53ComprasDto : IRegistro53Dto
    {
        public Registro53ComprasDto() { }

        public Registro53ComprasDto(Registro53ComprasDto reg50)
        {
            SiglaUf = reg50.SiglaUf;
            Cpf = reg50.Cpf;
            Cnpj = reg50.Cnpj;
            InscricaoEstadual = reg50.InscricaoEstadual;
            LancamentoEm = reg50.LancamentoEm;
            Serie = reg50.Serie;
            Numero = reg50.Numero;
            Cfop = reg50.Cfop;
            ValorTotal = reg50.ValorTotal;
            BaseCalculoIcmsSt = reg50.BaseCalculoIcmsSt;
            ChaveNfe = reg50.ChaveNfe;
            Cst = reg50.Cst;
        }

        public string SiglaUf { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoEstadual { get; set; }
        public DateTime LancamentoEm { get; set; }
        public short Serie { get; set; }
        public int Numero { get; set; }
        public string Cfop { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal BaseCalculoIcmsSt { get; set; }
        public string Cst { get; set; }
        public string ChaveNfe { get; set; }

        public bool IsNaoTemIcms => Cst != "900"
                                    && Cst != "90"
                                    && Cst != "00"
                                    && Cst != "10"
                                    && Cst != "20"
                                    && Cst != "70";

        public string DocumentoUnico
        {
            get { return GetDocumentoUnico(); }
            set
            {
                if (value.IsNullOrEmpty()) return;

                if (value.Length == 14)
                {
                    Cnpj = value;
                    return;
                }

                Cpf = value;
            }
        }

        public string GetDocumentoUnico()
        {
            if (Cnpj.IsNotNullOrEmpty()) return Cnpj;

            return Cpf;
        }

        public string GetInscricaoEstadual()
        {
            return InscricaoEstadual.IsNullOrEmpty() ? "ISENTO" : InscricaoEstadual;
        }

        public DateTime GetEmissaoRecebimento()
        {
            return LancamentoEm;
        }

        public string GetUf()
        {
            return SiglaUf;
        }

        public int GetModelo()
        {
            if (ChaveNfe.IsNotNullOrEmpty())
                return (int) ModeloDocumento.NFe;

            return 01;
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
            return "T";
        }

        public decimal GetBaseCalculoIcmsSt()
        {
            return BaseCalculoIcmsSt;
        }

        public decimal GetIcmsRetido()
        {
            return 0;
        }

        public decimal GetDespesasAcessorias()
        {
            return 0;
        }

        public string GetSituacaoNotaFiscal()
        {
            return "N";
        }

        public string GetCodigoAntecipacao()
        {
            return "3";
        }
    }
}