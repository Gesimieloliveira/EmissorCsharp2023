using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Helpers;

namespace Fusion.Visao.PedidoDeVenda
{
    public partial class EditarItemView
    {
        private readonly EditarItemViewModel _contexto;

        public EditarItemView(EditarItemViewModel contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;

            TbObs.Focus();
            TbObs.MoveCaretToEnd();
        }

        private void AplicarAlteracoesClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _contexto.AplicarAlteracoes();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
