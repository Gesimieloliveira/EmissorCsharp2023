using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.GerenciadorManifestacoesDestinatarios
{
    public partial class JustificativaOperacaoNaoRealizada
    {
        private readonly JustificativaOperacaoNaoRealizadaModel _model;

        public JustificativaOperacaoNaoRealizada(JustificativaOperacaoNaoRealizadaModel model)
        {
            _model = model;
            InitializeComponent();
            DataContext = _model;
        }


        private void AdicionarJustificativa_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.Validar();
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
