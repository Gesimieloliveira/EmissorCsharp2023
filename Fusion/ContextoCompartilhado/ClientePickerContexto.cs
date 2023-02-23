using System.Windows.Input;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Pessoas;
using FusionLibrary.VisaoModel;

namespace Fusion.ContextoCompartilhado
{
    public class ClientePickerContexto : ViewModel
    {
        public Cliente ClienteSelecionado
        {
            get => GetValue<Cliente>();
            set => SetValue(value);
        }

        public ICommand PickerCommand => GetSimpleCommand(o =>
        {
            var vm = new PessoaPickerModel(new ClienteEngine());
            vm.PickItemEvent += (sender, args) => { ClienteSelecionado = args.GetItem<Cliente>(); };

            vm.GetPickerView().ShowDialog();
        });
    }
}