using System.Collections.Generic;
using System.Linq;
using FusionCore.CadastroEmpresa;
using FusionCore.Extencoes;
using FusionCore.GerenciarManifestacoesEletronicas;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioEmpresa : Repositorio<EmpresaDTO, int>, IRepositorioEmpresa
    {
        public RepositorioEmpresa(ISession sessao) : base(sessao)
        {
        }

        public EmpresaDTO BuscaPrimeiraEmpresa()
        {
            var query = Sessao.QueryOver<EmpresaDTO>()
                .Take(1);

            return query.SingleOrDefault<EmpresaDTO>();
        }

        public IList<EmpresaDTO> BuscaPeloCnpjCpf(string documentoUnico)
        {
            var query = Sessao.QueryOver<EmpresaDTO>()
                .Where(e => e.Cnpj == documentoUnico || e.Cpf == documentoUnico);

            return query.List();
        }

        public EmpresaDTO PeloCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
            {
                return null;
            }

            var query = Sessao.QueryOver<EmpresaDTO>()
                .Where(e => e.Cnpj == cnpj);

            var empresas = query.List();

            return empresas.FirstOrDefault();
        }

        public IEnumerable<EmpresaComboBoxDTO> BuscarEmpresaComboBoxDtos()
        {
            EmpresaDTO empresaAlias = null;
            EmpresaComboBoxDTO resultado = null;

            var query = Sessao.QueryOver(() => empresaAlias)
                .SelectList(
                    list => list.Select(() => empresaAlias.Id).WithAlias(() => resultado.Id)
                        .Select(() => empresaAlias.RazaoSocial).WithAlias(() => resultado.Nome)
                );


            query.TransformUsing(Transformers.AliasToBean<EmpresaComboBoxDTO>());

            var lista = query.List<EmpresaComboBoxDTO>();

            return lista;
        }

        public IList<EmpresaPickerModelDto> BuscarEmpresaPickerModelDtos(string textoBuscado)
        {
            EmpresaDTO empresaAlias = null;
            CidadeDTO cidadeAlias = null;
            EmpresaPickerModelDto empresaPickerModelDto = null;

            var query = Sessao.QueryOver(() => empresaAlias)
                .JoinAlias(() => empresaAlias.CidadeDTO, () => cidadeAlias)
                .SelectList(list => list.Select(() => empresaAlias.Id).WithAlias(() => empresaPickerModelDto.Id)
                    .Select(() => empresaAlias.RazaoSocial).WithAlias(() => empresaPickerModelDto.RazaoSocial)
                    .Select(() => empresaAlias.Cnpj).WithAlias(() => empresaPickerModelDto.Cnpj)
                    .Select(() => empresaAlias.InscricaoEstadual).WithAlias(() => empresaPickerModelDto.InscricaoEstadual)
                    .Select(() => cidadeAlias.Nome).WithAlias(() => empresaPickerModelDto.NomeCidade)
                    .Select(() => cidadeAlias.SiglaUf).WithAlias(() => empresaPickerModelDto.SiglaUf));


            if (textoBuscado.IsNotNullOrEmpty())
            {
                var condicaoOr = Restrictions.Disjunction();

                var likeRazaoSocial = Restrictions.Like(Projections.Property(() => empresaAlias.RazaoSocial),
                    textoBuscado,
                    MatchMode.Anywhere);

                var likeNomeFantasia = Restrictions.Like(Projections.Property(() => empresaAlias.NomeFantasia),
                    textoBuscado,
                    MatchMode.Anywhere);

                var equalCnpj = Restrictions.Eq(Projections.Property(() => empresaAlias.Cnpj), textoBuscado);

                condicaoOr.Add(likeRazaoSocial);
                condicaoOr.Add(likeNomeFantasia);
                condicaoOr.Add(equalCnpj);

                query.Where(condicaoOr);
            }

            query.TransformUsing(Transformers.AliasToBean<EmpresaPickerModelDto>());

            var lisa = query.List<EmpresaPickerModelDto>();

            return lisa;
        }

        public IEnumerable<IEmpresa> BuscarTodas()
        {
            var query = Sessao.QueryOver<EmpresaDTO>();

            return query.List<EmpresaDTO>();
        }

        public DadosDestinatarioDTO GetDadosDestinatarioDTO(int empresaId)
        {
            EmpresaDTO alias = null;
            EstadoDTO aliasUf = null;
            DadosDestinatarioDTO resultado = null;

            var queryOver = Sessao.QueryOver(() => alias)
                .JoinAlias(() => alias.EstadoDTO, () => aliasUf)
                .SelectList(list => list.Select(() => alias.Cnpj).WithAlias(() => resultado.DocumentoUnico)
                    .Select(() => aliasUf.Sigla).WithAlias(() => resultado.SiglaUf)
                );

            queryOver.Where(() => alias.Id == empresaId);

            queryOver.TransformUsing(Transformers.AliasToBean<DadosDestinatarioDTO>());

            return queryOver.SingleOrDefault<DadosDestinatarioDTO>();
        }
    }
}