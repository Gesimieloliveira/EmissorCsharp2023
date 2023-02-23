using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Tef;
using FusionCore.FusionNfce.Pagamento;
using FusionCore.FusionNfce.Tef;

namespace FusionCore.FusionAdm.Nfce
{
    public class FormaPagamentoNfceAdm
    {
        public int Id { get; set; }
        public NfceAdm Nfce { get; set; }
        public string Nome { get; set; }
        public decimal ValorPagamento { get; set; }
        public string IdFormaPagamento { get; set; }

        public bool IsMfe { get; set; }
        public string XmlEnvEnviarPagamento { get; set; }
        public string XmlRetEnviarPagamento { get; set; }
        public string XmlEnvVerificarStatus { get; set; }
        public string XmlRetVerificarStatus { get; set; }
        public string XmlEnvRespostaFiscal { get; set; }
        public string XmlRetRespostaFiscal { get; set; }
        public bool SemInternet { get; set; }
        public string SerialPos { get; set; } = string.Empty;
        public string EstabelecimentoCodigo { get; set; } = string.Empty;
        public string Adquirente { get; set; } = string.Empty;

        public TipoAmbiente TipoAmbiente { get; set; } = TipoAmbiente.Producao;
        public string Bandeira { get; set; } = string.Empty;
        public string Nsu { get; set; } = string.Empty;
        public string NumeroAprovacao { get; set; } = string.Empty;
        public bool IsExigeDadosManual { get; set; }
        public bool IsEnviarPagamentoSucesso { get; set; }
        public bool IsVerificarStatusValidadorSucesso { get; set; }
        public bool IsRespostaFiscalSucesso { get; set; }
        public AjusteTipo AjusteTipo { get; set; } = AjusteTipo.Nenhum;
        public bool IsAjuste { get; set; }
        public string CnpjCredenciadora { get; set; } = string.Empty;
        public bool IsTef { get; set; }
        public string TipoTransacao { get; set; } = string.Empty;
        public string CodigoControle { get; set; } = string.Empty;
        public Operadora OperadoraTef { get; set; } = Operadora.TefDialHomologacao;
        public DateTime? DataEHoraTransacao { get; set; }
        public Credenciadora? Credenciadora { get; set; }
        public TipoCartaoPos? TipoCartaoPos { get; set; }
        public string DescricaoOutros { get; set; } = string.Empty;
    }
}