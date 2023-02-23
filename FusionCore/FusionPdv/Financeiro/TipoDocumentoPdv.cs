using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;

namespace FusionCore.FusionPdv.Financeiro
{
    public class TipoDocumentoPdv : ITipoDocumento
    {
        public virtual short Id { get; set; }
        public virtual string Descricao { get; set; }
        public FFormaPagamento FormaPagamento { get; set; }
        public bool RegistraFinanceiro { get; set; }
        public virtual bool EstaAtivo { get; set; } = true;
    }
}