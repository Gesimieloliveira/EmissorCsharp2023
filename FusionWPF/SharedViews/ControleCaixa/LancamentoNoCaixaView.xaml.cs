using System;
using System.Windows;
using System.Windows.Input;
using FusionCore.DI;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionWPF.SharedViews.ControleCaixa
{
    public partial class LancamentoNoCaixaView
    {
        public LancamentoNoCaixaView(
            ISessaoManager sessaoManager,
            IControleCaixaProvider caixaProvider)
        {
            InitializeComponent();

            RegistrarAtalhoBotao(Key.F2, BtnSalvar);
            RegistrarAtalho(Key.Escape, Close);

            Contexto = new LancamentoNoCaixaContexto(sessaoManager, caixaProvider);
        }

        public LancamentoNoCaixaContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
            Contexto.Inicializar();

            if (Contexto.IsNovo)
            {
                CbTipoOperacao.Focus();
            }
        }

        private void LancarMovimentoClickHandler(object sender, RoutedEventArgs e)
        {
            BtnSalvar.Focus();

            try
            {
                Contexto.ValidarInformacoes();
                Contexto.IncluirLancamento();

                DialogBox.MostraInformacao("Lançamento foi salvo com sucesso");

                DialogResult = true;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}