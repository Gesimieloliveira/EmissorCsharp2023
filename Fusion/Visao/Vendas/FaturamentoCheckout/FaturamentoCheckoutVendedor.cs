using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.Pessoa.Picker;
using Fusion.Visao.Pessoa.Picker.OpcoesBusca;
using FusionCore.FusionAdm.Pessoas;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void OnClickVendedor(object sender, RoutedEventArgs e)
        {
            AcaoVincularVendedor();
        }

        private void AcaoVincularVendedor()
        {
            var picker = new PessoaPickerModel(new VendedorEngine())
            {
                Titulo = "Escolha um vendedor para o faturamento"
            };

            picker.UsarBusca<BuscaPessoaPickerVendedor>();
            picker.InicializarComPesquisa(string.Empty);

            picker.PickItemEvent += (o, args) =>
            {
                try
                {
                    var vendedor = args.GetItem<Vendedor>();
                    ViewModel.VincularVendedor(vendedor);
                }
                catch (InvalidOperationException e)
                {
                    DialogBox.MostraAviso(e.Message);
                }
                finally
                {
                    Keyboard.Focus(ControlCheckoutBox);
                }
            };

            picker.GetPickerView().ShowDialog();
        }
    }
}