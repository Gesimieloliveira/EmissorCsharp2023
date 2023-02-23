using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.PedidoDeVenda
{
    public partial class FormCancelamentoPedido
    {
        private readonly FormCancelamentoPedidoModel _modelo;

        public FormCancelamentoPedido(FormCancelamentoPedidoModel modelo)
        {
            InitializeComponent();
            _modelo = modelo;
            RegistrarAtalho(Key.Escape, Close);
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _modelo;
            TbMotivoCancelamento.Focus();
        }

        private void ConfirmarClickHandler(object sender, RoutedEventArgs e)
        {
            if (_modelo.MotivoCancelamento == null || _modelo.MotivoCancelamento?.Length < 15)
            {
                DialogBox.MostraAviso("Observação não pode ter menos que 15 caracteres!");
                return;
            }

            DialogResult = true;
        }
    }
}
