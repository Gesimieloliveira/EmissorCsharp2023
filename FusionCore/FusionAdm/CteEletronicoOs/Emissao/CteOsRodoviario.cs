using FusionCore.Repositorio.Base;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsRodoviario : Entidade
    {
        private CteOsRodoviario()
        {
            //nhibernate
            Taf = string.Empty;
            NumeroDoRegimeEstadual = string.Empty;
        }

        public CteOsRodoviario(CteOs cte) : this()
        {
            CteOs = cte;
        }

        public int CteOsId { get; set; }
        public CteOs CteOs { get; private set; }
        public string Taf { get; set; }
        public string NumeroDoRegimeEstadual { get; set; }

        protected override int ReferenciaUnica => CteOsId;
    }
}