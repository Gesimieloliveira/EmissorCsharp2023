using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts
{
    public partial class FlyoutAddValePedagio
    {
        public FlyoutAddValePedagio()
        {
            InitializeComponent();
        }

        private void OnClickBotaoAdicionarValePedagio(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = DataContext as FlyoutAddValePedagioModel;
                model?.SalvarValePedagio();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
