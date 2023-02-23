using System;
using FusionCore.FusionAdm.Pessoas;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using static System.String;

namespace Fusion.Visao.Pessoa.SubFormularios
{
    public sealed class TelefoneFormModel : ViewModel
    {
        private PessoaTelefone _telefone;
        private bool _novo;
        private bool _isPessoaAlterar;

        public string Descricao
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Numero
        {
            get => GetValue();
            set => SetValue(value);
        }

        public TelefoneFormModel()
        {
            IsPessoaAlterar = true;
            _novo = true;
        }

        public TelefoneFormModel(PessoaTelefone telefone, bool isPessoaAlterar)
        {
            IsPessoaAlterar = isPessoaAlterar;
            _telefone = (PessoaTelefone) telefone.Clone();
        }

        public bool IsPessoaAlterar
        {
            get => _isPessoaAlterar;
            set
            {
                if (value == _isPessoaAlterar) return;
                _isPessoaAlterar = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<PessoaTelefone> TelefoneAdicionado;
        public event EventHandler<PessoaTelefone> TelefoneEditado;
        public event EventHandler Finalizado;

        public void CarregarDados()
        {
            Descricao = _telefone?.Descricao ?? Empty;
            Numero = _telefone?.Numero ?? Empty;
        }

        private void OnTelefoneAdicionado(PessoaTelefone telefone)
        {
            TelefoneAdicionado?.Invoke(this, telefone);
        }

        private void OnTelefoneEditado(PessoaTelefone e)
        {
            TelefoneEditado?.Invoke(this, e);
        }

        private void OnFinalizado()
        {
            Finalizado?.Invoke(this, EventArgs.Empty);
        }

        public void ConfirmarTelefone()
        {
            try
            {
                if (_novo)
                {
                    _telefone = new PessoaTelefone(Descricao, Numero);

                    OnTelefoneAdicionado(_telefone);
                    OnFinalizado();
                    return;
                }

                _telefone.Descricao = Descricao;
                _telefone.Numero = Numero;

                OnTelefoneEditado(_telefone);
                OnFinalizado();
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

    }
}