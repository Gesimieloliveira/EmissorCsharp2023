using System;
using System.Windows;

namespace Fusion.Visao.MdfeEletronico
{
    public partial class MdfeEmitirForm
    {
        private readonly int _idMdfe;

        public MdfeEmitirForm(int idMdfe)
        {
            _idMdfe = idMdfe;
            InitializeComponent();
            Contexto = new MdfeEmitirFormModel();
            Contexto.InicializarModel();
            DataContext = Contexto;
        }

        public MdfeEmitirForm() : this(0) { }

        public MdfeEmitirFormModel Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto.FecharTela += (o, args) => Dispatcher.Invoke(Close);
        }

        private async void MdfeEmitirForm_OnContentRendered(object sender, EventArgs e)
        {
            await RunTaskWithProgress(() => { Contexto.CarregarMdfe(_idMdfe); });

            Contexto.CarregarMdfeParaEdicao();
            Contexto.VerificarHistoricoPendente();
        }
    }
}
