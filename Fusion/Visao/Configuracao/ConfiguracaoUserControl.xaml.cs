using System.Windows;
using System.Windows.Forms;
using Fusion.Visao.Configuracao.Model;

namespace Fusion.Visao.Configuracao
{
    public partial class ConfiguracaoUserControl
    {
        public ConfiguracaoUserControl(ConfiguracaoModel contexto)
        {
            InitializeComponent();
            DataContext = contexto;
        }

        private ConfiguracaoModel ViewModel => DataContext as ConfiguracaoModel;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel?.Inicializa();
        }

        private void ClickDiretorioExportacaoHandler(object sender, RoutedEventArgs e)
        {
            using (var fdb = new FolderBrowserDialog())
            {
                var resul = fdb.ShowDialog();

                if (resul != DialogResult.OK)
                {
                    return;
                }

                ViewModel.FusionServico.DiretorioExportacao = fdb.SelectedPath;
            }
        }
    }
}