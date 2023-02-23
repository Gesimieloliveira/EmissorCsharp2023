using System.ComponentModel;

namespace FusionLibrary.VisaoModel
{
    public abstract class AutoSaveModel : ViewModel
    {
        private bool _inicializando;

        public void Inicializa()
        {
            _inicializando = true;

            try
            {
                OnInicializa();

                PropertyChanged -= PropertyChangedHandler;
                PropertyChanged += PropertyChangedHandler;
            }
            finally
            {
                _inicializando = false;
            }
        }

        protected abstract void OnInicializa();

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (_inicializando)
            {
                return;
            }

            OnSalvaAlteracoes();
        }

        protected abstract void OnSalvaAlteracoes();
    }
}
