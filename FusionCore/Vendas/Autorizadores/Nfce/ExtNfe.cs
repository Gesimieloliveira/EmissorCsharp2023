namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public static class ExtNfe
    {
        public static string ObterChaveNfeZeus(this NFe.Classes.NFe nfe)
        {
            return nfe.infNFe.Id.Substring(3, 44);
        }
    }
}