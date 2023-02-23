using FusionCore.SelecionarNfce;
using System.Collections.Generic;

namespace FusionCore.FusionAdm.ConverterVendaParaNfe
{
    public interface IRepositorioConverterNfe
    {
        IList<NfceDto> BuscaNfceParaConversao(FiltroConversorNfce filtroConversorNfce);
    }
}