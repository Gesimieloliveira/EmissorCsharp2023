using System;
using System.Windows.Input;
using Fusion.Visao.Pessoa;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Veiculos
{
    public class OpcoesDonoModel : ViewModel
    {
        public ICommand CommandNovaPessoa => GetSimpleCommand(NovaPessoaAction);
        public ICommand CommandFechar => GetSimpleCommand(FecharAction);

        public event EventHandler<object> RegistroSalvo;

        private void NovaPessoaAction(object obj)
        {
            var vm = new PessoaFormModel {IsTransportadora = true, IsCliente = false};
            vm.RegistroSalvo += (s, e) => OnRegistroSalvo(e);

            new PessoaForm(vm).ShowDialog();
            OnFechar();
        }

        private void FecharAction(object obj)
        {
            OnFechar();
        }

        protected virtual void OnRegistroSalvo(object e)
        {
            RegistroSalvo?.Invoke(this, e);
        }
    }
}