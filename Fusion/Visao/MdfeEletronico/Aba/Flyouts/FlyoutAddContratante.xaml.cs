using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts
{
    public partial class FlyoutAddContratante
    {
        public FlyoutAddContratante()
        {
            InitializeComponent();
        }

        private void OnClickBotaoCondutor(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = (DataContext) as FlyoutAddContratanteModel;
                model?.SalvarContratante();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
