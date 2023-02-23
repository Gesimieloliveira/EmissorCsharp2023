using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FusionLibrary.Command;
using JetBrains.Annotations;

namespace FusionLibrary.VisaoModel
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        public event EventHandler Fechar;
        private readonly IDictionary<string, SimpleCommand> _commandPool = new Dictionary<string, SimpleCommand>();
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void PropriedadeAlterada([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected ICommand GetSimpleCommand(
            Action<object> action,
            bool canExecute = true,
            [CallerMemberName] string commandName = null)
        {
            if (_commandPool.ContainsKey(commandName))
            {
                return _commandPool[commandName];
            }

            return _commandPool[commandName] = new SimpleCommand
            {
                ExecuteDelegate = action,
                CanExecuteDelegate = p => canExecute
            };
        }

        public virtual void OnFechar()
        {
            Fechar?.Invoke(this, EventArgs.Empty);
        }
    }
}