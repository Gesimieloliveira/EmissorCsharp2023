using System;
using System.Windows.Input;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.Tef;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Tef
{
    public class TefPosFormModel : ViewModel
    {
        private readonly Pos _pos;

        private string _descricao;
        private string _cnpjCredenciadora;

        public TefPosFormModel(Pos pos)
        {
            _pos = pos;
        }

        public event EventHandler Fechar;

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (value == _descricao) return;
                _descricao = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandSalvar => GetSimpleCommand(SalvarAction);

        public string CnpjCredenciadora
        {
            get => _cnpjCredenciadora;
            set
            {
                if (value == _cnpjCredenciadora) return;
                _cnpjCredenciadora = value;
                PropriedadeAlterada();
            }
        }

        public bool Status
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }


        private void SalvarAction(object obj)
        {
            try
            {
                HidratarValor();
                ValidarDescricao();
                ValidarNFce();
                MontarObjeto();

                var sessao = SessaoHelperFactory.AbrirSessaoAdm();
                var transacao = sessao.BeginTransaction();

                using (sessao)
                using (transacao)
                {
                    var repositorio = new RepositorioPos(sessao);

                    repositorio.SalvarOuAtualizar(_pos);

                    transacao.Commit();
                }

                OnFechar();
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void ValidarNFce()
        {
            if (CnpjCredenciadora.IsNullOrEmpty())
                throw new ArgumentException("Cnpj campo obrigatório");
        }

        private void ValidarDescricao()
        {
            if (Descricao.IsNullOrEmpty())
                throw new ArgumentException("Descrição campo obrigatório");
        }

        private void MontarObjeto()
        {
            _pos.Descricao = Descricao;
            _pos.CnpjCredenciadora = CnpjCredenciadora;
            _pos.Serial = string.Empty;
            _pos.EstabelecimentoCodigo = string.Empty;
            _pos.Adquirente = string.Empty;
            _pos.FlagMfe = false;
            _pos.FlagNfce = true;
            _pos.Credenciadora = Credenciadora.Outros;
            _pos.Status = Status;
        }

        private void HidratarValor()
        {
            Descricao = Descricao.TrimOrEmpty();
            CnpjCredenciadora = CnpjCredenciadora.TrimOrEmpty();
        }

        private void LimparCampos()
        {
            Descricao = string.Empty;
            CnpjCredenciadora = string.Empty;
        }
        protected virtual void OnFechar()
        {
            Fechar?.Invoke(this, EventArgs.Empty);
        }

        public void Load()
        {
            Descricao = _pos.Descricao;
            CnpjCredenciadora = _pos.CnpjCredenciadora;
            Status = _pos.Status;
        }
    }
}