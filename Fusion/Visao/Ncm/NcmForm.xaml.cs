using System;
using System.Windows;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Ncm
{
    public partial class NcmForm
    {
        private readonly NcmFormModel _ncmFormModel;

        public NcmForm(NcmDTO ncmDTO)
        {
            _ncmFormModel = new NcmFormModel(ncmDTO);
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = _ncmFormModel;
            ConfiguraFormulario();
        }

        private void ConfiguraFormulario()
        {
            if (_ncmFormModel.Id.IsNullOrEmpty()) // novo registro
            {
                BotaoDeletar.Visibility = Visibility.Collapsed;
                BotaoSalvar.Content = "Salvar Inclusão";
                return;
            };
            Ccodigo.IsEnabled = false;
            TbDescricao.Focus();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _ncmFormModel.SalvarModel();
                DialogBox.MostraInformacao("Ncm salva com sucesso!");
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

        private void OnClickDeletar(object sender, RoutedEventArgs e)
        {
            if (DialogBox.MostraConfirmacao("Deseja prosseguir com a exclusão?") != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                _ncmFormModel.DeletarModel();
                DialogBox.MostraInformacao("Ncm foi deletada!");
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.GetType().ToString());
                DialogBox.MostraErro(ex.Message, ex);
            }
        }
    }
}
