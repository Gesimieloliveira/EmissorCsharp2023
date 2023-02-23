using System.ComponentModel;

namespace Fusion.Visao.DocumentoAPagar
{
    public partial class OpcoesDocumento
    {
        public OpcoesDocumento(OpcoesDocumentoModel modelView)
        {
            var model = modelView; 
            model.Fechar += (sender, args) => Close();

            DataContext = model;
            InitializeComponent();
        }

        private void OpcoesDocumento_OnClosing(object sender, CancelEventArgs e)
        {
            DialogResult = true;
        }
    }
}
