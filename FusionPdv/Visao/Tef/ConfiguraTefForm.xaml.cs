using System.Windows;
using System.Windows.Input;

namespace FusionPdv.Visao.Tef
{
    public partial class ConfiguraTefForm
    {
        private readonly ConfiguraTefFormModel _configuraTefFormModel;

        public ConfiguraTefForm()
        {
            _configuraTefFormModel = new ConfiguraTefFormModel();
            DataContext = _configuraTefFormModel;
            InitializeComponent();
        }

        private void ConfiguraTefForm_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    SalvarArquivoTef();
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void SalvarArquivoTef()
        {
            if (!_configuraTefFormModel.BotaoSalvar) return;

            _configuraTefFormModel.Salvar();
        }

        private void Salvar_OnClick(object sender, RoutedEventArgs e)
        {
            SalvarArquivoTef();
        }


        private void Fechar_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
