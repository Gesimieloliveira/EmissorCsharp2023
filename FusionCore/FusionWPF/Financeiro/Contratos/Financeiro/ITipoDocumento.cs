using FusionCore.FusionAdm.Financeiro;

namespace FusionCore.FusionWPF.Financeiro.Contratos.Financeiro
{
    public interface ITipoDocumento
    {
        short Id { get; set; }
        string Descricao { get; set; } 
        FFormaPagamento FormaPagamento { get; set; }
        bool RegistraFinanceiro { get; set; }
    }
}