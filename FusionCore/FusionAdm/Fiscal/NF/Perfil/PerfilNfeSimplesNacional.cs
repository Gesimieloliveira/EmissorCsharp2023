using FusionCore.Tributacoes.Estadual;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionAdm.Fiscal.NF.Perfil
{
    public sealed class PerfilNfeSimplesNacional
    {
        private PerfilNfe _parent;

        private short Id { get; set; }

        public PerfilNfe Parent
        {
            get => _parent;
            set
            {
                Id = value.Id;
                _parent = value;
            }
        }

        public TributacaoCsosn Csosn { get; set; }
        public decimal AliquotaCredito { get; set; }
    }
}