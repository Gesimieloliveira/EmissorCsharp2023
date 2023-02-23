namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public static class TipoDfeHelper
    {
        public static TipoDfe IdentificarTipo(string xmlString)
        {
            if (xmlString.StartsWith("<resNFe"))
            {
                return TipoDfe.ResumoNfe;
            }

            if (xmlString.StartsWith("<procEventoNFe"))
            {
                return TipoDfe.ProcEventoNfe;
            }

            if (xmlString.StartsWith("<resEvento"))
            {
                return TipoDfe.ResEvento;
            }

            if (xmlString.StartsWith("<nfeProc"))
            {
                return TipoDfe.NfeProc;
            }

            return TipoDfe.Outros;
        }
    }
}