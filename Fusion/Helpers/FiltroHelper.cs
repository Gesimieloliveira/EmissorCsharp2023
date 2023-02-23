using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fusion.Helpers
{
    public static class FiltroHelper
    {
        public static void RegitrarAtalhoFiltro(UIElement element, Button botao)
        {
            botao.ToolTip = "Use [Ctrl + Enter] no campo para pesquisa rápida";  

            element.PreviewKeyDown += (sender, args) =>
            {
                if (args.KeyboardDevice.Modifiers == ModifierKeys.Control && args.Key == Key.Enter)
                {
                    if (args.OriginalSource is FrameworkElement tb)
                    {
                        tb.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
                        tb.GetBindingExpression(ComboBox.TextProperty)?.UpdateSource();
                    }

                    var peer = new ButtonAutomationPeer(botao);
                    var inokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;

                    inokeProvider?.Invoke();
                }
            };
        }
    }
}