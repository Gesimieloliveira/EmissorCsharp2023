using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Visao.NotaFiscalEletronica.Principal;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.ConverterVendaParaNfe
{
    public partial class ConverterParaNfeForm
    {
        private readonly ConverterParaNfeFormModel _model;

        public ConverterParaNfeForm(ConverterParaNfeFormModel model)
        {
            InitializeComponent();
            _model = model;
            DataContext = _model;
        }

        private void DoubleClickListItem(object sender, MouseButtonEventArgs e)
        {
            ConverterPedidoParaNFe(null);
        }

        private void ClickItemPerfil(object sender, RoutedEventArgs e)
        {
            var perfilDto = ((Button) sender);

            ConverterPedidoParaNFe(perfilDto.Tag as AbaPerfilNfeDTO);
        }

        private void ConverterPedidoParaNFe(AbaPerfilNfeDTO abaPerfilNfeDTO)
        {
            try
            {
                var nfeId = _model.ConverterPedidoParaNFe(_model.ItemSelecionado ?? abaPerfilNfeDTO);

                new NfeletronicaWizzard(new NfeletronicaGrid
                {
                    Id = nfeId
                }).ShowDialog();

                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
