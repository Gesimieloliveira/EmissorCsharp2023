using FusionCore.FusionAdm.Pessoas;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteTomador
    {
        public int CteId { get; set; }
        public Cte Cte { get; set; }
        public PessoaEntidade Tomador { get; set; }

        public static CteTomador Cria(Cte cte, PessoaEntidade tomador)
        {
            var cteTomador = new CteTomador {
                CteId = cte.Id,
                Cte = cte,
                Tomador = tomador};
            return cteTomador;
        }
    }
}