using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.PedidoDeVenda
{
    public partial class ClienteVisitanteControl
    {
        private readonly ClienteVisitanteContexto _contexto;

        public ClienteVisitanteControl(ClienteVisitanteContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;
            TbNome.Focus();
        }

        private void ConfirmarClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _contexto.ConfirmarVisitante();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
