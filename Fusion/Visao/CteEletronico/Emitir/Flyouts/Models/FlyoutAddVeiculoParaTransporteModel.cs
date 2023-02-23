using System;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts.Models
{
    public class RetornoVeiculoParaTransporte
    {
        public FlyoutAddVeiculoParaTransporteModel FlyoutAddVeiculoParaTransporteModel { get; set; }

        public RetornoVeiculoParaTransporte(FlyoutAddVeiculoParaTransporteModel flyoutAddVeiculoParaTransporteModel)
        {
            FlyoutAddVeiculoParaTransporteModel = flyoutAddVeiculoParaTransporteModel;
        }
    }

    public class FlyoutAddVeiculoParaTransporteModel : ModelBase
    {
        private bool _isOpen;
        private string _chassi;
        private string _cor;
        private string _descricaoCor;
        private string _codigoMarcaModelo;
        private decimal _valorUnitario;
        private decimal _freteUnitario;

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

        public string Chassi
        {
            get { return _chassi; }
            set
            {
                if (value == _chassi) return;
                _chassi = value;
                PropriedadeAlterada();
            }
        }

        public string Cor
        {
            get { return _cor; }
            set
            {
                if (value == _cor) return;
                _cor = value;
                PropriedadeAlterada();
            }
        }

        public string DescricaoCor
        {
            get { return _descricaoCor; }
            set
            {
                if (value == _descricaoCor) return;
                _descricaoCor = value;
                PropriedadeAlterada();
            }
        }

        public string CodigoMarcaModelo
        {
            get { return _codigoMarcaModelo; }
            set
            {
                if (value == _codigoMarcaModelo) return;
                _codigoMarcaModelo = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorUnitario
        {
            get { return _valorUnitario; }
            set
            {
                if (value == _valorUnitario) return;
                _valorUnitario = value;
                PropriedadeAlterada();
            }
        }

        public decimal FreteUnitario
        {
            get { return _freteUnitario; }
            set
            {
                if (value == _freteUnitario) return;
                _freteUnitario = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<RetornoVeiculoParaTransporte> AdicionaVeiculoParaTransporte;

        public virtual void OnAdicionaVeiculoParaTransporte()
        {
            Cor = Cor.TrimOrEmpty();
            DescricaoCor = DescricaoCor.TrimOrEmpty();
            CodigoMarcaModelo = CodigoMarcaModelo.TrimOrEmpty();
            Chassi = Chassi.TrimOrEmpty();
            FreteUnitario = FreteUnitario.Format("N2");
            ValorUnitario = ValorUnitario.Format("N2");

            Validacoes();

            AdicionaVeiculoParaTransporte?.Invoke(this, new RetornoVeiculoParaTransporte(this));

            LimaCampos();
        }

        private void Validacoes()
        {
            if (string.IsNullOrEmpty(Cor)) throw new ArgumentException("Adicione cor");
            if (string.IsNullOrEmpty(DescricaoCor)) throw new ArgumentException("Adicione descrição cor");
            if (string.IsNullOrEmpty(CodigoMarcaModelo)) throw new ArgumentException("Adicione Codigo Marca Modelo");
            if (string.IsNullOrEmpty(Chassi)) throw new ArgumentException("Adicione chassi");
            if (Chassi.Length != 17) throw new ArgumentException("Chassi deve ter 17 caracteres");
            if (ValorUnitario == 0 || ValorUnitario < 0)
                throw new ArgumentException("Valor Unitário deve ser maior que 0");
            if (FreteUnitario == 0 || FreteUnitario < 0)
                throw new ArgumentException("Frete Unitário deve ser maior que 0");
        }

        public void LimaCampos()
        {
            Cor = string.Empty;
            DescricaoCor = string.Empty;
            CodigoMarcaModelo = string.Empty;
            Chassi = string.Empty;
            ValorUnitario = 0;
            FreteUnitario = 0;
        }
    }
}