using System.Collections.Generic;
using FusionCore.FusionAdm.CteEletronicoOs.Perfil;
using FusionCore.FusionAdm.Emissores;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioPerfilCteOs : Repositorio<PerfilCteOs, int>, IRepositorioPerfilCteOs
    {
        public RepositorioPerfilCteOs(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(PerfilCteOs entidade)
        {
            if (entidade.Id == 0)
            {
                Sessao.Persist(entidade);
                return;
            }

            Sessao.Merge(entidade);
        }

        public IList<PerfilCteOsGrid> BuscarPor(string descricao)
        {
            PerfilCteOs perfilCteOs = null;
            PerfilCteOsGrid retorno = null;

            var queryOver = Sessao.QueryOver(() => perfilCteOs)
                .SelectList(list => list.Select(() => perfilCteOs.Descricao).WithAlias(() => retorno.Descricao)
                .Select(() => perfilCteOs.Id).WithAlias(() => retorno.Id))
                .TransformUsing(Transformers.AliasToBean<PerfilCteOsGrid>());

            if (descricao.IsNotNullOrEmpty())
            {
                var descricaoLike = Restrictions.Like(Projections.Property(() => perfilCteOs.Descricao), descricao.ToUpper(), MatchMode.Anywhere);

                queryOver.Where(descricaoLike);
            }

            var lista = queryOver.OrderBy(() => perfilCteOs.Id).Desc.List<PerfilCteOsGrid>();

            return lista;
        }

        public IList<AbaPerfilCteOsDTO> BuscarAbaPerfilCteOS()
        {
            PerfilCteOs perfilCteOs = null;
            EmissorFiscal emissorFiscal = null;
            EmissorFiscalCTeOS emissorFiscalCteOs = null;
            EmpresaDTO empresa = null;
            PerfilCfopDTO perfilCfop = null;
            AbaPerfilCteOsDTO resultado = null;

            var queryOver = Sessao.QueryOver(() => perfilCteOs)
                .JoinAlias(() => perfilCteOs.EmissorFiscal, () => emissorFiscal)
                .JoinAlias(() => emissorFiscal.Empresa, () => empresa)
                .JoinAlias(() => emissorFiscal.EmissorFiscalCteOs, () => emissorFiscalCteOs)
                .JoinAlias(() => perfilCteOs.PerfilCfop, () => perfilCfop)
                .SelectList(list => list.Select(() => perfilCteOs.Id).WithAlias(() => resultado.Id)
                .Select(() => perfilCteOs.Descricao).WithAlias(() => resultado.Descricao)
                .Select(() => empresa.RazaoSocial).WithAlias(() => resultado.RazaoSocial)
                .Select(() => empresa.Cnpj).WithAlias(() => resultado.CnpjEmpresa)
                .Select(() => perfilCfop.Codigo).WithAlias(() => resultado.PerfilCfopCodigo)
                .Select(() => perfilCteOs.TipoCte).WithAlias(() => resultado.TipoCte)
                .Select(() => perfilCteOs.TipoServico).WithAlias(() => resultado.TipoServico)
                .Select(() => emissorFiscalCteOs.Ambiente).WithAlias(() => resultado.AmbienteSefaz));

            queryOver.TransformUsing(Transformers.AliasToBean<AbaPerfilCteOsDTO>());

            var lista = queryOver.OrderByAlias(() => perfilCteOs.Id).Desc.List<AbaPerfilCteOsDTO>();

            return lista;
        }

        PerfilCteOs IRepositorio<PerfilCteOs, int>.GetPeloId(int id)
        {
            var perfilCteOs = Sessao.Get<PerfilCteOs>(id);

            if (perfilCteOs.Tomador == null) return perfilCteOs;

            NHibernateUtil.Initialize(perfilCteOs.Tomador.Enderecos);
            NHibernateUtil.Initialize(perfilCteOs.Tomador.Telefones);
            NHibernateUtil.Initialize(perfilCteOs.Tomador.Emails);

            return perfilCteOs;
        }
    }
}