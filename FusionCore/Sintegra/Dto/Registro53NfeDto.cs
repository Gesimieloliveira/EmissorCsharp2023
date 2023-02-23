using System;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Cancelar;

namespace FusionCore.Sintegra.Dto
{
    public class Registro53NfeDto : IRegistro53Dto
    {
        public Registro53NfeDto() { }

        public Registro53NfeDto(Registro53NfeDto reg50)
        {
            DocumentoUnico = reg50.DocumentoUnico;
            InscricaoEstadual = reg50.InscricaoEstadual;
            EmitidaEm = reg50.EmitidaEm;
            SiglaUf = reg50.SiglaUf;
            Serie = reg50.Serie;
            Numero = reg50.Numero;
            Cfop = reg50.Cfop;
            Csosn = reg50.Csosn;
            ValorTotal = reg50.ValorTotal;
            BaseCalculoSt = reg50.BaseCalculoSt;
            Cancelamento = reg50.Cancelamento;
            IsDenegado = reg50.IsDenegado;
        }

        public string DocumentoUnico { get; set; }
        public string InscricaoEstadual { get; set; }
        public DateTime EmitidaEm { get; set; }
        public string SiglaUf { get; set; }
        public short Serie { get; set; }
        public int Numero { get; set; }
        public string Cfop { get; set; }
        public string Csosn { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal BaseCalculoSt { get; set; }
        public StatusCancelamento Cancelamento { get; set; }
        public bool IsDenegado { get; set; }

        public bool IsNaoTemIcms => Csosn != "900";

        public string GetDocumentoUnico()
        {
            if (IsDenegado) return string.Empty;

            return DocumentoUnico;
        }

        public string GetInscricaoEstadual()
        {
            if (IsDenegado) return string.Empty;

            return InscricaoEstadual.IsNullOrEmpty() ? "ISENTO" : InscricaoEstadual;
        }

        public DateTime GetEmissaoRecebimento()
        {
            return EmitidaEm;
        }

        public string GetUf()
        {
            if (IsDenegado) return string.Empty;

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

        public int GetNumero()
        {
            return TrataNumeroSintegra.Trata(Numero);
        }

        public int GetCfop()
        {
            if (IsDenegado) return 0;

            return Convert.ToInt32(Cfop);
        }

        public string GetEmitente()
        {
            return "P";
        }

        public decimal GetBaseCalculoIcmsSt()
        {
            if (IsDenegado) return 0;

            return BaseCalculoSt;
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
            if (Cancelamento != null && Cancelamento.EstaCancelado) return "S";

            if (IsDenegado) return "2";

            return "N";
        }

        public string GetCodigoAntecipacao()
        {
            return "3";
        }
    }
}