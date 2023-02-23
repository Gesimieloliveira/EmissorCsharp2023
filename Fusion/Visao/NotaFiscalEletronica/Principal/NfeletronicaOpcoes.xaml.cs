using System;
using System.Windows;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Visao.NotaFiscalEletronica.Principal
{
    public partial class NfeletronicaOpcoes
    {
        private NfeletronicaOpcoesModel ViewModel => DataContext as NfeletronicaOpcoesModel;

        public NfeletronicaOpcoes(Nfeletronica nfe)
        {
            DataContext = new NfeletronicaOpcoesModel(nfe);
            InitializeComponent();
        }

        public NfeletronicaOpcoes(NfeletronicaGrid nfeGrid)
            : this(CarregaNfe(nfeGrid))
        {
        }

        private static Nfeletronica CarregaNfe(NfeletronicaGrid nfeGrid)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioNfe(sessao);
                return repositorio.GetPeloId(nfeGrid.Id);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.JanelaCancelarFechada += JanelaCancelarFechadaHandler;
        }

        private void JanelaCancelarFechadaHandler(object sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}