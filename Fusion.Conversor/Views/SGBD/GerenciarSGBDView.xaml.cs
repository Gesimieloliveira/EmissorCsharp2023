using System;
using System.Windows;
using System.Windows.Forms;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Conversor.Views.SGBD
{
    public partial class GerenciarSGBDView
    {
        private readonly GerenciarSGBDConexto _contexto;

        public GerenciarSGBDView(GerenciarSGBDConexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private async void LoadedHandler(object sender, RoutedEventArgs e)
        {
            await RunTaskWithProgress(AsyncLoaded);

            DataContext = _contexto;
        }

        private void AsyncLoaded()
        {
            try
            {
                _contexto.CarregarBancosExistentes();
            }
            catch (Exception e)
            {
                DialogBox.MostraErro(e.Message, e);
                Close();
            }
        }

        private async void CriarBancoClickHandler(object sender, RoutedEventArgs e)
        {
            var pergunta = $"Deseja continuar com a criação do banco de dados {_contexto.NomeBancoNovo}";

            if (DialogBox.MostraConfirmacao(pergunta) != MessageBoxResult.Yes)
            {
                return;
            }

            await RunTaskWithProgress(() =>
            {
                try
                {
                    _contexto.CriarNovoBancoDados();
                    _contexto.CarregarBancosExistentes();

                    DialogBox.MostraInformacao("Banco de dados criado com sucesso");
                    DialogBox.MostraInformacao("Não esqueça de conferir a conexão e atualiza-lo");
                }
                catch (Exception ex)
                {
                    DialogBox.MostraErro(ex.Message, ex);
                }
            });
        }

        private async void FazerBackupClickHandler(object sender, RoutedEventArgs e)
        {
            var explorerDialog = new FolderBrowserDialog();
            var result = explorerDialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            var destino = explorerDialog.SelectedPath;

            await RunTaskWithProgress(() =>
            {
                try
                {
                    _contexto.FazerBackup(destino);
                    DialogBox.MostraInformacao($"Tudo certo! Seu backup está em: {destino}");
                }
                catch (Exception ex)
                {
                    DialogBox.MostraErro("Oops... não consegui fazer o backup", ex);
                }
            });
        }
    }
}
