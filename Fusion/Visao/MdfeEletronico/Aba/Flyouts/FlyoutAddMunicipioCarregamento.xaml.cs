using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts
{
    public partial class FlyoutAddMunicipioCarregamento
    {
        private FlyoutAddMunicipioCarregamentoModel _model;

        public FlyoutAddMunicipioCarregamento()
        {
            InitializeComponent();
        }

        private void OnClickBotaoAdicionaMunicipioCarregamento(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.SalvarMunicipioCarregamento();
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void FlyoutAddMunicipioCarregamento_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as FlyoutAddMunicipioCarregamentoModel;

            if (model == null) return;

            _model = model;
        }
    }
}
