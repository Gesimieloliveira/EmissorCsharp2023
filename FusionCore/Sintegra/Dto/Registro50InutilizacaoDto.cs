using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.Sintegra.Dto
{
    public class Registro50InutilizacaoDto : IRegistro50Dto
    {
        public DateTime EmissaoEm { get; set; }
        public ModeloDocumento ModeloDocumento { get; set; }
        public short Serie { get; set; }
        public int Numero { get; set; }

        public string GetDocumentoUnico()
        {
            return "00000000000000";
        }

        public string GetInscricaoEstadual()
        {
            return string.Empty;
        }

        public DateTime GetEmissaoRecebimento()
        {
            return EmissaoEm;
        }

        public string GetUf()
        {
            return string.Empty;
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
            return 0;
        }

        public string GetEmitente()
        {
            return "P";
        }

        public decimal GetValorTotal()
        {
            return 0;
        }

        public decimal GetBaseCalculoIcms()
        {
            return 0;
        }

        public decimal GetValorIcms()
        {
            return 0;
        }

        public decimal? GetValorOutras()
        {
            return null;
        }

        public decimal GetAliquotaIcms()
        {
            return 0;
        }

        public string GetSituacaoNotaFiscal()
        {
            return "4";
        }

        public decimal GetValorSt()
        {
            return 0;
        }
    }
}