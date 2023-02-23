using System;
using System.Windows.Input;
using Fusion.Sessao;
using FusionCore.FusionAdm.Estoque.Movimentacoes;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MovimentacaoEstoque.Flyouts
{
    public sealed class MovimentoEstoqueFlyoutModel : ModelBase
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;

        private MovimentoEstoque _movimento;
        private bool _isOpen;
        private TipoEventoEstoque _tipoEvento;
        private string _descricao;
        private DateTime _dataMovimento = DateTime.Now;

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        public TipoEventoEstoque TipoEvento
        {
            get { return _tipoEvento; }
            set
            {
                if (value == _tipoEvento) return;
                _tipoEvento = value;
                PropriedadeAlterada();
            }
        }

        public string Descricao
        {
            get { return _descricao; }
            set
            {
                if (value == _descricao) return;
                _descricao = value;
                PropriedadeAlterada();
            }
        }

        public DateTime DataMovimento
        {
            get { return _dataMovimento; }
            set
            {
                if (value.Equals(_dataMovimento)) return;
                _dataMovimento = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CloseCommand => GetSimpleCommand(CloseCommandHandler);

        private void CloseCommandHandler(object obj)
        {
            OnOperacaoFinalizada();
        }

        public event EventHandler<MovimentoEstoque> MovimentoCadastrado;
        public event EventHandler OperacaoFinalizada;

        private void OnMovimentoCadastrado(MovimentoEstoque e)
        {
            MovimentoCadastrado?.Invoke(this, e);
        }

        private void OnOperacaoFinalizada()
        {
            OperacaoFinalizada?.Invoke(this, EventArgs.Empty);
        }

        public void CarregaDados()
        {
            if (_movimento == null)
                return;

            TipoEvento = _movimento.TipoEvento;
            Descricao = _movimento.Descricao;
            DataMovimento = _movimento.DataMovimento;
        }

        public void SavarAlteracoes()
        {
            try
            {
                var movimento = GetMovimentoEstoque();

                if (_movimento == null)
                    OnMovimentoCadastrado(movimento);

                OnOperacaoFinalizada();
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private MovimentoEstoque GetMovimentoEstoque()
        {
            var movimento = _movimento ?? new MovimentoEstoque(Descricao, TipoEvento, _sessaoSistema.UsuarioLogado);

            movimento.Descricao = Descricao;
            movimento.DataMovimento = DataMovimento;

            return movimento;
        }
    }
}