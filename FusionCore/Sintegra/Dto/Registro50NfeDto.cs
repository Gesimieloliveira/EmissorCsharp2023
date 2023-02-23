using System;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Cancelar;

namespace FusionCore.Sintegra.Dto
{
    public class Registro50NfeDto : IRegistro50Dto
    {
        public Registro50NfeDto() { }

        public Registro50NfeDto(Registro50NfeDto registro50NfeDto)
        {
            DocumentoUnico = registro50NfeDto.DocumentoUnico;
            InscricaoEstadual = registro50NfeDto.InscricaoEstadual;
            EmitidaEm = registro50NfeDto.EmitidaEm;
            SiglaUf = registro50NfeDto.SiglaUf;
            Serie = registro50NfeDto.Serie;
            Numero = registro50NfeDto.Numero;
            Cfop = registro50NfeDto.Cfop;
            ValorTotal = registro50NfeDto.ValorTotal;
            BaseCalculo = registro50NfeDto.BaseCalculo;
            ValorIcms = registro50NfeDto.ValorIcms;
            AliquotaIcms = registro50NfeDto.AliquotaIcms;
            Cancelamento = registro50NfeDto.Cancelamento;
            Csosn = registro50NfeDto.Csosn;
        }

        public string DocumentoUnico { get; set; }
        public string InscricaoEstadual { get; set; }
        public DateTime EmitidaEm { get; set; }
        public string SiglaUf { get; set; }
        public int Serie { get; set; }
        public int Numero { get; set; }
        public string Cfop { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal ValorIcms { get; set; }
        public decimal AliquotaIcms { get; set; }
        public StatusCancelamento Cancelamento { get; set; }
        public string Csosn { get; set; }

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

            return int.Parse(Cfop);
        }

        public string GetEmitente()
        {
            return "P";
        }

        public decimal GetValorTotal()
        {
            if (IsDenegado) return 0;

            return ValorTotal;
        }

        public decimal GetBaseCalculoIcms()
        {
            if (IsDenegado) return 0;

            return BaseCalculo;
        }

        public decimal GetValorIcms()
        {
            if (IsDenegado) return 0;

            return ValorIcms;
        }

        public decimal? GetValorOutras()
        {
            if (IsDenegado) return null;

            if (Csosn == "900") return null;

            return GetValorTotal();
        }

        public decimal GetAliquotaIcms()
        {
            if (IsDenegado) return 0;

            return AliquotaIcms;
        }

        public string GetSituacaoNotaFiscal()
        {
            if (Cancelamento != null && Cancelamento.EstaCancelado) return "S";

            if (IsDenegado) return "2";

            return "N";
        }

        public decimal GetValorSt()
        {
            return 0;
        }

        public bool IsTemValorOutros => Csosn != "900";
        public bool IsDenegado { get; set; }
    }
}