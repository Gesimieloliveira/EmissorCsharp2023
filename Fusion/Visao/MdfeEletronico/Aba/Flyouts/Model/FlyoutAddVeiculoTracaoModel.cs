using System;
using System.Windows.Input;
using Fusion.Visao.Veiculos;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model
{
    public class SalvarVeiculoTracaoEventArgs : EventArgs
    {
        public SalvarVeiculoTracaoEventArgs(FlyoutAddVeiculoTracaoModel model)
        {
            Model = model;
        }

        public FlyoutAddVeiculoTracaoModel Model { get; set; }
    }

    public class FlyoutAddVeiculoTracaoModel : ViewModel
    {
        private bool _isOpen;
        private Veiculo _veiculo;
        private string _descricao;
        private string _codigoInterno;
        private string _renavam;
        private string _placa;
        private int _taraKg;
        private int _capacidadeEmKg;
        private short _capacidadeEmM3;
        private TipoRodado _tipoRodado;
        private TipoCarroceria _tipoCarroceria;
        private TipoVeiculo _tipoVeiculo;
        private TipoPropriedadeVeiculo _tipoPropriedadeVeiculo;
        private string _siglaUf;
        private string _nomeProprietario;
        public ICommand CommandBuscarVeiculo => GetSimpleCommand(BuscarVeiculo);

        public event EventHandler<SalvarVeiculoTracaoEventArgs> SalvarVeiculoTracaoHandler; 

        public Veiculo Veiculo
        {
            get { return _veiculo; }
            set
            {
                _veiculo = value;
                PropriedadeAlterada();
                AtualizaModel();
            }
        }

        public string SiglaUf
        {
            get { return _siglaUf; }
            set
            {
                if (value == _siglaUf) return;
                _siglaUf = value;
                PropriedadeAlterada();
            }
        }

        public TipoPropriedadeVeiculo TipoPropriedadeVeiculo
        {
            get { return _tipoPropriedadeVeiculo; }
            set
            {
                if (value == _tipoPropriedadeVeiculo) return;
                _tipoPropriedadeVeiculo = value;
                PropriedadeAlterada();
            }
        }

        public string NomeProprietario
        {
            get { return _nomeProprietario; }
            set
            {
                if (value == _nomeProprietario) return;
                _nomeProprietario = value;
                PropriedadeAlterada();
            }
        }

        public TipoVeiculo TipoVeiculo
        {
            get { return _tipoVeiculo; }
            set
            {
                if (value == _tipoVeiculo) return;
                _tipoVeiculo = value;
                PropriedadeAlterada();
            }
        }

        public TipoCarroceria TipoCarroceria
        {
            get { return _tipoCarroceria; }
            set
            {
                if (value == _tipoCarroceria) return;
                _tipoCarroceria = value;
                PropriedadeAlterada();
            }
        }

        public TipoRodado TipoRodado
        {
            get { return _tipoRodado; }
            set
            {
                if (value == _tipoRodado) return;
                _tipoRodado = value;
                PropriedadeAlterada();
            }
        }

        public short CapacidadeEmM3
        {
            get { return _capacidadeEmM3; }
            set
            {
                if (value == _capacidadeEmM3) return;
                _capacidadeEmM3 = value;
                PropriedadeAlterada();
            }
        }

        public int CapacidadeEmKg
        {
            get { return _capacidadeEmKg; }
            set
            {
                if (value == _capacidadeEmKg) return;
                _capacidadeEmKg = value;
                PropriedadeAlterada();
            }
        }

        public int TaraKg
        {
            get { return _taraKg; }
            set
            {
                if (value == _taraKg) return;
                _taraKg = value;
                PropriedadeAlterada();
            }
        }

        public string Placa
        {
            get { return _placa; }
            set
            {
                if (value == _placa) return;
                _placa = value;
                PropriedadeAlterada();
            }
        }

        public string Renavam
        {
            get { return _renavam; }
            set
            {
                if (value == _renavam) return;
                _renavam = value;
                PropriedadeAlterada();
            }
        }

        public string CodigoInterno
        {
            get { return _codigoInterno; }
            set
            {
                if (value == _codigoInterno) return;
                _codigoInterno = value;
                PropriedadeAlterada();
            }
        }

        public string Descricao
        {
            get { return _descricao; }
            set
            {
                if (value == _descricao) return;
                _descricao = value;
                PropriedadeAlterada();
            }
        }

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

        private void BuscarVeiculo(object obj)
        {
            var pickerModel = new VeiculoPickerModel();
            pickerModel.PickItemEvent += VeiculoSelecionadoCompleted;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void VeiculoSelecionadoCompleted(object sender, GridPickerEventArgs e)
        {
            Veiculo = e.GetItem<Veiculo>();
        }

        private void AtualizaModel()
        {
            Descricao = Veiculo.Descricao;
            CodigoInterno = Veiculo.Id.ToString();
            Renavam = Veiculo.Renavam;
            Placa = Veiculo.Placa;
            TaraKg = Veiculo.TaraEmKg;
            CapacidadeEmKg = Veiculo.CapacidadeEmKg;
            CapacidadeEmM3 = Veiculo.CapacidadeEmM3;
            TipoRodado = Veiculo.TipoRodado;
            TipoCarroceria = Veiculo.TipoCarroceria;
            TipoVeiculo = Veiculo.TipoVeiculo;
            TipoPropriedadeVeiculo = Veiculo.TipoProprietario;
            SiglaUf = Veiculo.SiglaUf;

            var proprietario = Veiculo.CarregaProprietario();
            NomeProprietario = proprietario?.Nome;
        }

        public void LimpaCampos()
        {
            CodigoInterno = string.Empty;
            Renavam = string.Empty;
            Placa = string.Empty;
            TaraKg = 0;
            CapacidadeEmKg = 0;
            CapacidadeEmM3 = 0;
            TipoPropriedadeVeiculo = TipoPropriedadeVeiculo.Proprio;
            TipoVeiculo = TipoVeiculo.Tracao;
            TipoRodado = TipoRodado.NaoAplicavel;
            TipoCarroceria = TipoCarroceria.NaoAplicavel;
            SiglaUf = string.Empty;
            NomeProprietario = string.Empty;
            Descricao = string.Empty;
        }

        public void SalvarVeiculoTracao()
        {
            Valida();
            OnSalvarVeiculoTracaoHandler();
        }

        private void Valida()
        {
            if (Veiculo == null) throw new ArgumentException("Adicionar um veículo");
            if (Veiculo.TipoRodado == TipoRodado.NaoAplicavel) throw new ArgumentException("Tipo rodado do veículo é inválido");

            if (Veiculo.TipoProprietario != TipoPropriedadeVeiculo.Terceiro)
            {
                return;
            }

            ValidadorProprietario.Checa(Veiculo.CarregaProprietario());
        }

        protected virtual void OnSalvarVeiculoTracaoHandler()
        {
            SalvarVeiculoTracaoHandler?.Invoke(this, new SalvarVeiculoTracaoEventArgs(this));
        }
    }
}