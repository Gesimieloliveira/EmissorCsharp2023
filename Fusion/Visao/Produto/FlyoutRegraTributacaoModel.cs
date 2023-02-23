using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Produto
{
    public sealed class FlyoutRegraTributacaoModel : ViewModel
    {
        public ICollection<EstadoDTO> Estados { get; } = LocalidadesServico.GetInstancia().GetEstados();
        
        public bool IsNew
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool IsOpen
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public RegraTribuacaoInfo Regra
        {
            get { return GetValue<RegraTribuacaoInfo>(); }
            set { SetValue(value); }
        }

        public FlyoutRegraTributacaoModel()
        {
            IsNew = true;
            Regra = new RegraTribuacaoInfo();
        }

        public FlyoutRegraTributacaoModel(RegraTribuacaoInfo regra)
        {
            Regra = regra;
        }

        public event EventHandler<RegraTribuacaoInfo> RegraAdicionada;
        public event EventHandler<RegraTribuacaoInfo> RegraAlterada;
        public event EventHandler<RegraTribuacaoInfo> RegraDeletada;

        public void DeletarRegra()
        {
            RegraDeletada?.Invoke(this, Regra);
        }

        public void SalvarRegra()
        {
            try
            {
                if (Regra.Uf == null)
                    throw new InvalidOperationException("Preciso que informe a UF da regra");

                if (Regra.Aliquota <= 0)
                    throw new InvalidOperationException("Preciso que informe uma alíquota válida");

                if (IsNew) RegraAdicionada?.Invoke(this, Regra);
                else RegraAlterada?.Invoke(this, Regra);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }
    }
}