using System;
using System.Windows.Input;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Contratos;

namespace Fusion.Visao.Compras.NotaFiscal.Controls
{
    public class ConfiguracaoRegraCalculoItemModel : ViewModel, IChildContext
    {
        public bool IpiCompoeIcms
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool FreteCompoeIcms
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool SeguroCompoeIcms
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool DespesasCompoeIcms
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool ImpostoManual
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ICommand CommandConfirmar => GetSimpleCommand(ActionConfirmar);
        public string TituloChild => string.Empty;

        private void ActionConfirmar(object obj)
        {
            ConfirmouAlteracoes?.Invoke(this, this);
            SolicitaFechamento?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<ConfiguracaoRegraCalculoItemModel> ConfirmouAlteracoes;
        public event EventHandler SolicitaFechamento;
    }
}