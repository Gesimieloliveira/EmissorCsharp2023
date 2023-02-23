﻿using MotorTributarioNet.Flags;
using MotorTributarioNet.Impostos;

namespace FusionCore.FusionAdm.CteEletronico.Fiscal
{
    public class FreteTributavel : ITributavel
    {
        public FreteTributavel()
        {
            Documento = Documento.CTe;
        }

        public Documento Documento { get; set; }
        public Cst Cst { get; set; }
        public decimal ValorProduto { get; set; }
        public decimal Frete { get; set; }
        public decimal Seguro { get; set; }
        public decimal OutrasDespesas { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorIpi { get; set; }
        public decimal PercentualReducao { get; set; }
        public decimal QuantidadeProduto { get; set; }
        public decimal PercentualIcms { get; set; }
        public decimal PercentualCredito { get; set; }
        public decimal PercentualDiferimento { get; set; }
        public decimal PercentualDifalInterna { get; set; }
        public decimal PercentualDifalInterestadual { get; set; }
        public decimal PercentualFcp { get; set; }
        public decimal PercentualMva { get; set; }
        public decimal PercentualIcmsSt { get; set; }
        public decimal PercentualIpi { get; set; }
        public decimal PercentualCofins { get; set; }
        public decimal PercentualPis { get; set; }
        public decimal PercentualReducaoSt { get; set; }
    }
}