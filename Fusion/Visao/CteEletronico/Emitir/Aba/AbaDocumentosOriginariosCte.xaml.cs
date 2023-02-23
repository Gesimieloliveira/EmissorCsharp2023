using System;
using System.IO;
using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.Aba
{
    public partial class AbaDocumentosOriginariosCte
    {
        public AbaDocumentosOriginariosCte()
        {
            InitializeComponent();
        }

        private AbaDocumentosOriginariosModel ViewModel => DataContext as AbaDocumentosOriginariosModel;

        private void ClickAdicionarNfeHandler(object sender, RoutedEventArgs e)
        {
            CatchException(ViewModel.OnAdicionaNfeCall);
        }

        private void ClickNotaFiscalImpressaHandler(object sender, RoutedEventArgs e)
        {
            CatchException(ViewModel.OnAdicionaNfImpressaCall);
        }

        private void ClickOutrosDocumentoHandler(object sender, RoutedEventArgs e)
        {
            CatchException(ViewModel.OnAdicionaNfOutroDocumentoCall);
        }

        private void ClickDocumentoAnteriorHandler(object sender, RoutedEventArgs e)
        {
            CatchException(ViewModel.OnAdicionaDocumentoAnteriorCall);
        }

        private void OnClickBotaoAnterior(object sender, RoutedEventArgs e)
        {
            CatchException(ViewModel.Anterior);
        }

        private void OnClickProximoPasso(object sender, RoutedEventArgs e)
        {
            CatchException(ViewModel.OnProximoPasso);
        }

        private static void CatchException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (InvalidDataException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (Exception e)
            {
                DialogBox.MostraErro(e.Message, e);
            }
        }

        private void ClickComponenteValorPrestacaoHandler(object sender, RoutedEventArgs e)
        {
            CatchException(ViewModel.OnAdicionaComponenteValorPrestacaoCall);
        }
    }
}