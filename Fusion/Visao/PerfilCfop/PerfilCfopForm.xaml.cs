using System;
using System.Windows;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.PerfilCfop
{
    public partial class PerfilCfopForm
    {
        private PerfilCfopFormModel ViewModel => DataContext as PerfilCfopFormModel;

        public PerfilCfopForm(PerfilCfopDTO perfilCfop)
        {
            DataContext = new PerfilCfopFormModel(perfilCfop);
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.AtualizaView();
        }

        private void SalvarHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.SalvarModel();
                DialogBox.MostraInformacao("Perfil Cfop salvo com sucesso");

                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }

        private void FecharHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
