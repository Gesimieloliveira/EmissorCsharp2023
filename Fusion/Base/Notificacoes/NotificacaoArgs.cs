namespace Fusion.Base.Notificacoes
{
    public class NotificacaoArgs
    {
        private readonly object[] _args;

        private NotificacaoArgs()
        {
            _args = new object[] { };
        }

        public NotificacaoArgs(params object[] args)
            : this()
        {
            _args = args;
        }

        public static NotificacaoArgs Empty => new NotificacaoArgs();

        public bool Possui<T>()
        {
            foreach (var o in _args)
            {
                if (o is T) return true;
            }

            return false;
        }

        public T ObterArg<T>()
        {
            foreach (var o in _args)
            {
                if (o is T obj) return obj;
            }

            return default(T);
        }
    }
}