using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.CteEletronico
{
    public class PerfilCte
    {
        private Ibpt _lazyIbpt = null;

        public short Id { get; set; }
        public EmissorFiscal EmissorFiscal { get; set; }
        public PerfilCfopDTO PerfilCfop { get; set; }
        public string Descricao { get; set; }
        public string NaturezaOperacao { get; set; }
        public TipoCte TipoCte { get; set; }
        public TipoServico TipoServico { get; set; }
        public string Observacao { get; set; }
        public bool Ativo { get; set; }
        public bool RemetentePadrao { get; set; }
        public bool DocumentoPadrao { get; set; }
        public string ProdutoPredominante { get; set; }
        public string CodigoIbpt { get; set; }
        public PerfilCteCarga Carga { get; set; }

        public Ibpt FetchIbpt()
        {
            if (_lazyIbpt != null && _lazyIbpt.Codigo == CodigoIbpt)
            {
                return _lazyIbpt;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioIbpt(sessao);
                _lazyIbpt = repositorio.GetPeloNbs(CodigoIbpt);
            }

            return _lazyIbpt;
        }
    }
}