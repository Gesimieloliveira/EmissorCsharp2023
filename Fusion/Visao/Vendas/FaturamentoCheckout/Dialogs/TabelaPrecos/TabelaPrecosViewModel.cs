using System;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.TabelaPrecos
{
    public sealed class TabelaPrecosViewModel : ViewModel
    {
        public event EventHandler<ITabelaPreco> Confirmou;

        public ITabelaPreco TabelaSelecionada
        {
            get => GetValue<ITabelaPreco>();
            set => SetValue(value);
        }

        public void ConfirmarEscolha()
        {
            Confirmou?.Invoke(this, TabelaSelecionada);
        }
    }
}