using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionNfce.Pagamento;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtFormaPagamentoNfceAdm
    {
        public static FormaPagamentoNfceAdm ToAdm(this FormaPagamentoNfce formaPagamento, NfceAdm nfceAdm)
        {
            if (formaPagamento == null) return null;

            var formaPagamentoAdm = new FormaPagamentoNfceAdm
            {
                Id = 0,
                Nfce = nfceAdm,
                IdFormaPagamento = formaPagamento.IdFormaPagamento,
                Nome = formaPagamento.Nome,
                ValorPagamento = formaPagamento.ValorPagamento,
                Adquirente = formaPagamento.Adquirente,
                NumeroAprovacao = formaPagamento.NumeroAprovacao,
                Bandeira = formaPagamento.Bandeira,
                IsExigeDadosManual = formaPagamento.IsExigeDadosManual,
                SemInternet = formaPagamento.SemInternet,
                IsVerificarStatusValidadorSucesso = formaPagamento.IsVerificarStatusValidadorSucesso,
                SerialPos = formaPagamento.SerialPos,
                Nsu = formaPagamento.Nsu,
                EstabelecimentoCodigo = formaPagamento.EstabelecimentoCodigo,
                IsRespostaFiscalSucesso = formaPagamento.IsRespostaFiscalSucesso,
                IsEnviarPagamentoSucesso = formaPagamento.IsEnviarPagamentoSucesso,
                IsMfe = formaPagamento.IsMfe,
                XmlEnvEnviarPagamento = formaPagamento.XmlEnvEnviarPagamento,
                XmlRetEnviarPagamento = formaPagamento.XmlRetEnviarPagamento,
                TipoAmbiente = formaPagamento.TipoAmbiente,
                XmlEnvRespostaFiscal = formaPagamento.XmlEnvRespostaFiscal,
                XmlEnvVerificarStatus = formaPagamento.XmlEnvVerificarStatus,
                XmlRetRespostaFiscal = formaPagamento.XmlRetRespostaFiscal,
                XmlRetVerificarStatus = formaPagamento.XmlRetVerificarStatus,
                CnpjCredenciadora = formaPagamento.CnpjCredenciadora,
                AjusteTipo = formaPagamento.AjusteTipo,
                IsAjuste = formaPagamento.IsAjuste,
                CodigoControle = formaPagamento.CodigoControle,
                DataEHoraTransacao = formaPagamento.DataEHoraTransacao,
                TipoTransacao = formaPagamento.TipoTransacao,
                IsTef = formaPagamento.IsTef,
                OperadoraTef = formaPagamento.OperadoraTef,
                Credenciadora = formaPagamento.Credenciadora,
                TipoCartaoPos = formaPagamento.TipoCartaoPos,
                DescricaoOutros = formaPagamento.DescricaoOutros
            };

            return formaPagamentoAdm;
        }
    }
}