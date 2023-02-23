using FusionCore.FusionAdm.MdfeEletronico.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico.Extencoes
{
    public static class ExtMdfeResponsavelSeguro
    {
        public static bool IsContratante(this MDFeResponsavelSeguro seguro)
        {
            return seguro == MDFeResponsavelSeguro.ContratanteServicoTransporte;
        }

        public static bool IsEmitente(this MDFeResponsavelSeguro seguro)
        {
            return seguro == MDFeResponsavelSeguro.Emitente;
        }
    }
}