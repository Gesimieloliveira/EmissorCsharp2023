using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class ListaMunicipioDescarregamento
    {
        public ListaMunicipioDescarregamento()
        {
            InitializeComponent();
        }

        private AbaMdfeCarregamentoModel Contexto => DataContext as AbaMdfeCarregamentoModel;

        private void RemoverDescarregamentoClickHandler(object sender, RoutedEventArgs e)
        {
            const string aviso = "Continuar com a exclusão do descarregamento selcionado?";

            if (!DialogBox.MostraDialogoDeConfirmacao(aviso))
            {
                return;
            }

            Contexto?.DeletarDescarregamentoSelecionado();
        }
    }
}