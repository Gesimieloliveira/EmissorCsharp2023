using FusionCore.FusionAdm.Pessoas;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteRecebedor
    {
        public int CteId { get; set; }
        public Cte Cte { get; set; }
        public PessoaEntidade Recebedor { get; set; }

        private CteRecebedor()
        {
            //nhibernate
        }

        public static CteRecebedor Cria(Cte cte, PessoaEntidade recebedor, int idRecebedor)
        {
            var cteRecebedor = new CteRecebedor
            {
                CteId = idRecebedor,
                Cte = cte,
                Recebedor = recebedor
            };

            return cteRecebedor;
        }
    }
}