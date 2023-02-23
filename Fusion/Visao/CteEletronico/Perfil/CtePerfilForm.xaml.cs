using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Perfil
{
    public partial class CtePerfilForm
    {
        private readonly CtePerfilFormModel _model;

        public CtePerfilForm(CtePerfilFormModel model)
        {
            _model = model;
            _model.Fechar += Fechar;
            DataContext = model;
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
           // BarraFerramenta.ShowDelete = false;
        }
        
        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            _model.Salvar();
        }

        private void OnClickFechar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnClickDeletar(object sender, RoutedEventArgs e)
        {
            _model.Deletar();
            DialogBox.MostraMensagemDeletouComSucesso();
            Close();
        }

        private void CteForm_OnContentRendered(object sender, EventArgs e)
        {
            _model.Inicializa();
        }
    }
}