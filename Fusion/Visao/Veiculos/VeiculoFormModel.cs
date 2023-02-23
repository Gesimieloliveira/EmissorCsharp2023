using System;
using System.Collections.ObjectModel;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

// ReSharper disable MemberCanBePrivate.Global

namespace Fusion.Visao.Veiculos
{
    public sealed class VeiculoFormModel : ViewModel
    {
        private Veiculo _veiculo;
        private TipoPropriedadeVeiculo _tipoProprietario;
        private TipoRodado _tipoRodado;
        private TipoCarroceria _tipoCarroceria;
        private string _renavam;
        private int _taraEmKg;
        private int _capacidadeEmKg;
        private short _capacidadeEmM3;
        private TipoVeiculo _tipoVeiculo;

        public VeiculoFormModel(Veiculo veiculo = null)
        {
            _veiculo = veiculo;
        }

        public ProprietarioVeiculo Proprietario
        {
            get => GetValue<ProprietarioVeiculo>();
            set => SetValue(value);
        }

        public string Descricao
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Placa
        {
            get => GetValue();
            set => SetValue(value);
        }

        public bool IsPrincipal
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string SiglaEstado
        {
            get => GetValue();
            set => SetValue(value);
        }

        public TipoPropriedadeVeiculo TipoProprietario
        {
            get => _tipoProprietario;
            set
            {
                _tipoProprietario = value;
                PropriedadeAlterada();
            }
        }

        public TipoRodado TipoRodado
        {
            get => _tipoRodado;
            set
            {
                if (value == _tipoRodado) return;
                _tipoRodado = value;
                PropriedadeAlterada();
            }
        }

        public TipoCarroceria TipoCarroceria
        {
            get => _tipoCarroceria;
            set
            {
                if (value == _tipoCarroceria) return;
                _tipoCarroceria = value;
                PropriedadeAlterada();
            }
        }

        public string Renavam
        {
            get => _renavam;
            set
            {
                if (value == _renavam) return;
                _renavam = value;
                PropriedadeAlterada();
            }
        }

        public int TaraEmKg
        {
            get => _taraEmKg;
            set
            {
                if (value == _taraEmKg) return;
                _taraEmKg = value;
                PropriedadeAlterada();
            }
        }

        public int CapacidadeEmKg
        {
            get => _capacidadeEmKg;
            set
            {
                if (value == _capacidadeEmKg) return;
                _capacidadeEmKg = value;
                PropriedadeAlterada();
            }
        }

        public short CapacidadeEmM3
        {
            get => _capacidadeEmM3;
            set
            {
                if (value == _capacidadeEmM3) return;
                _capacidadeEmM3 = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EstadoDTO> Estados { get; } = new ObservableCollection<EstadoDTO>();

        public TipoVeiculo TipoVeiculo
        {
            get => _tipoVeiculo;
            set
            {
                if (value == _tipoVeiculo) return;
                _tipoVeiculo = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler OperacaoFinalizada;
        public event EventHandler<Veiculo> RegistroSalvo;

        private void OnOperacaoFinalizada()
        {
            OperacaoFinalizada?.Invoke(this, EventArgs.Empty);
        }

        public void CarregarDados()
        {
            CarregarEstados();
            CarregarVeiculo();
        }

        private void CarregarEstados()
        {
            Estados.Clear();

            var estados = LocalidadesServico.GetInstancia().GetEstados();
            estados?.ForEach(Estados.Add);
        }

        private void CarregarVeiculo()
        {
            Descricao = _veiculo?.Descricao;
            Placa = _veiculo?.Placa;
            SiglaEstado = _veiculo?.SiglaUf;
            IsPrincipal = _veiculo?.IsPrincipal ?? true;
            CapacidadeEmKg = _veiculo?.CapacidadeEmKg ?? 0;
            CapacidadeEmM3 = _veiculo?.CapacidadeEmM3 ?? 0;
            Renavam = _veiculo?.Renavam ?? string.Empty;
            TaraEmKg = _veiculo?.TaraEmKg ?? 0;
            TipoCarroceria = _veiculo?.TipoCarroceria ?? TipoCarroceria.Abera;
            TipoProprietario = _veiculo?.TipoProprietario ?? TipoPropriedadeVeiculo.Proprio;
            TipoRodado = _veiculo?.TipoRodado ?? TipoRodado.NaoAplicavel;
            TipoVeiculo = _veiculo?.TipoVeiculo ?? TipoVeiculo.Tracao;
            Proprietario = _veiculo?.CarregaProprietario();
        }

        public void ConfirmaVeiculo()
        {
            try
            {
                EliminaEspacosDesnecessariosNasString();
                ThrowExceptionSeDadosInformadoForInvalido();

                _veiculo.Descricao = Descricao.TrimOrEmpty();
                _veiculo.Placa = Placa.TrimOrEmpty();
                _veiculo.SiglaUf = SiglaEstado.TrimOrEmpty();
                _veiculo.IsPrincipal = IsPrincipal;
                _veiculo.CapacidadeEmKg = CapacidadeEmKg;
                _veiculo.CapacidadeEmM3 = CapacidadeEmM3;
                _veiculo.Renavam = Renavam.TrimOrEmpty();
                _veiculo.TaraEmKg = TaraEmKg;
                _veiculo.TipoCarroceria = TipoCarroceria;
                _veiculo.TipoRodado = TipoRodado;
                _veiculo.TipoVeiculo = TipoVeiculo;
                _veiculo.TipoProprietario = TipoProprietario;
                _veiculo.TransportadoraId = Proprietario?.Id;

                SalvarOuAtualizar(_veiculo);

                OnRegistroSalvo(_veiculo);
                OnOperacaoFinalizada();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void ThrowExceptionSeDadosInformadoForInvalido()
        {
            if (TipoProprietario == TipoPropriedadeVeiculo.Terceiro && Proprietario == null)
            {
                throw new ArgumentException("Quando veiculo for de terceiro é preciso informar um proprietário");
            }

            if (TipoProprietario == TipoPropriedadeVeiculo.Proprio && Proprietario != null)
            {
                throw new ArgumentException("Veículo próprio não deve ter proprietário vinculado");
            }

            if (Renavam.IsNotNullOrEmpty() && Renavam.Length < 9)
            {
                throw new ArgumentException("Renavam precisa ter entre 9 a 11 digitos");
            }

            if (Placa.Contains(" "))
            {
                throw new ArgumentException("A placa do veiculo não pode ter espaço");
            }

            if (_veiculo == null)
            {
                _veiculo = new Veiculo(Descricao, Placa, SiglaEstado);
            }
        }

        private void EliminaEspacosDesnecessariosNasString()
        {
            Descricao = Descricao.TrimOrEmpty();
            Placa = Placa.TrimOrEmpty();
            SiglaEstado.TrimOrEmpty();
            Renavam = Renavam.TrimOrEmpty();
        }

        private static void SalvarOuAtualizar(Veiculo veiculo)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var reposiotiro = new RepositorioVeiculo(sessao);
                reposiotiro.SalvarOuAtualizar(veiculo);
                transacao.Commit();
            }
        }

        private void OnRegistroSalvo(Veiculo e)
        {
            RegistroSalvo?.Invoke(this, e);
        }
    }
}