using System;
using DFe.Utils;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public sealed class NfeletronicaGrid
    {
        public int Id { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public FinalidadeEmissao FinalidadeEmissao { get; set; }
        public StatusNfe StatusAtual { get; set; }
        public int NumeroDocumento { get; set; }
        public short Serie { get; set; }
        public string ProtocoloAutorizacao { get; set; }
        public string NomeDestinatario { get; set; }
        public string DocUnicoDestinatario { get; set; }
        public DateTime EmitidaEm { get; set; }
        public string RazaoSocialEmitente { get; set; }
        public decimal TotalFiscal { get; set; }
        public string Chave { get; set; }

        public string SituacaoInformativa => StatusAtual.Descricao();

        public bool IsFinalizada => StatusAtual != StatusNfe.Pendente;
        public bool IsAutorizada => StatusAtual == StatusNfe.Autorizada;
        public bool IsDenegada => StatusAtual == StatusNfe.Denegada;
        public bool IsCancelada => StatusAtual == StatusNfe.Cancelada;
        public bool IsPendente => StatusAtual == StatusNfe.Pendente;
    }
}