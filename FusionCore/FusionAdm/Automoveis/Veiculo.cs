using System;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.FusionAdm;

// ReSharper disable ConvertToAutoProperty
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.Automoveis
{
    public class Veiculo : Entidade
    {
        private string _descricao;
        private string _siglaUf;
        private string _placa;
        private string _renavam;
        private int _taraEmKg;
        private int _capacidadeEmKg;
        private short _capacidadeEmM3;
        private TipoPropriedadeVeiculo _tipoProprietario;

        private Veiculo()
        {
            IsPrincipal = true;
        }

        public Veiculo(string descricao, string placa, string siglaUf) : this()
        {
            _descricao = descricao;
            _placa = placa;
            _siglaUf = siglaUf;
        }

        protected override int ReferenciaUnica => Id;
        public int Id { get; private set; }
        public int? TransportadoraId { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nome veículo da transportadora é inválido");

                _descricao = value;
            }
        }

        public string SiglaUf
        {
            get => _siglaUf;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Sigla UF do veículo da transportadora é inválido");

                _siglaUf = value;
            }
        }

        public string Placa
        {
            get => _placa;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Placa do veículo da transportadora é inválido");

                if (value.Length != 7)
                    throw new ArgumentException("Placa do veículo da transportadora deve ter 7 caracteres");

                _placa = value;
            }
        }

        public bool IsPrincipal { get; set; }

        public TipoPropriedadeVeiculo TipoProprietario
        {
            get => _tipoProprietario;
            set => _tipoProprietario = value;
        }

        public TipoVeiculo TipoVeiculo { get; set; }
        public TipoRodado TipoRodado { get; set; }
        public TipoCarroceria TipoCarroceria { get; set; }

        public string Renavam
        {
            get => _renavam;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Renavam veículo da transportadora é inválido");
                _renavam = value.Trim();
            }
        }

        public int TaraEmKg
        {
            get => _taraEmKg;
            set => _taraEmKg = value;
        }

        public int CapacidadeEmKg
        {
            get => _capacidadeEmKg;
            set => _capacidadeEmKg = value;
        }

        public short CapacidadeEmM3
        {
            get => _capacidadeEmM3;
            set => _capacidadeEmM3 = value;
        }

        public ProprietarioVeiculo CarregaProprietario()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repostorio = new RepositorioVeiculo(sessao);
                return repostorio.GetProprietario(this);
            }
        }

        public Transportadora CarregaTransPortadora()
        {
            if (TransportadoraId == null)
            {
                throw new InvalidOperationException("Não tem proprietário vinculado");
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repostorio = new RepositorioPessoa(sessao);
                return repostorio.GetTransportadoraPeloId(TransportadoraId.Value);
            }
        }
    }
}