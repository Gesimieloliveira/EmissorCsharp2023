using System;
using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.FusionAdm.TabelaIbpt;

namespace Fusion.Visao.CteEletronico.Emitir.Aba
{
    public partial class AbaCabecalhoCte
    {
        private AbaCabecalhoCteModel Model => DataContext as AbaCabecalhoCteModel;

        public AbaCabecalhoCte()
        {
            InitializeComponent();
        }

        private void OnClickProximoPasso(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.Proximo();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void PickerIbptClickHandler(object sender, RoutedEventArgs e)
        {
            var ibptPicker = new IbptPicker();
            ibptPicker.PickItemEvent += (o, args) => Model.Nbs = args.GetItem<Ibpt>();

            ibptPicker.GetPickerView().ShowDialog();
        }

        private void AlocarNumeracao_OnClick(object sender, RoutedEventArgs e)
        {
            const string msg =
                "Utilizar o próximo número definido no emissor? Utilize apenas para correção de duplicidade.";

            var confirm = DialogBox.MostraConfirmacao(msg);

            if (confirm == MessageBoxResult.Yes)
            {
                Model.AlocarNovaNumeracaoParaCTe();
            }
        }

        private void ClearIbptClickHandler(object sender, RoutedEventArgs e)
        {
            Model.Nbs = null;
        }
    }
}