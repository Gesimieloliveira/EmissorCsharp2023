using System.Collections.Generic;
using FusionCore.FusionAdm.CteEletronico;
using FusionCore.FusionAdm.Emissores;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioPerfilCte : Repositorio<PerfilCte, short>, IRepositorioPerfilCte
    {
        public RepositorioPerfilCte(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(PerfilCte perfilCte)
        {
            Sessao.SaveOrUpdate(perfilCte);
        }

        public void Deletar(PerfilCte perfilCte)
        {
            Sessao.Delete(perfilCte);
        }

        public IList<PerfilCteGrid> BuscaTodosParaGrid()
        {
            PerfilCteGrid resultado = null;
            PerfilCte perfilCteAlias = null;

            var queryOver = Sessao.QueryOver(() => perfilCteAlias)
                .SelectList(
                    list => list.Select(() => perfilCteAlias.Descricao).WithAlias(() => resultado.Descricao)
                    .Select(() => perfilCteAlias.Id).WithAlias(() => resultado.Id)
                    .Select(() => perfilCteAlias.TipoServico).WithAlias(() => resultado.TipoServico)
                    .Select(() => perfilCteAlias.TipoCte).WithAlias(() => resultado.TipoCte)
                );

            queryOver.TransformUsing(Transformers.AliasToBean<PerfilCteGrid>());


            var lista = queryOver.List<PerfilCteGrid>();

            return lista;
        }

        public IList<PerfilCteListBoxDTO> BuscaTodosParaListBoxEmissao()
        {
            PerfilCteListBoxDTO resultado = null;
            PerfilCte perfilCteAlias = null;
            EmissorFiscal emissorFiscalAlias = null;
            EmissorFiscalCTE emissorFiscalCteAlias = null;
            EmpresaDTO empresaAlias = null;
            PerfilCfopDTO perfilCfopAlias = null;

            var queryOver = Sessao.QueryOver(() => perfilCteAlias)
                .Inner.JoinAlias(() => perfilCteAlias.EmissorFiscal, () => emissorFiscalAlias)
                .Inner.JoinAlias(() => emissorFiscalAlias.EmissorFiscalCte, () => emissorFiscalCteAlias)
                .Inner.JoinAlias(() => emissorFiscalAlias.Empresa, () => empresaAlias)
                .Inner.JoinAlias(() => perfilCteAlias.PerfilCfop, () => perfilCfopAlias)
                .SelectList(
                    list => list.Select(() => perfilCteAlias.Descricao).WithAlias(() => resultado.Descricao)
                    .Select(() => perfilCteAlias.Id).WithAlias(() => resultado.Id)
                    .Select(() => perfilCteAlias.TipoServico).WithAlias(() => resultado.TipoServico)
                    .Select(() => perfilCteAlias.TipoCte).WithAlias(() => resultado.TipoCte)
                    .Select(() => emissorFiscalCteAlias.Ambiente).WithAlias(() => resultado.AmbienteSefaz)
                    .Select(() => empresaAlias.RazaoSocial).WithAlias(() => resultado.RazaoSocialEmpresa)
                    .Select(() => empresaAlias.Cnpj).WithAlias(() => resultado.CnpjEmpresa)
                    .Select(() => perfilCfopAlias.Codigo).WithAlias(() => resultado.PerfilCfopCodigo)
                );

            queryOver.TransformUsing(Transformers.AliasToBean<PerfilCteListBoxDTO>());

            var lista = queryOver.List<PerfilCteListBoxDTO>();

            return lista;
        }
    }
}