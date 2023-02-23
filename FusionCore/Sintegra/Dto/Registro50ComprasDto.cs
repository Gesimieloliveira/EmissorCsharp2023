using System;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.Sintegra.Dto
{
    public class Registro50ComprasDto : IRegistro50Dto
    {
        public Registro50ComprasDto() { }

        public Registro50ComprasDto(Registro50ComprasDto reg50)
        {
            Cnpj = reg50.Cnpj;
            Cpf = reg50.Cpf;
            InscricaoEstadual = reg50.InscricaoEstadual;
            LancamentoEm = reg50.LancamentoEm;
            SiglaUf = reg50.SiglaUf;
            ChaveNfe = reg50.ChaveNfe;
            Serie = reg50.Serie;
            Numero = reg50.Numero;
            Cfop = reg50.Cfop;
            ValorTotal = reg50.ValorTotal;
            BaseCalculoIcms = reg50.BaseCalculoIcms;
            ValorIcms = reg50.ValorIcms;
            Aliquota = reg50.Aliquota;
            Cst = reg50.Cst;
        }

        public string Cnpj { get; set; }
        public string Cpf { get; set; }
        public string InscricaoEstadual { get; set; }
        public DateTime LancamentoEm { get; set; }
        public string SiglaUf { get; set; }
        public string ChaveNfe { get; set; }
        public short Serie { get; set; }
        public int Numero { get; set; }
        public string Cfop { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal BaseCalculoIcms { get; set; }
        public decimal ValorIcms { get; set; }
        public decimal Aliquota { get; set; }
        public string Cst { get; set; }

        public bool IsTemValorOutros => Cst != "900" 
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

        public decimal ValorSt { get; set; }
        public decimal ValorIpi { get; set; }
        public decimal DespesaRateio { get; set; }
        public decimal ValorFcpSt { get; set; }

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
            return LancamentoEm;
        }

        public string GetUf()
        {
            return SiglaUf;
        }

        public int GetModelo()
        {
            if (ChaveNfe.IsNotNullOrEmpty()) return (int) ModeloDocumento.NFe;

            const int modelo01 = 01;

            return modelo01;
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

        public decimal GetValorTotal()
        {
            return ValorTotal;
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
            return GetValorTotal();
        }

        public decimal GetAliquotaIcms()
        {
            return Aliquota;
        }

        public string GetSituacaoNotaFiscal()
        {
            return "N";
        }

        public decimal GetValorSt()
        {
            return ValorSt;
        }
    }
}