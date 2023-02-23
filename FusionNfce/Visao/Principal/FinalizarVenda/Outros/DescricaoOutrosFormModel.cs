using System;
using System.Windows.Input;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Contratos;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Principal.FinalizarVenda.Outros
{
    public class DescricaoOutrosFormModel : ViewModel, IChildContext
    {
        private string _descricaoOutros;
        public ICommand EnviarDescricaoOutrosCommand => GetSimpleCommand(EnviarDescroOutrosAction);

        public string TituloChild => "Descrição forma pagamento outros";
        public EventHandler<DescricaoOutrosFormModel> EnviarDescricaoOutros { get; set; }

        public event EventHandler SolicitaFechamento;

        public string DescricaoOutros
        {
            get => _descricaoOutros;
            set
            {
                _descricaoOutros = value;
                PropriedadeAlterada();
            }
        }

        private void EnviarDescroOutrosAction(object obj)
        {
            DescricaoOutros = DescricaoOutros.TrimOrEmpty();

            if (DescricaoOutros.Length < 2)
            {
                DialogBox.MostraAviso("Descrição precisa ser maior que 2 caracteres.");
                return;
            }

            EnviarDescricaoOutros.Invoke(obj, this);
        }
    }
}