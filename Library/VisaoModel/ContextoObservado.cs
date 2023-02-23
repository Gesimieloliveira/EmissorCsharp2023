using System;
using System.Linq.Expressions;

namespace FusionLibrary.VisaoModel
{
    public abstract class ContextoObservado<T> : ViewModel where T : class
    {
        public void QuandoMudar<TOut>(Expression<Func<T, TOut>> prop, Action<T> acao)
        {
            var expr = prop.Body as MemberExpression;
            if (expr?.Member == null) return;

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == expr.Member.Name)
                {
                    acao?.Invoke(this as T);
                }
            };
        }

        public void QuandoMudarComPartida<TOut>(Expression<Func<T, TOut>> prop, Action<T> acao)
        {
            acao.Invoke(this as T);
            QuandoMudar(prop, acao);
        }
    }
}