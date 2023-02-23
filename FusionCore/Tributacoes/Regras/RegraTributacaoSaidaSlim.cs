using FusionCore.Comum;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace FusionCore.Tributacoes.Regras
{
    public class RegraTributacaoSaidaSlim : Comparavel<short>
    {
        private RegraTributacaoSaida _regraTributacao;
        protected override short ChaveUnica => Id;
        public short Id { get; set; }
        public bool Ativo { get; set; }
        public string AtivoDescricao => Ativo ? "Ativo" : "Inativo";
        public string Descricao { get; set; }
        public string Cst { get; set; }
        public string Csosn { get; set; }
        public string CfopIntermunicipal { get; set; }
        public string CfopInterestadual { get; set; }
        public string CfopExterior { get; set; }
        public string CfopNfce { get; set; }

        public static RegraTributacaoSaidaSlim From(RegraTributacaoSaida regra)
        {
            if (regra == null)
            {
                return null;
            }

            return new RegraTributacaoSaidaSlim
            {
                Id = regra.Id,
                Ativo = regra.Ativo,
                Descricao = regra.Descricao,
                Cst = regra.Cst.Codigo,
                Csosn = regra.Csosn.Codigo,
                CfopIntermunicipal = regra.CfopIntermunicipal.Cfop.Id,
                CfopInterestadual = regra.CfopInterestadual.Cfop.Id,
                CfopExterior = regra.CfopExterior?.Cfop.Id,
                CfopNfce = regra.CfopNfce.Id,
                _regraTributacao = regra
            };
        }

        public static IQueryOver<RegraTributacaoSaida, RegraTributacaoSaida> CriaQueryOver(ISession session)
        {
            RegraTributacaoSaida tbRegra = null;
            PerfilCfopDTO tbCfopMunicipal = null;
            PerfilCfopDTO tbCfopEstadual = null;
            PerfilCfopDTO tbCfopExterior = null;
            RegraTributacaoSaidaSlim slim = null;

            var query = session.QueryOver(() => tbRegra)
                .JoinAlias(() => tbRegra.CfopIntermunicipal, () => tbCfopMunicipal, JoinType.InnerJoin)
                .JoinAlias(() => tbRegra.CfopInterestadual, () => tbCfopEstadual, JoinType.InnerJoin)
                .JoinAlias(() => tbRegra.CfopExterior, () => tbCfopExterior, JoinType.LeftOuterJoin)
                .SelectList(list => list
                    .Select(() => tbRegra.Id).WithAlias(() => slim.Id)
                    .Select(() => tbRegra.Ativo).WithAlias(() => slim.Ativo)
                    .Select(() => tbRegra.Descricao).WithAlias(() => slim.Descricao)
                    .Select(() => tbRegra.Cst.Codigo).WithAlias(() => slim.Cst)
                    .Select(() => tbRegra.Csosn.Codigo).WithAlias(() => slim.Csosn)
                    .Select(() => tbCfopMunicipal.Cfop.Id).WithAlias(() => slim.CfopIntermunicipal)
                    .Select(() => tbCfopEstadual.Cfop.Id).WithAlias(() => slim.CfopInterestadual)
                    .Select(() => tbCfopExterior.Cfop.Id).WithAlias(() => slim.CfopExterior)
                    .Select(() => tbRegra.CfopNfce.Id).WithAlias(() => slim.CfopNfce)
                );

            query.TransformUsing(Transformers.AliasToBean<RegraTributacaoSaidaSlim>());

            return query;
        }

        public RegraTributacaoSaida GetRegra(ISessaoManager manager)
        {
            if (_regraTributacao != null)
            {
                return _regraTributacao;
            }

            using (var sessao = manager.CriaSessao())
            {
                _regraTributacao = new RepositorioRegraTributacao(sessao).GetPeloId(Id);
            }

            return _regraTributacao;
        }
    }
}