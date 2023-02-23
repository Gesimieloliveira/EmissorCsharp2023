using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;

namespace FusionCore.FusionNfce
{
    public static class ExtTabelaPrecoAdm
    {
        public static TabelaPreco ToAdm(this TabelaPrecoNfce tabelaPrecoNfce)
        {
            if (tabelaPrecoNfce == null) return null;

            return new TabelaPreco
            {
                Id = tabelaPrecoNfce.Id
            };
        }
    }
}