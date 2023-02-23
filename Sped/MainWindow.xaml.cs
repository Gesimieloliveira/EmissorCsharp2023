using System;

namespace Sped
{
    public partial class MainWindow
    {
        private readonly MainWindowModel _model;

        public MainWindow()
        {
            _model = new MainWindowModel();
            DataContext = _model;
            InitializeComponent();
        }

        private void MainWindow_OnContentRendered(object sender, EventArgs e)
        {
            _model.Inicializa();
        }
    }
}
