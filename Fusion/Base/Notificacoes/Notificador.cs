using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NHibernate.Util;

namespace Fusion.Base.Notificacoes
{
    public class Notificador
    {
        private readonly IDictionary<string, IList<Action<NotificacaoArgs>>> _observadores
            = new ConcurrentDictionary<string, IList<Action<NotificacaoArgs>>>();

        public void Registrar(string nome, Action<NotificacaoArgs> comando)
        {
            if (!_observadores.ContainsKey(nome))
            {
                _observadores.Add(nome, new List<Action<NotificacaoArgs>>());
            }

            _observadores[nome].Add(comando);
        }

        public void Notificar(string nome, NotificacaoArgs args)
        {
            if (!_observadores.ContainsKey(nome))
            {
                return;
            }

            if (!_observadores[nome].Any())
            {
                return;
            }

            foreach (var observador in _observadores[nome])
            {
                observador.Invoke(args);
            }
        }
    }
}