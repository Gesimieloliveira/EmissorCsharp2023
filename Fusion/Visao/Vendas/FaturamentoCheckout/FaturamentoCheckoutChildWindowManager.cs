using System.Windows.Input;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private ChildWindow _childAberta;

        protected bool PossuiChildAberta() => _childAberta?.IsOpen == true;
        protected bool NaoPossuiChildAberta() => !PossuiChildAberta();

        private void AbrirChildWindow(ChildWindow view)
        {
            _childAberta = view;

            _childAberta.IsOpenChanged += (o, args) =>
            {
                if (_childAberta.IsOpen)
                {
                    return;
                }

                // Contexto.Inicializar();
                Keyboard.Focus(ControlCheckoutBox);
            };

            this.ShowChildWindowAsync(_childAberta);
        }
    }
}