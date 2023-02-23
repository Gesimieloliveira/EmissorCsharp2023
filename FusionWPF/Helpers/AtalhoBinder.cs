using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;

namespace FusionWPF.Helpers
{
    public class AtalhoBinder
    {
        private readonly Dictionary<Key, ActionGroup> _atalhos;

        private AtalhoBinder(IInputElement control)
        {
            _atalhos = new Dictionary<Key, ActionGroup>();
            control.PreviewKeyDown += ControlOnPreviewKeyDown;
        }

        public static AtalhoBinder Iniciar(IInputElement element)
        {
            return new AtalhoBinder(element);
        }

        private void ControlOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!_atalhos.ContainsKey(e.Key))
            {
                return;
            }

            e.Handled = _atalhos[e.Key].Invoke();
        }

        public AtalhoBinder BindBotao(Key key, Button button, Func<bool> condition = null)
        {
            var action = new Action(() =>
            {
                if (!button.IsEnabled || !button.IsVisible)
                {
                    return;
                }

                button.Focus();

                var peer = new ButtonAutomationPeer(button);
                var inokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;

                inokeProvider?.Invoke();
            });

            BindAction(key, action, condition);

            return this;
        }

        public AtalhoBinder BindAction(Key key, Action acao, Func<bool> condition = null)
        {
            if (_atalhos.ContainsKey(key))
            {
                _atalhos[key] = new ActionGroup(acao, condition);
                return this;
            }

            _atalhos.Add(key, new ActionGroup(acao, condition));
            return this;
        }

        private class ActionGroup
        {
            private readonly Action _action;
            private readonly Func<bool> _condition;

            private bool _inProgress;

            public ActionGroup(Action action, Func<bool> condition = null)
            {
                _action = action;
                _condition = condition;
            }

            public bool Invoke()
            {
                if (_inProgress) return true;
                if (_condition != null && _condition() == false) return false;

                _inProgress = true;

                try
                {
                    _action();
                }
                finally
                {
                    _inProgress = false;
                }

                return true;
            }
        }
    }
}