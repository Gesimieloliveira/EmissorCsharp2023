using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FusionCore.FusionAdm.Fiscal.NF.Perfil
{
    public class PerfilNfeTransportadora : Entidade
    {
        private PerfilNfe _parent;
        private short Id { get; set; }
        protected override int ReferenciaUnica => Id;

        public PerfilNfe Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                Id = _parent.Id;
            }
        }

        public Transportadora Transportadora { get; set; }
        public Veiculo Veiculo { get; set; }

        private PerfilNfeTransportadora()
        {
            //nhibernate
        }

        public PerfilNfeTransportadora(PerfilNfe parent, Transportadora transportadora, Veiculo veiculo) : this()
        {
            Parent = parent;
            Transportadora = transportadora;
            Veiculo = veiculo;
        }
    }
}