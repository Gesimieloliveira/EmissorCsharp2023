using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts
{
    public partial class FlyoutAddLacre
    {
        public FlyoutAddLacre()
        {
            InitializeComponent();
        }

        private void OnClickBotaoAdicionaLacre(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = DataContext as FlyoutAddLacreModel;
                model?.SalvarLacre();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
