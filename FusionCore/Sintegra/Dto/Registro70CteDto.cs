using System;
using DFe.DocumentosEletronicos.Flags;
using FusionCore.Extencoes;

namespace FusionCore.Sintegra.Dto
{
    public class Registro70CteDto : IRegistro70Dto
    {
        public string Cnpj { get; set; }
        public string Cpf { get; set; }
        public string InscricaoEstadual { get; set; }
        public DateTime EmissaoEm { get; set; }
        public string SiglaUf { get; set; }
        public short Serie { get; set; }
        public int Numeracao { get; set; }
        public string Cfop { get; set; }
        public decimal ValorTotalDocumentoFiscal { get; set; }
        public decimal BaseCalculoIcms { get; set; }
        public decimal ValorIcms { get; set; }
        public short StatusCancelamento { get; set; }
        
        private bool EstaCancelada => StatusCancelamento == 135 || StatusCancelamento == 136 || StatusCancelamento == 134;
        public string Cst { get; set; }

        public string GetDocumentoUnico()
        {
            return Cnpj.IsNotNullOrEmpty() ? Cnpj : Cpf;
        }

        public string GetInscricaoEstadual()
        {
            return InscricaoEstadual;
        }

        public DateTime GetEmissaoRecebimento()
        {
            return EmissaoEm;
        }

        public string GetUf()
        {
            return SiglaUf;
        }

        public int GetModelo()
        {
            return (int) ModeloDocumento.NFe;
        }

        public string GetSerie()
        {
            return Serie.ToString("D3");
        }

        public string GetSubSerie()
        {
            return "0";
        }

        public int GetNumero()
        {
            return TrataNumeroSintegra.Trata(Numeracao);
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
            if (EstaCancelada)
                return "S";

            return "N";
        }

        public int GetCifFob()
        {
            return 2;
        }
    }
}