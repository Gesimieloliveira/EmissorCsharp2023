using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.Fiscal.NF.Perfil
{
    public class PerfilNfeDestinatario : Entidade
    {
        private PerfilNfe _parent;
        private short Id { get; set; }
        protected override int ReferenciaUnica => Id;
        public Cliente Destinatario { get; set; }

        public PerfilNfe Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                Id = value.Id;
            }
        }

        private PerfilNfeDestinatario()
        {
            //nhibernate
        }

        public PerfilNfeDestinatario(PerfilNfe parent, Cliente destinatario) : this()
        {
            Parent = parent;
            Destinatario = destinatario;
        }
    }
}