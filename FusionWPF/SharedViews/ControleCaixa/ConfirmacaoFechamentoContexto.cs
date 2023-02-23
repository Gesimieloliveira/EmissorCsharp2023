using System;
using FusionLibrary.VisaoModel;

namespace FusionWPF.SharedViews.ControleCaixa
{
    internal class ConfirmacaoFechamentoContexto : ViewModel
    {
        public decimal ValorConferidoEmCaixa
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorEmDinheiroCalculado
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public event EventHandler ConfirmouOperacao;

        public void ConfirmarFechamento()
        {
            if (ValorConferidoEmCaixa < 0)
            {
                throw new InvalidOperationException("Saldo em caixa não pode ser negativo");
            }

            ConfirmouOperacao?.Invoke(this, EventArgs.Empty);
        }
    }
}