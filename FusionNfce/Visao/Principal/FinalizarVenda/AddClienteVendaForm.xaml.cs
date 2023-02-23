using System;
using System.Windows.Input;

namespace FusionNfce.Visao.Principal.FinalizarVenda
{
    public partial class AddClienteVendaForm
    {
        private AddClienteVendaFormModel Model { get; set; }

        public AddClienteVendaForm(AddClienteVendaFormModel model)
        {
            Model = model;
            Model.Fechar += Fechar;
            DataContext = Model;
            InitializeComponent();
        }

        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }

        private void AddClienteVendaForm_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();

            if (e.Key == Key.F5)
                Model.BuscaClienteAction(sender);
        }

        private void AddClienteVendaForm_OnContentRendered(object sender, EventArgs e)
        {
            Model.OnRendered();
        }
    }
}
