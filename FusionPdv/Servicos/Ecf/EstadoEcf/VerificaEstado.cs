using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf.EstadoEcf
{
    public abstract class VerificaEstado
    {
        public VerificaEstado VerificaEstadoEcf { get; private set; }
        protected EstadoEcfFiscal Estado;
        protected bool NaoPegaEstado { get; }

        protected VerificaEstado()
        {
            
        }

        protected VerificaEstado(EstadoEcfFiscal estadoEcf)
        {
            Estado = estadoEcf;
            NaoPegaEstado = true;
        }

        public void Verificar()
        {
            PegaEstadoEcf();
            CondicaoDeVerificacao();

            VerificaEstadoEcf?.Verificar();
        }

        protected abstract void CondicaoDeVerificacao();

        protected void PegaEstadoEcf()
        {
            if (NaoPegaEstado) return;
            Estado = SessaoEcf.EcfFiscal.Estado();
        }

        public void Proximo(VerificaEstado verificaEstado)
        {
            VerificaEstadoEcf = verificaEstado;
        }

    }
}
