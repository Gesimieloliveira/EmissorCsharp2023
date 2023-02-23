using System;
using FusionCore.FusionAdm.Financeiro;

namespace FusionCore.FusionWPF.Financeiro.Contratos.Financeiro.Crediarios
{
    public interface IDocumentoReceberModel
    {
        event EventHandler AlterarPrimeiraParcela;
        byte Parcela { get; set; } 
        DateTime? Vencimento { get; set; }
        int Dias { get; set; }
        decimal ValorAjustado { get; set; }
        ITipoDocumento TipoDocumento { get; set; }
        string Descricao { get; set; }
        bool Editar { get; set; }
        CentroLucro CentroLucro { get; set; }
    }
}