using System.Windows;
using System.Windows.Controls;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Pessoas;
using FusionWPF.Controles;

namespace Fusion.Controles
{
    public class PickerCliente : Control
    {
        private const string PartPickerCliente = "PART_PickerCliente";

        public static readonly DependencyProperty SelecionadoProperty = DependencyProperty.Register("Selecionado",
            typeof(Cliente),
            typeof(PickerCliente),
            new FrameworkPropertyMetadata(
                default(Cliente),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
            )
        );

        private SearchTextBox TbSearchCliente => (SearchTextBox) GetTemplateChild(PartPickerCliente);

        public Cliente Selecionado
        {
            get => (Cliente) GetValue(SelecionadoProperty);
            set => SetValue(SelecionadoProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (TbSearchCliente != null)
            {
                TbSearchCliente.SearchEvent -= PickerClienteClickHandler;
                TbSearchCliente.SearchEvent += PickerClienteClickHandler;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            TbSearchCliente?.Focus();
        }

        private void PickerClienteClickHandler(object sender, RoutedEventArgs e)
        {
            var vm = new PessoaPickerModel(new ClienteEngine());

            vm.PickItemEvent += (s, args) => { Selecionado = args.GetItem<Cliente>(); };

            vm.GetPickerView().ShowDialog();
        }
    }
}