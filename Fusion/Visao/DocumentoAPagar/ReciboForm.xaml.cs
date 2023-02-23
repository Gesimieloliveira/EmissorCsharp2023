using System;

namespace Fusion.Visao.DocumentoAPagar
{
    public partial class ReciboForm
    {
        public ReciboFormModel Model { get; }

        public ReciboForm()
        {
            Model = new ReciboFormModel();
            DataContext = Model;
            InitializeComponent();
            Loaded += OnLoaded;
            Model.FocusTela += FocusTela;
        }

        private void FocusTela(object sender, EventArgs e)
        {
            Focus();
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            Model.Inicializa();
        }
    }
}
