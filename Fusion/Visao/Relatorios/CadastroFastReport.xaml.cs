using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Factories;
using FusionCore.Relatorios;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Relatorios
{
    public partial class CadastroFastReport
    {
        private readonly CadastroFastReportContexto _contexto;

        public CadastroFastReport(CadastroFastReportContexto contexto)
        {
            _contexto = contexto;
            _contexto.SalvouComSucesso += SalvouComSucessoHandler;

            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;

            if (_contexto.IsEdicao)
            {
                GpTemplate.Visibility = Visibility.Collapsed;
            }
        }

        private void SalvouComSucessoHandler(object sender, RelatorioProprio e)
        {
            DialogBox.MostraInformacao("Relatório foi salvo com sucesso!");
            DialogResult = true;
        }

        private void SalvarAlteracoesClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _contexto.SalvarAlteracoes();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ImportarClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = FileDialogFactory.CriaDialogFRX();

                if (dialog.ShowDialog() == true && dialog.CheckFileExists)
                {
                    _contexto.ArquivoFrx = dialog.FileName;
                }
            }
            finally
            {
                Keyboard.Focus(sender as UIElement);
            }
        }
    }
}