using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class ProdutoPerigosoView
    {
        public ProdutoPerigosoView()
        {
            InitializeComponent();
            Contexto = new ProdutoPerigosoContexto();
        }

        public ProdutoPerigosoContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
        }

        private void SalvarAlteracoesClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Contexto.AdicionarNovoProdutoPerigoso();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}