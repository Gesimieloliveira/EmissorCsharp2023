using System.Windows;
using Fusion.Visao.CteEletronico.Perfil;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionWPF.FusionAdm.TabelaIbpt;

namespace Fusion.Visao.CteEletronico.Controls.Perfil
{
    public partial class Geral
    {
        private CtePerfilFormModel GetModel => (CtePerfilFormModel) DataContext;

        public Geral()
        {
            InitializeComponent();
        }

        private void OnClickAddEmissorFiscal(object sender, RoutedEventArgs e)
        {
            GetModel.AdicionarEmissorFiscal();
        }

        private void PickerNbsClick(object sender, RoutedEventArgs e)
        {
            var picker = new IbptPicker();
            picker.PickItemEvent += (o, args) => GetModel.Nbs = args.GetItem<Ibpt>();

            picker.ShowPickerDialog();
        }

        private void ClearNbsClick(object sender, RoutedEventArgs e)
        {
            GetModel.Nbs = null;
        }
    }
}