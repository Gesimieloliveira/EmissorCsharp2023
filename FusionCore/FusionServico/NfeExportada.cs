using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace FusionCore.FusionServico
{
    public class NfeExportada : Entidade
    {
        public NfeExportada(int nfeId) : this()
        {
            NfeId = nfeId;
        }

        private NfeExportada()
        {
            //nhibernate
        }

        public int NfeId { get; set; }
        protected override int ReferenciaUnica => NfeId;
    }
}