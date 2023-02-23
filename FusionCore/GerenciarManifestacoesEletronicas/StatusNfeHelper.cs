using FusionCore.FusionAdm.Fiscal.NF;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public static class StatusNfeHelper
    {
        public static StatusNfe Converter(byte sitNfe)
        {
            switch (sitNfe)
            {
                case 1: return StatusNfe.Autorizada;
                case 2: return StatusNfe.Denegada;
                default: return StatusNfe.Cancelada;
            }
        }
    }
}