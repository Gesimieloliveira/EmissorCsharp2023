using FusionCore.FusionAdm.Pessoas;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteExpedidor
    {
        public int CteId { get; set; }
        public Cte Cte { get; set; }
        public PessoaEntidade Expedidor { get; set; }

        private CteExpedidor()
        {
            //nhibernate
        }

        public static CteExpedidor Cria(Cte cte, PessoaEntidade expedidor, int idExpedidor)
        {
            var cteExpedidor = new CteExpedidor
            {
                CteId = idExpedidor,
                Cte = cte,
                Expedidor = expedidor
            };

            return cteExpedidor;
        }
    }
}