using System;
using FusionLibrary.VisaoModel;

namespace FusionWPF.CapturarPeso
{
    public sealed class CapturarPesoBalancaContexto : ViewModel
    {
        public CapturarPesoBalancaContexto(string nomeItem)
        {
            NomeItem = nomeItem;
            TextoAviso = "Sem comunicação com a balança!!";
        }

        public decimal PesoItem
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public string TextoInformativo
        {
            get => GetValue();
            set => SetValue(value?.ToUpper());
        }

        public string NomeItem
        {
            get => GetValue();
            private set => SetValue(value?.ToUpper());
        }

        public string TextoAviso
        {
            get => GetValue();
            private set => SetValue(value);
        }

        public event EventHandler<decimal> PesoConfirmado;

        public void ConfirmarPeso()
        {
            if (PesoItem <= 0)
            {
                return;
            }

            PesoConfirmado?.Invoke(this, PesoItem);
        }
    }
}