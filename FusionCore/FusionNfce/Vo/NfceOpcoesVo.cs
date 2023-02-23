using System;
using DFe.Utils;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.Flags;

namespace FusionCore.FusionNfce.Vo
{
    public class NfceOpcoesVo
    {
        public int Id { get; set; }
        public DateTime EmitidaEm { get; set; }
        public DateTime? AutorizadaEm { get; set; }
        public int NumeroDocumento { get; set; }
        public short Serie { get; set; }
        public TipoEmissao? TipoEmissao { get; set; }
        public string Chave { get; set; }
        public decimal Total { get; set; }
        public bool Autorizado { get; set; }
        public Status Status { get; set; }
        public bool IsContingencia => GetContingencia();
        public bool IsCancelada => GetCancelada();

        public string TipoEmissaoString => TipoEmissao == null ? "Ainda não possui emissão" : TipoEmissao.Descricao();

        private bool GetCancelada()
        {
            return Status == Status.Cancelada;
        }

        private bool GetContingencia()
        {
            return TipoEmissao == FusionAdm.Fiscal.Flags.TipoEmissao.ContigenciaOfflineNFCe;
        }
    }
}