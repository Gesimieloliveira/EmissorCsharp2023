using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.MdfeEletronico.Aba.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class InformacaoVeiculoTracao
    {
        public InformacaoVeiculoTracao()
        {
            InitializeComponent();
        }

        private void OnDoubleClickItem(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaRodoviarioMdfeModel;

            try
            {
                model?.DeletarVeiculoTracaoSelecionado();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
