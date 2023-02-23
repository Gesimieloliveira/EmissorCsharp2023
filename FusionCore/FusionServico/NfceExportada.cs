using FusionCore.Repositorio.Base;

namespace FusionCore.FusionServico
{
    public class NfceExportada : Entidade
    {
        public NfceExportada(int nfeId) : this()
        {
            NfceId = nfeId;
        }

        private NfceExportada()
        {
            //nhibernate
        }

        public int NfceId { get; set; }
        protected override int ReferenciaUnica => NfceId;
    }
}