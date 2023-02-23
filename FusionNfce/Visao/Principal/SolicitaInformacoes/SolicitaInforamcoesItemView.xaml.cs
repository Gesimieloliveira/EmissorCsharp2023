using System;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Principal.SolicitaInformacoes
{
    public partial class SolicitaInforamcoesItemView 
    {
        private readonly SolicitaInforamcoesItemContexto _model;

        public SolicitaInforamcoesItemView(SolicitaInforamcoesItemContexto model)
        {
            InitializeComponent();
            DataContext = model;
            _model = model;
        }

        private void EnviarProduto_OnKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    _model.EnviarProduto();
                    e.Handled = true;
                }
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
