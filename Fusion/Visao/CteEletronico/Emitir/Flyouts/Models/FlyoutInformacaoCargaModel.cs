using System;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts.Models
{
    public class RetornoFlyoutInformacaoCarga : EventArgs
    {
        public FlyoutInformacaoCargaModel FlyoutInformacaoCargaModel { get; set; }

        public RetornoFlyoutInformacaoCarga(FlyoutInformacaoCargaModel flyoutInformacaoCargaModel)
        {
            FlyoutInformacaoCargaModel = flyoutInformacaoCargaModel;
        }
    }

    public class FlyoutInformacaoCargaModel : ModelBase
    {
        private bool _isOpen;
        private UnidadeMedida _unidadeMedida;
        private string _tipoMedida;
        private decimal _quantidade;

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        public UnidadeMedida UnidadeMedida
        {
            get { return _unidadeMedida; }
            set
            {
                if (value == _unidadeMedida) return;
                _unidadeMedida = value;
                PropriedadeAlterada();
            }
        }

        public string TipoMedida
        {
            get { return _tipoMedida; }
            set
            {
                if (value == _tipoMedida) return;
                _tipoMedida = value;
                PropriedadeAlterada();
            }
        }

        public decimal Quantidade
        {
            get { return _quantidade; }
            set
            {
                if (value == _quantidade) return;
                _quantidade = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<RetornoFlyoutInformacaoCarga> AdicionarInformacaoCarga;

        public virtual void OnAdicionarInformacaoCarga()
        {
            TipoMedida = TipoMedida.TrimOrEmpty();
            Quantidade = Quantidade.Format("N4");
            Validacoes();

            AdicionarInformacaoCarga?.Invoke(this, new RetornoFlyoutInformacaoCarga(this));
            LimpaCampos();
        }

        private void Validacoes()
        {
            if (string.IsNullOrEmpty(TipoMedida)) throw new ArgumentException("Adicionar Descrição Tipo Unidade");
            if (Quantidade == 0 || Quantidade < 0) throw new ArgumentException("Quantidade deve ser maior que 0");
        }

        public void LimpaCampos()
        {
            TipoMedida = string.Empty;
            Quantidade = 0;
            UnidadeMedida = UnidadeMedida.M3;
        }
    }
}