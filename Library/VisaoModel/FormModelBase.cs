using System;

namespace FusionLibrary.VisaoModel
{
    public abstract class FormModelBase<T> : ModelBase
    {
        private T _model;

        public T Model
        {
            get => _model;
            set
            {
                _model = value;
                PreencherViewModel();
            }
        }

        protected void LancarExcecaoDeErro(string mensagem)
        {
            throw new InvalidOperationException(mensagem);
        }

        public virtual void SalvarModel()
        {
        }

        public virtual void DeletarModel()
        {
        }

        public virtual void PreencherViewModel()
        {
        }
    }
}