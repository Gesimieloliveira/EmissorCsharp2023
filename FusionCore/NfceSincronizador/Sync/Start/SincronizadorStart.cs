using System;
using FusionCore.Extencoes;
using FusionCore.NfceSincronizador.Flags;

namespace FusionCore.NfceSincronizador.Sync.Start
{
    public class MensagemEventsArgs : EventArgs
    {
        public MensagemEventsArgs(string mensagem)
        {
            Mensagem = mensagem;
        }

        public string Mensagem { get; set; }
    }

    public sealed class SincronizadorStart
    {
        public event EventHandler<MensagemEventsArgs> MensagemHandler;

        public void Start()
        {
            OnMensagemHandler("Sincronizando estado(uf)");
            EntidadeSincronizavel.EstadoUf.Sincronizar();

            OnMensagemHandler("Sincronizando cidade");
            EntidadeSincronizavel.Cidade.Sincronizar();

            OnMensagemHandler("Sincronizando cliente");
            EntidadeSincronizavel.Pessoa.Sincronizar();

            OnMensagemHandler("Sincronizando usuário");
            EntidadeSincronizavel.Usuario.Sincronizar();

            OnMensagemHandler("Sincronizando empresa");
            EntidadeSincronizavel.Empresa.Sincronizar();

            OnMensagemHandler("Sincronizando cfop");
            EntidadeSincronizavel.Cfop.Sincronizar();

            OnMensagemHandler("Sincronizando emissor fiscal");
            EntidadeSincronizavel.EmissorFiscal.Sincronizar();

            OnMensagemHandler("Sincronizando regras de tributações para saidas");
            EntidadeSincronizavel.RegraTributacaoSaida.Sincronizar();

            OnMensagemHandler("Sincronizando produto unidade");
            EntidadeSincronizavel.ProdutoUnidade.Sincronizar();

            OnMensagemHandler("Sincronizando produto");
            EntidadeSincronizavel.Produto.Sincronizar();

            OnMensagemHandler("Sincronizando eventos de estoque");
            EntidadeSincronizavel.ProdutoEstoqueEvento.Sincronizar();

            OnMensagemHandler("Sincronizando configuração de envio de email");
            EntidadeSincronizavel.ConfiguracaoEmail.Sincronizar();

            OnMensagemHandler("Sincronizando tabela ibpt");
            EntidadeSincronizavel.Ibpt.Sincronizar();

            OnMensagemHandler("Sincronizando tipo documento");
            EntidadeSincronizavel.TipoDocumento.Sincronizar();

            OnMensagemHandler("Sincronizando configuração frente de caixa");
            EntidadeSincronizavel.ConfiguracaoFrenteCaixa.Sincronizar();

            OnMensagemHandler("Sincronizando configuração de estoque");
            EntidadeSincronizavel.ConfiguracaoEstoque.Sincronizar();

            OnMensagemHandler("Sincronizañdo balança");
            EntidadeSincronizavel.Balanca.Sincronizar();

            OnMensagemHandler("Sincronizando TEF Pos");
            EntidadeSincronizavel.Pos.Sincronizar();

            OnMensagemHandler("Sincronizando Responsável Técnico");
            EntidadeSincronizavel.ResponsavelTecnico.Sincronizar();

            OnMensagemHandler("Sincronizando Papeis de Usuários");
            EntidadeSincronizavel.Papel.Sincronizar();
        }

        private void OnMensagemHandler(string mensagem)
        {
            MensagemHandler?.Invoke(this, new MensagemEventsArgs(mensagem));
        }
    }
}