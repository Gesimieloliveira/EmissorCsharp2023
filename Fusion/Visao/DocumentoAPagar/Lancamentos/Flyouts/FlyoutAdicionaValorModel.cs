using System;
using Fusion.Sessao;
using FusionCore.ControleCaixa.Facades;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Servicos;
using FusionLibrary.ValidacaoAnotacao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.DocumentoAPagar.Lancamentos.Flyouts
{
    public class FlyoutAdicionaValorModel : ViewModel
    {
        private readonly DocumentoPagar _documento;
        private readonly SessaoSistema _sessaoSistema;

        public FlyoutAdicionaValorModel(DocumentoPagar documento, SessaoSistema sessaoSistema)
        {
            _documento = documento;
            _sessaoSistema = sessaoSistema;

            InicializarPropriedades();
        }

        public int Id
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public string Historico
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool IsOpen
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public decimal ValorRestante
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        [DecimalRequired(ErrorMessage = "Preciso de um valor acima de 0,00")]
        public decimal? ValorPagamento
        {
            get => GetValue<decimal?>();
            set => SetValue(value);
        }

        public bool MarcarComoQuitado
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        private void InicializarPropriedades()
        {
            Id = _documento.Id;
            ValorRestante = _documento.ValorEmAberto;
            Historico = string.Empty;
            MarcarComoQuitado = false;
        }

        public event EventHandler ValorAdicionadoSucesso;

        protected virtual void OnValorAdicionadoSucesso()
        {
            ValorAdicionadoSucesso?.Invoke(this, EventArgs.Empty);
        }

        public void AdicionarPagamento()
        {
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_sessaoSistema.UsuarioLogado);
            ThrowExceptionSeExistirErros();

            var servico = new ServicoPagarDocumento(_sessaoSistema.SessaoManager)
            {
                DocumentoId = _documento.Id,
                ValorPagamento = ValorPagamento ?? 0M, 
                Historico = Historico, 
                MarcarComoQuitado = MarcarComoQuitado,
                Usuario = _sessaoSistema.UsuarioLogado
            };

            servico.FazerPagamento();
            OnValorAdicionadoSucesso();
        }

        public bool ValorPagamentoMenorQueRestante()
        {
            return ValorPagamento < ValorRestante;
        }

        public bool ValorPagamentoMaiorQueRestante()
        {
            return ValorPagamento > ValorRestante;
        }
    }
}