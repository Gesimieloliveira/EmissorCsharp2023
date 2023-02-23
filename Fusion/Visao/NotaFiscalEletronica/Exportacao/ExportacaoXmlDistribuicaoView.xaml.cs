namespace Fusion.Visao.NotaFiscalEletronica.Exportacao
{
    public partial class ExportacaoXmlDistribuicaoView
    {
        private readonly ExportacaoXmlDistribuicaoViewModel _model;

        public ExportacaoXmlDistribuicaoView()
        {
            InitializeComponent();
            _model = new ExportacaoXmlDistribuicaoViewModel();
            DataContext = _model;
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _model.Inicializar();
        }
    }
}