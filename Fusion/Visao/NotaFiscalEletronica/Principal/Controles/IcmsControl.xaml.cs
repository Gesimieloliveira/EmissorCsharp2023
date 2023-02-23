using System.ComponentModel;
using System.Windows;
using FusionCore.Tributacoes.Flags;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Controles
{
    public partial class IcmsControl
    {
        public IcmsControl()
        {
            Contexto = new IcmsContexto();
            Contexto.PropertyChanged += PropertyChangedHandler;
            Contexto.RegimeTributarioChanged += RegimeTributarioChangedHandler;

            InitializeComponent();
        }

        public IcmsContexto Contexto { get; }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Contexto.CstSelecionado))
            {
                AjustarPermissoesImposto();
            }
        }

        private void RegimeTributarioChangedHandler(object sender, RegimeTributario e)
        {
            GbCredito.Visibility = e.Equals(RegimeTributario.SimplesNacional)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void AjustarPermissoesImposto()
        {
            GbCredito.IsEnabled = Contexto.CstSelecionado?.PermiteCredito() == true;
            GbIcms.IsEnabled = Contexto.CstSelecionado?.PermiteIcms() == true;
            GbIcmsSt.IsEnabled = Contexto.CstSelecionado?.PermiteSubstituicao() == true;
            TbReducaoIcms.IsEnabled = Contexto.CstSelecionado?.PermiteReducaoIcms() == true;
            GbFcp.IsEnabled = Contexto.CstSelecionado?.PermiteFcp() == true;
            GbFcpSt.IsEnabled = Contexto.CstSelecionado?.PermiteFcpSt() == true;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
        }
    }
}