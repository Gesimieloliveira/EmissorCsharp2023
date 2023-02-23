using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;

namespace FusionCore.FusionWPF.Financeiro.Contratos.Financeiro.Crediarios
{
    public interface IParcelamento
    {
        CentroLucro CentroLucro { get; }
        ITipoDocumento TipoDocumento { get; }
        string Descricao { get; }
        decimal ValorEntrada { get; }
        CentroCusto CentroCusto { get; set; }
        IList<IDocumentoReceberModel> Parcelas { get; }
    }
}
