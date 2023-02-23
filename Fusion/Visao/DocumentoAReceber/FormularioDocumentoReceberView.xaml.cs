using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.DocumentoAReceber
{
    public partial class FormularioDocumentoReceberView
    {
        public FormularioDocumentoReceberView(FormularioDocumentoReceberContexto contexto)
        {
            Contexto = contexto;
            InitializeComponent();
        }

        public bool WindowResult { get; private set; }
        public FormularioDocumentoReceberContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;

            Contexto.EmpresaContexto.CarregarEmpresasDisponiveis();
            Contexto.TipoDocumentoContexto.CarregarTipos();
            ConfigurarTela();
        }

        private void ConfigurarTela()
        {
            if (Contexto.EhNovoRegistro)
            {
                BotaoSalvar.Content = "Salvar Inclusão";
                BotaoCancelar.Visibility = Visibility.Collapsed;
                BotaoEstornar.Visibility = Visibility.Collapsed;
            };
        }

        private void SalvarDocumentoLCickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Contexto.ThrowExceptionSeDadosInvalido();
                Contexto.SalvarAlteracoes();

                DialogBox.MostraInformacao("Documento foi salvo com sucesso!");

                WindowResult = true;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void CancelarDocumentoClickHandler(object sender, RoutedEventArgs e)
        {
            const string msg = "Cancelamento não podera ser desfeito! Quer mesmo cancelar este documento?";

            if (DialogBox.MostraDialogoDeConfirmacao(msg) == false)
            {
                return;
            }

            try
            {
                Contexto.FazerCancelamento();
                DialogBox.MostraInformacao("Documento foi cancelado com sucesso!");
                WindowResult = true;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void EstornarLancamentoClickHandler(object sender, RoutedEventArgs e)
        {
            var msg = "Continuar com o estorno do utlimo lançamento que ainda não foi estornado?";

            if (DialogBox.MostraDialogoDeConfirmacao(msg) == false)
            {
                return;
            }

            try
            {
                Contexto.EstornarUltimoLancamento();
                WindowResult = true;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}