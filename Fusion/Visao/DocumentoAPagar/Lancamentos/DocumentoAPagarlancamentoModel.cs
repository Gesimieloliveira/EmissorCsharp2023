using System.Collections.ObjectModel;
using Fusion.Base.Notificacoes;
using Fusion.Sessao;
using Fusion.Visao.DocumentoAPagar.Lancamentos.Flyouts;
using FusionCore.ControleCaixa.Facades;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Servicos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.DocumentoAPagar.Lancamentos
{
    public class DocumentoAPagarLancamentoModel : ViewModel
    {
        private readonly Notificador _notificador;
        private FlyoutAdicionaValorModel _flyoutAdicionaValorModel;
        private decimal _valorAjustado;
        private decimal _valorQuitado;
        private decimal _juros;
        private decimal _desconto;
        private DocumentoPagar _documentoPagar;
        private FlyoutAdicionaDescontoModel _flyoutAdicionaDescontoModel;
        private FlyoutAdicionaJuroModel _flyoutAdicionaJuroModel;
        private decimal _valorAberto;
        private decimal _valorOriginal;
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;

        public DocumentoPagar DocumentoPagar
        {
            get => _documentoPagar;
            set
            {
                if (Equals(value, _documentoPagar)) return;
                _documentoPagar = value;
                PropriedadeAlterada();
            }
        }

        public DocumentoPagarLancamento ItemSelecionado
        {
            get => GetValue<DocumentoPagarLancamento>();
            set => SetValue(value);
        }

        public ObservableCollection<DocumentoPagarLancamento> Lancamentos
        {
            get => GetValue<ObservableCollection<DocumentoPagarLancamento>>();
            set => SetValue(value);
        }

        public decimal ValorAjustado
        {
            get => _valorAjustado.Arredonda(2);
            set
            {
                if (value == _valorAjustado) return;
                _valorAjustado = value.Arredonda(2);
                PropriedadeAlterada();
            }
        }

        public decimal ValorQuitado
        {
            get => _valorQuitado.Arredonda(2);
            set
            {
                if (value == _valorQuitado) return;
                _valorQuitado = value.Arredonda(2);
                PropriedadeAlterada();
            }
        }

        public decimal Juros
        {
            get => _juros.Arredonda(2);
            set
            {
                if (value == _juros) return;
                _juros = value.Arredonda(2);
                PropriedadeAlterada();
            }
        }

        public decimal Desconto
        {
            get => _desconto.Arredonda(2);
            set
            {
                if (value == _desconto) return;
                _desconto = value.Arredonda(2);
                PropriedadeAlterada();
            }
        }

        public decimal ValorAberto
        {
            get => _valorAberto.Arredonda(2);
            set
            {
                if (value == _valorAberto) return;
                _valorAberto = value.Arredonda(2);
                PropriedadeAlterada();
            }
        }

        public FlyoutAdicionaValorModel FlyoutAdicionaValorModel
        {
            get => _flyoutAdicionaValorModel;
            set
            {
                if (Equals(value, _flyoutAdicionaValorModel)) return;
                _flyoutAdicionaValorModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAdicionaDescontoModel FlyoutAdicionaDescontoModel
        {
            get => _flyoutAdicionaDescontoModel;
            set
            {
                if (Equals(value, _flyoutAdicionaDescontoModel)) return;
                _flyoutAdicionaDescontoModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAdicionaJuroModel FlyoutAdicionaJuroModel
        {
            get => _flyoutAdicionaJuroModel;
            set
            {
                if (Equals(value, _flyoutAdicionaJuroModel)) return;
                _flyoutAdicionaJuroModel = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorOriginal
        {
            get => _valorOriginal.Arredonda(2);
            set
            {
                if (value == _valorOriginal) return;
                _valorOriginal = value.Arredonda(2);
                PropriedadeAlterada();
            }
        }

        public DocumentoAPagarLancamentoModel(DocumentoPagar documento, Notificador notificador)
        {
            _notificador = notificador;

            DocumentoPagar = documento;
            Lancamentos = new ObservableCollection<DocumentoPagarLancamento>();

            AtualizarViewModel();
        }

        private void AtualizarViewModel()
        {
            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao())
            {
                sessao.Refresh(DocumentoPagar);
            }

            ValorOriginal = DocumentoPagar.ValorOriginal;
            ValorAjustado = DocumentoPagar.ValorAjustado;
            ValorQuitado = DocumentoPagar.ValorQuitado;
            Juros = DocumentoPagar.Juros;
            Desconto = DocumentoPagar.Desconto;
            ValorAberto = DocumentoPagar.ValorEmAberto;

            Lancamentos.Clear();
            DocumentoPagar.Lancamentos.ForEach(Lancamentos.Add);
        }

        public void AbrirFlyoutAdicionaValor()
        {
            _sessaoSistema
                .UsuarioLogado
                .VerificaPermissao
                .IsTemPermissaoThrow(Permissao.FINANCEIRO_DOCUMENTO_APAGAR_QUITAR);

            FlyoutAdicionaValorModel = new FlyoutAdicionaValorModel(DocumentoPagar, _sessaoSistema) {IsOpen = true};

            FlyoutAdicionaValorModel.ValorAdicionadoSucesso += (s, a)  =>
            {
                AtualizarViewModel();
                FlyoutAdicionaValorModel.IsOpen = false;

                _notificador.Notificar("documentoPagarSalvo", new NotificacaoArgs(DocumentoPagar));
            };
        }

        public void AbrirFlyoutAdicionaJuro()
        {
            _sessaoSistema
                .UsuarioLogado
                .VerificaPermissao
                .IsTemPermissaoThrow(Permissao.FINANCEIRO_DOCUMENTO_APAGAR_ADICIONAR_JUROS);

            FlyoutAdicionaJuroModel = new FlyoutAdicionaJuroModel(DocumentoPagar);
            FlyoutAdicionaJuroModel.Retorno += RetornoAdicionaJuroModel;
            FlyoutAdicionaJuroModel.IsOpen = true;
        }

        private void RetornoAdicionaJuroModel(object sender, FlyoutAdicionaJuroArgs args)
        {
            if (args.Juros == 0) return;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioDocumentoPagar(sessao);

                DocumentoPagar.AdicionarJuros(args.Juros, args.Historico);

                repositorio.Salvar(DocumentoPagar);
                transacao.Commit();
            }

            AtualizarViewModel();
            FlyoutAdicionaJuroModel.IsOpen = false;

            _notificador.Notificar("documentoPagarSalvo", new NotificacaoArgs(DocumentoPagar));
        }

        public void EstornarItem()
        {
            if (ItemSelecionado == null)
            {
                return;
            }

            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_sessaoSistema.UsuarioLogado);

            var servicoEstorno = new ServicoEstornarPagamento(_sessaoSistema.SessaoManager)
            {
                Historico = "ESTORNO DE LANÇAMENTO EM DOCUMENTO A PAGAR", 
                LancamentoId = ItemSelecionado.Id, 
                Usuario = _sessaoSistema.UsuarioLogado
            };

            servicoEstorno.FazerEstorno();
            AtualizarViewModel();

            _notificador.Notificar("documentoPagarSalvo", new NotificacaoArgs(DocumentoPagar));

            DialogBox.MostraInformacao("Lançamento estornado com sucesso");
        }

        public void AbrirFlyoutAdicionaDesconto()
        {
            _sessaoSistema
                .UsuarioLogado
                .VerificaPermissao
                .IsTemPermissaoThrow(Permissao.FINANCEIRO_DOCUMENTO_APAGAR_ADICIONAR_DESCONTO);

            FlyoutAdicionaDescontoModel = new FlyoutAdicionaDescontoModel(DocumentoPagar);
            FlyoutAdicionaDescontoModel.Retorno += RetornoAdicionaDescontoModel;
            FlyoutAdicionaDescontoModel.IsOpen = true;
        }

        private void RetornoAdicionaDescontoModel(object sender, FlyoutAdicionaDescontoArgs args)
        {
            if (args.Desconto == 0) return;

            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioDocumentoPagar(sessao);

                DocumentoPagar.AdicionarDesconto(args.Desconto, args.Historico);

                repositorio.Salvar(DocumentoPagar);
                transacao.Commit();
            }

            AtualizarViewModel();
            FlyoutAdicionaDescontoModel.IsOpen = false;

            _notificador.Notificar("documentoPagarSalvo", new NotificacaoArgs(DocumentoPagar));
        }
    }
}