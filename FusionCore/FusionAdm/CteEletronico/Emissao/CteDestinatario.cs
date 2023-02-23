using FusionCore.FusionAdm.Pessoas;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteDestinatario
    {
        public int CteId { get; set; }
        public Cte Cte { get; set; }
        public PessoaEntidade Destinatario { get; set; }

        public static CteDestinatario Cria(Cte cte, PessoaEntidade destinatario)
        {
            var cteDestinatario = new CteDestinatario
            {
                CteId = cte.Id,
                Cte = cte,
                Destinatario = destinatario
            };

            return cteDestinatario;
        }
    }
}