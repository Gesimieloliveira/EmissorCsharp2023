using System;
using System.Windows.Input;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Usuario
{
    public class PapelUsuarioFormModel : ViewModel
    {
        private string _descricao;
        private readonly Papel _papel;

        public PapelUsuarioFormModel(Papel papel)
        {
            _papel = papel;
            Descricao = _papel.Descricao ?? string.Empty;
        }

        public ICommand CommandInserirUsuario => GetSimpleCommand(InvocarInserirUsuario);

        public string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<Papel> OperacaoSucesso;

        private void InvocarInserirUsuario(object obj)
        {
            Descricao = Descricao.TrimOrEmpty();

            if (Descricao.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Digite uma descrição para o papel");
                return;
            }

            if (Descricao.Length < 3)
            {
                DialogBox.MostraInformacao("Descrição deve ter no mínimo 3 digitos");
                return;
            }

            SalvaAlteracoes();

            OperacaoSucesso?.Invoke(this, _papel);
        }

        private void SalvaAlteracoes()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _papel.Descricao = Descricao;

                new RepositorioPapel(sessao).SalvarAlteracoes(_papel);

                transacao.Commit();
            }
        }
    }
}