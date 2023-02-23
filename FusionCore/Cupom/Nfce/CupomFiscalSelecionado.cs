using FusionCore.Vendas.Faturamentos;

namespace FusionCore.Cupom.Nfce
{
    public class CupomFiscalSelecionado
    {
        public CupomFiscalSelecionado(int id, bool isFaturamento, CupomSituacao cupomSituacao, string nomeEmpresa, int vendaId)
        {
            Id = id;
            IsFaturamento = isFaturamento;
            CupomSituacao = cupomSituacao;
            NomeEmpresa = nomeEmpresa;
            VendaId = vendaId;
        }

        public int Id { get; }
        public bool IsFaturamento { get; }
        public CupomSituacao CupomSituacao { get; }
        public string NomeEmpresa { get; }
        public int VendaId { get; set; }
    }
}