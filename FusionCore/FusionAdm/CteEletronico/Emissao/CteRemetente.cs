using FusionCore.FusionAdm.Pessoas;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteRemetente
    {
        public int CteId { get; set; }
        public Cte Cte { get; set; }
        public PessoaEntidade Remetente { get; set; }

        public static CteRemetente Cria(Cte cte, PessoaEntidade remetente)
        {
            var cteRemetente = new CteRemetente
            {
                CteId = cte.Id,
                Cte = cte,
                Remetente = remetente
            };

            return cteRemetente;
        }
    }
}