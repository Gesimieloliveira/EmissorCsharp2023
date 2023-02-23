using MDFe.Classes.Retorno.MDFeConsultaProtocolo;

namespace Fusion.Visao.MdfeEletronico
{
    public static class ExtMDFEletronicoZeus
    {
        public static bool IsAutorizado(this MDFeRetConsSitMDFe retorno)
        {
            return retorno != null && retorno.CStat == 100;
        }

        public static bool IsRejeicao999(this MDFeRetConsSitMDFe retorno)
        {
            return retorno != null && retorno.CStat == 999;
        }

        public static bool IsRejeicao(this MDFeRetConsSitMDFe retorno)
        {
            return retorno != null && retorno.CStat != 100;
        }

        public static bool IsTemNfeCancelada(this MDFeRetConsSitMDFe retorno)
        {
            return retorno != null && retorno.CStat == 677;
        }
    }
}