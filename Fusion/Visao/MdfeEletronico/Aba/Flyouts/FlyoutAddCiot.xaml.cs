using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts
{
    public partial class FlyoutAddCiot
    {
        public FlyoutAddCiot()
        {
            InitializeComponent();
        }

        private void OnClickBotaoSalvaCondutor(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = (DataContext) as FlyoutAddCiotModel;
                model?.SalvarCiot();
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
