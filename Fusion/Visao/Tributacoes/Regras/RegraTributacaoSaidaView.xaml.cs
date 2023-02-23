using System;
using System.Windows;
using FusionCore.Excecoes.RegraNegocio;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Tributacoes.Regras
{
    public partial class RegraTributacaoSaidaView
    {
        private readonly RegraTributacaoSaidaContexto _contexto;

        public RegraTributacaoSaidaView(RegraTributacaoSaidaContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _contexto.Inicializa();
            DataContext = _contexto;
        }

        private void SalvarAlteracoesClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _contexto.SalvaAlteracoes();
                DialogBox.MostraInformacao("Regra foi salva com sucesso!");
                Close();
            }
            catch (RegraNegocioException ex)
            {
                DialogBox.MostraAviso(ex.Message, ex.Detalhes);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
