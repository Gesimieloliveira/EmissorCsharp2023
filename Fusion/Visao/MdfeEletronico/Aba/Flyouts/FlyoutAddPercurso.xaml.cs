using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts
{
    public partial class FlyoutAddPercurso
    {
        public FlyoutAddPercurso()
        {
            InitializeComponent();
        }

        private void OnClickBotaoAdicionaPercurso(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = DataContext as FlyoutAddPercursoModel;
                model?.SalvarPercurso();
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
