using System.Windows;
using System.Windows.Input;
using Fusion.Sessao;
using FusionWPF.Utils;
using NHibernate.Util;

namespace Fusion.Visao.Cfop
{
    public partial class CfopPickerView
    {
        public CfopPickerView()
        {
            InitializeComponent();
            Contexto = new CfopPickerContexto(SessaoSistema.Instancia.SessaoManager);

            RegistrarAtalho(Key.Escape, Close);
            RegistrarAtalho(Key.F6, () => FocarPesquisa());
        }

        public CfopPickerContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;

            AcaoCarregarDados();
        }

        private void AcaoCarregarDados()
        {
            Contexto.CarregarDados();
            FocarPesquisa();
        }

        private void FocarPesquisa()
        {
            TbPesquisa.SelectAll();
            TbPesquisa.Focus();
        }

        private void PesquisaRapidaClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoCarregarDados();
        }

        private void PesquiaRapidaKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                AcaoCarregarDados();
                return;
            }

            if (e.Key == Key.Down && ListBoxCfop.Items.Any())
            {
                e.Handled = true;

                ListBoxCfop.FocusFirstItem();
                return;
            }
        }

        private void ItemKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AcaoSelecionarItem();
            }
        }

        private void AcaoSelecionarItem()
        {
            if (Contexto.CfopSelecionado == null)
            {
                return;
            }

            Contexto.NotificarEscolhaDoCfop();
        }

        private void ItemDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            AcaoSelecionarItem();
        }
    }
}