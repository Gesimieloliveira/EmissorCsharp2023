using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.DocumentoAReceber
{
    public partial class ImpressaoDocumentosAhReceberView
    {
        private readonly ImpressaoDocumentosAhReceberContexto _contexto;

        public ImpressaoDocumentosAhReceberView(ImpressaoDocumentosAhReceberContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _contexto.CarregarDados();
            DataContext = _contexto;
        }

        private void ImprimirClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _contexto.FazerImpresao();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}