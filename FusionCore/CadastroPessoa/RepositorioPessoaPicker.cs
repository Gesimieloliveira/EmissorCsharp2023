using System.Collections.Generic;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;
using NHibernate.Criterion;

namespace FusionCore.CadastroPessoa
{
    public class RepositorioPessoaPicker : RepositorioBase
    {
        public RepositorioPessoaPicker(ISession sessao) : base(sessao)
        {
        }

        public IEnumerable<PessoaGrid> BuscarPorVariasCorrespondencias(string filtro = null)
        {
            var query = QueryPicker.CriarApenasAtivos(Sessao);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var typeString = NHibernateUtil.String;

                var id = Restrictions.Eq(Projections.Cast(typeString, Projections.Property(() => QueryPicker.TbPessoa.Id)), filtro);
                var cnpj = Restrictions.Eq(Projections.Property(() => QueryPicker.TbPessoa.Cnpj.Valor), filtro);
                var cpf = Restrictions.Eq(Projections.Property(() => QueryPicker.TbPessoa.Cpf.Valor), filtro);
                var rg = Restrictions.Eq(Projections.Property(() => QueryPicker.TbPessoa.Rg.Rg), filtro);
                var nome = Restrictions.Like(Projections.Property(() => QueryPicker.TbPessoa.Nome), filtro, MatchMode.Anywhere);
                var nomeFantasia = Restrictions.Like(Projections.Property(() => QueryPicker.TbPessoa.NomeFantasia), filtro, MatchMode.Anywhere);

                query.And(id || cnpj || cpf || nome || nomeFantasia || rg);
            }

            return query.List<PessoaGrid>();
        }

        public IEnumerable<PessoaGrid> BuscarPorEmail(string email)
        {
            var query = QueryPicker.CriarApenasAtivos(Sessao);

            if (!string.IsNullOrWhiteSpace(email))
            {
                var like = Restrictions.Like(
                    Projections.Property(() => QueryPicker.TbPessoaEmail.Email.Valor), email, MatchMode.Anywhere);

                var subquery = QueryOver.Of(() => QueryPicker.TbPessoaEmail)
                    .Select(e => e.Pessoa.Id)
                    .Where(like);

                query.WithSubquery.WhereProperty(() => QueryPicker.TbPessoa.Id).In(subquery);
            }

            return query.List<PessoaGrid>();
        }

        public IList<PessoaGrid> BuscaPorTelefone(string endereco = null)
        {
            var query = QueryPicker.CriarApenasAtivos(Sessao);

            if (!string.IsNullOrWhiteSpace(endereco))
            {

                var telefoneLike = Restrictions.Eq(Projections.Property(() => QueryPicker.TbPessoaTelefone.Numero), endereco);

                var telefoneSubQuery = QueryOver.Of(() => QueryPicker.TbPessoaTelefone)
                    .Where(telefoneLike)
                    .Select(email => email.Contato.Id);

                query.WithSubquery.WhereProperty(() => QueryPicker.TbPessoa.Id).In(telefoneSubQuery);
            }

            return query.List<PessoaGrid>();
        }

        public IList<PessoaGrid> BuscaPorDocumento(string documento = null)
        {
            var query = QueryPicker.CriarApenasAtivos(Sessao);

            if (!string.IsNullOrWhiteSpace(documento))
            {
                var cpf = Restrictions.Like(Projections.Property(() => QueryPicker.TbPessoa.Cpf.Valor), documento, MatchMode.Anywhere);
                var cnpj = Restrictions.Like(Projections.Property(() => QueryPicker.TbPessoa.Cnpj.Valor), documento, MatchMode.Anywhere);

                query.And(cpf || cnpj);
            }

            return query.List<PessoaGrid>();
        }

        public IList<PessoaGrid> BuscaPorNome(string nome = null)
        {
            var query = QueryPicker.CriarApenasAtivos(Sessao);

            if (!string.IsNullOrWhiteSpace(nome))
            {
                var nomeRazao = Restrictions.Like(Projections.Property(() => QueryPicker.TbPessoa.Nome), nome, MatchMode.Anywhere);
                var fantasia = Restrictions.Like(Projections.Property(() => QueryPicker.TbPessoa.NomeFantasia), nome, MatchMode.Anywhere);

                query.And(nomeRazao || fantasia);
            }

            return query.List<PessoaGrid>();
        }

        public IList<PessoaGrid> BuscaPorEndereco(string input = null)
        {
            var query = QueryPicker.CriarApenasAtivos(Sessao);

            if (!string.IsNullOrWhiteSpace(input))
            {

                var logradouro = Restrictions.Like(Projections.Property(() => QueryPicker.TbPessoaEndereco.Logradouro), input, MatchMode.Anywhere);

                var enderecoSubQuery = QueryOver.Of(() => QueryPicker.TbPessoaEndereco)
                    .Where(logradouro)
                    .Select(endereco => endereco.Residente.Id);

                query.WithSubquery.WhereProperty(() => QueryPicker.TbPessoa.Id).In(enderecoSubQuery);
            }

            return query.List<PessoaGrid>();
        }

        public IList<PessoaGrid> BuscaPorVendedor(string nome = null)
        {
            var query = QueryPicker.CriarApenasAtivos(Sessao);

            if (!string.IsNullOrWhiteSpace(nome))
            {
                var vendedor = Restrictions.Like(Projections.Property(() => QueryPicker.TbPessoa.Nome), nome, MatchMode.Anywhere);

                var nomeSubQuery = QueryOver.Of(() => QueryPicker.TbVendedor)
                    .Where(vendedor)
                    .Select(n => n.Id);

                query.WithSubquery.WhereProperty(() => QueryPicker.TbPessoa.Id).In(nomeSubQuery);
                  
            }
            else
            {
                var todosVendedores = QueryOver.Of(() => QueryPicker.TbVendedor)
                    .Select(n => n.Id);
                query.WithSubquery.WhereProperty(() => QueryPicker.TbPessoa.Id).In(todosVendedores);
            }

            return query.List<PessoaGrid>();
        }
    }
}