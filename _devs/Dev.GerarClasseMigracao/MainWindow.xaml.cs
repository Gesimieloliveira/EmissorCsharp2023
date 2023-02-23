using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Dev.GerarClasseMigracao
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowModel();
        }

        private void TbMigracaoTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, @"[^\w]", RegexOptions.IgnoreCase);
        }

        private void TbMigracaoKeyDownHandler(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }
    }
}
