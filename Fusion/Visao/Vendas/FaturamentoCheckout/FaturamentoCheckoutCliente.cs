using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Pessoas;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void OnClickCliente(object sender, RoutedEventArgs e)
        {
            AcaoCliente();
        }

        private void AcaoCliente()
        {
            var picker = new PessoaPickerModel(new ClienteEngine());

            picker.PickItemEvent += (o, args) =>
            {
                try
                {
                    var cliente = args.GetItem<Cliente>();
                    ViewModel.VincularCliente(cliente);
                }
                catch (InvalidOperationException ex)
                {
                    DialogBox.MostraAviso(ex.Message);
                }
            };

            var view = new GridPicker(picker);
            view.ShowDialog();

            Keyboard.Focus(ControlCheckoutBox);
        }

        private void TextBoxCliente_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            AcaoCliente();
        }
    }
}