using DFe.Utils;
using NFe.Classes;

namespace FusionCore.Helpers.DocumentoXml
{
    public static class XmlNfeHelper
    {
        public static bool TentarAnalisar(string xml, out NFe.Classes.NFe nfe)
        {
            try
            {
                nfe = FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(xml);
                if (nfe != null) return true;
            }
            catch
            {
                // ignore;
            }

            try
            {
                nfe = FuncoesXml.XmlStringParaClasse<nfeProc>(xml).NFe;
                if (nfe != null) return true;
            }
            catch
            {
                // ignore;
            }

            nfe = null;
            return false;
        }
    }
}