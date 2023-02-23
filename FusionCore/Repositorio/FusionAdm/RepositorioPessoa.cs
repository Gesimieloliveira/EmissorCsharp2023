using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Util;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioPessoa : Repositorio<PessoaEntidade, int>
    {
        public RepositorioPessoa(ISession sessao) : base(sessao)
        {
        }

        public PessoaEntidade GetPeloId(int id, bool carregaLazy = true)
        {
            var query = Sessao.QueryOver<PessoaEntidade>().Where(p => p.Id == id);
            var pessoa = query.SingleOrDefault();

            if (carregaLazy && pessoa != null)
            {
                LoadLazy(pessoa);
            }

            return pessoa;
        }

        public Cliente GetClientePeloId(int id, bool carregaLazy = true)
        {
            var query = Sessao.QueryOver<Cliente>().Where(p => p.Id == id);
            var cliente = query.SingleOrDefault();

            if (carregaLazy && cliente != null)
            {
                LoadLazy(cliente.Pessoa);
            }

            return cliente;
        }

        public Vendedor GetVendedorPeloId(int id, bool carregaLazy = true)
        {
            var query = Sessao.QueryOver<Vendedor>().Where(p => p.Id == id);
            var vendedor = query.SingleOrDefault();

            if (carregaLazy && vendedor != null)
            {
                LoadLazy(vendedor.Pessoa);
            }

            return vendedor;
        }

        public Fornecedor GetFornecedorPeloId(int id, bool carregaLazy = true)
        {
            var query = Sessao.QueryOver<Fornecedor>().Where(p => p.Id == id);
            var result = query.SingleOrDefault();

            if (carregaLazy && result != null)
            {
                LoadLazy(result.Pessoa);
            }

            return result;
        }

        public Transportadora GetTransportadoraPeloId(int id, bool carregaLazy = true)
        {
            var query = Sessao.QueryOver<Transportadora>().Where(p => p.Id == id);
            var result = query.SingleOrDefault();

            if (carregaLazy && result != null)
            {
                LoadLazy(result.Pessoa);
            }

            return result;
        }

        private static void LoadLazy(PessoaEntidade pessoa)
        {
            NHibernateUtil.Initialize(pessoa.Enderecos);
            NHibernateUtil.Initialize(pessoa.Telefones);
            NHibernateUtil.Initialize(pessoa.Emails);
        }

        public IList<PessoaGrid> BuscaPessoasGridModel(FiltroPessoaGrid filtro)
        {
            PessoaEntidade pessoa = null;
            Cliente cliente = null;
            Vendedor vendedor = null;
            Transportadora transportadora = null;
            Fornecedor fornecedor = null;
            PessoaGrid alias = null;
            PessoaEmail pessoaEmail = null;
            PessoaTelefone pessoaTelefone = null;

            var clienteCondition = Projections.Conditional(
                Restrictions.Ge(Projections.Property<Cliente>(c => c.Id), 1),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));

            var isClienteSubQuery = QueryOver.Of(() => cliente)
                .Where(c => c.Id == pessoa.Id)
                .Select(clienteCondition)
                .Take(1);

            var vendedorCondition = Projections.Conditional(
                Restrictions.Ge(Projections.Property<Vendedor>(v => v.Id), 1),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean)
            );

            var isVendedorSubQuery = QueryOver.Of(() => vendedor)
                .Where(v => v.Id == pessoa.Id)
                .Select(vendedorCondition).Take(1);

            var conditionTansp = Projections.Conditional(
                Restrictions.Ge(Projections.Property<Transportadora>(c => c.Id), 1),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));

            var isTransportadoraSubQuery = QueryOver.Of(() => transportadora)
                .Where(c => c.Id == pessoa.Id)
                .Select(conditionTansp)
                .Take(1);

            var conditionFornecedor = Projections.Conditional(
                Restrictions.Ge(Projections.Property<Transportadora>(c => c.Id), 1),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));

            var isFornecedorSubQuery = QueryOver.Of(() => fornecedor)
                .Where(c => c.Id == pessoa.Id)
                .Select(conditionFornecedor)
                .Take(1);

            var query = Sessao.QueryOver(() => pessoa)
                .SelectList(list => list
                    .Select(() => pessoa.Id).WithAlias(() => alias.Id)
                    .Select(() => pessoa.Nome).WithAlias(() => alias.Nome)
                    .Select(() => pessoa.Tipo).WithAlias(() => alias.Tipo)
                    .Select(() => pessoa.Cpf.Valor).WithAlias(() => alias.Cpf)
                    .Select(() => pessoa.Cnpj.Valor).WithAlias(() => alias.Cnpj)
                    .Select(() => pessoa.DocumentoExterior).WithAlias(() => alias.DocumentoExtrangeiro)
                    .Select(() => pessoa.InscricaoEstadual).WithAlias(() => alias.InscricaoEstadual)
                    .Select(() => pessoa.Rg.Rg).WithAlias(() => alias.Rg)
                    .SelectSubQuery(isClienteSubQuery).WithAlias(() => alias.IsCliente)
                    .SelectSubQuery(isVendedorSubQuery).WithAlias(() => alias.IsVendedor)
                    .SelectSubQuery(isTransportadoraSubQuery).WithAlias(() => alias.IsTransportadora)
                    .SelectSubQuery(isFornecedorSubQuery).WithAlias(() => alias.IsFornecedor));

            var and = Restrictions.Conjunction();

            and.Add(Restrictions.Eq(Projections.Property(() => pessoa.Ativo), filtro.Ativos));

            if (filtro.Codigo != null)
            {
                var id = Restrictions.Eq(Projections.Cast(NHibernateUtil.String, Projections.Property(() => pessoa.Id)), filtro.Codigo);
                and.Add(id);
            }

            if (filtro.NomePessoaContenha.IsNotNullOrEmpty())
            {
                var nome = Restrictions.Like(Projections.Property(() => pessoa.Nome), filtro.NomePessoaContenha, MatchMode.Anywhere);
                and.Add(nome);
            }

            if (filtro.NomeFantasiaPessoaContenha.IsNotNullOrEmpty())
            {
                var nomeFantasia = Restrictions.Like(Projections.Property(() => pessoa.NomeFantasia),
                    filtro.NomeFantasiaPessoaContenha,
                    MatchMode.Anywhere);

                and.Add(nomeFantasia);
            }

            if (filtro.DocumentoUnicoIgualA.IsNotNullOrEmpty())
            {
                var cnpj = Restrictions.Eq(Projections.Property(() => pessoa.Cnpj.Valor), filtro.DocumentoUnicoIgualA);
                var cpf = Restrictions.Eq(Projections.Property(() => pessoa.Cpf.Valor), filtro.DocumentoUnicoIgualA);

                var orDocumentoUnico = Restrictions.Disjunction();
                orDocumentoUnico.Add(cnpj);
                orDocumentoUnico.Add(cpf);

                and.Add(orDocumentoUnico);
            }

            if (filtro.EmailPessoaContenha.IsNotNullOrEmpty())
            {
                var emailLike = Restrictions.Like(Projections.Property(() => pessoaEmail.Email.Valor), filtro.EmailPessoaContenha, MatchMode.Anywhere);

                var emailSubQuery = QueryOver.Of(() => pessoaEmail).Where(emailLike)
                    .Select(email => email.Pessoa.Id);

                query.WithSubquery.WhereProperty(() => pessoa.Id).In(emailSubQuery);
            }

            if (filtro.TelefoneIgualA.IsNotNullOrEmpty())
            {
                var telefoneLike = Restrictions.Eq(Projections.Property(() => pessoaTelefone.Numero), filtro.TelefoneIgualA);

                var telefoneSubQuery = QueryOver.Of(() => pessoaTelefone).Where(telefoneLike)
                    .Select(email => email.Contato.Id);

                query.WithSubquery.WhereProperty(() => pessoa.Id).In(telefoneSubQuery);
            }

            query.Where(and);
            query.TransformUsing(Transformers.AliasToBean<PessoaGrid>());

            return query.List<PessoaGrid>();
        }

        public IList<Cliente> BuscaClientesParaSincronizacao(DateTime ultimaAlteracao)
        {
            PessoaEntidade pessoa = null;
            Cliente cliente = null;

            var queryOver = Sessao.QueryOver(() => cliente)
                .JoinAlias(() => cliente.Pessoa, () => pessoa)
                .Where(() => pessoa.AlteradoEm >= ultimaAlteracao);


            return queryOver.List<Cliente>();
        }

        public decimal GetTotalEmAbertoFinanceiro(int clienteId)
        {
            Cliente pessoa = null;

            var queryTotalAberto = Sessao.QueryOver<DocumentoReceber>()
                .Inner.JoinAlias(d => d.Cliente, () => pessoa)
                .Select(Projections.Sum<DocumentoReceber>(d => d.ValorDocumento))
                .Where(d => d.Cliente.Id == clienteId && d.Situacao == Situacao.Aberto);

            var queryTotalQuitado = Sessao.QueryOver<DocumentoReceber>()
                .Select(Projections.Sum<DocumentoReceber>(d => d.ValorQuitado))
                .Where(d => d.Cliente.Id == clienteId && d.Situacao == Situacao.Aberto);

            var valorAberto = queryTotalAberto.SingleOrDefault<decimal>();
            var valorQuitado = queryTotalQuitado.SingleOrDefault<decimal>();

            return valorAberto - valorQuitado;
        }

        public ClienteDt BuscarClientePorCpfOuCnpjPdv(string documentoUnico)
        {
            ClienteDt resultado = null;
            Cliente cliente = null;
            PessoaEntidade pessoa = null;

            var queryOver = Sessao.QueryOver(() => cliente)
                .JoinAlias(() => cliente.Pessoa, () => pessoa)
                .SelectList(list => list
                    .Select(() => cliente.Id).WithAlias(() => resultado.Id)
                    .Select(() => pessoa.Cpf.Valor).WithAlias(() => resultado.Cpf)
                    .Select(() => pessoa.Cnpj.Valor).WithAlias(() => resultado.Cnpj)
                    .Select(() => cliente.LimiteCredito).WithAlias(() => resultado.LimiteCredito)
                    .Select(() => cliente.AplicaLimiteCredito).WithAlias(() => resultado.AplicaLimiteCredito)
                    .Select(() => pessoa.Nome).WithAlias(() => resultado.Nome));

            queryOver.Where(() => pessoa.Cpf.Valor == documentoUnico || pessoa.Cnpj.Valor == documentoUnico);

            queryOver.TransformUsing(Transformers.AliasToBean(typeof(ClienteDt)));

            var lista = queryOver.List<ClienteDt>();

            if (lista != null && lista.Count > 1)
            {
                throw new InvalidOperationException(
                    "Achamos mais de um cliente com o mesmo documento (cpf ou cnpj) porfavor verificar\n\n");
            }

            var clienteDt = lista?[0];

            return clienteDt;
        }

        public PessoaEntidade BuscarPessoaPorDocumentoUnico(string documentoUnicoDestinatario)
        {
            var pessoaList = Sessao.QueryOver<PessoaEntidade>()
                .Where(x => x.Cnpj.Valor == documentoUnicoDestinatario || x.Cpf.Valor == documentoUnicoDestinatario)
                .List<PessoaEntidade>();

            var pessoa = pessoaList?.FirstOrNull() as PessoaEntidade;

            if (pessoa != null)
            {
                LoadLazy(pessoa);
            }

            return pessoa;
        }

        public void SalvaAlteracoes(PessoaEntidade pessoa)
        {
            if (pessoa.Id == 0)
            {
                Sessao.Persist(pessoa);
                SalvarItensNaoSalvos(pessoa);
                Sessao.Flush();
                return;
            }

            SalvarItensNaoSalvos(pessoa);
            Sessao.Merge(pessoa);
            Sessao.Flush();

            if (pessoa.Transportadora != null)
            {
                pessoa.Transportadora.Id = pessoa.Id;
            }

            if (pessoa.Cliente != null)
            {
                pessoa.Cliente.Id = pessoa.Id;
            }

            if (pessoa.Fornecedor != null)
            {
                pessoa.Fornecedor.Id = pessoa.Id;
            }
        }

        private void SalvarItensNaoSalvos(PessoaEntidade pessoa)
        {
            SalvarEmails(pessoa);
            SalvarEnderecos(pessoa);
            SalvarTelefones(pessoa);
        }

        private void SalvarEmails(PessoaEntidade pessoa)
        {
            try
            {
                foreach (var email in pessoa.Emails)
                {
                    if (email.Id == 0)
                    {
                        email.Pessoa = pessoa;
                        Sessao.Persist(email);
                    }
                }
            }
            catch (LazyInitializationException e)
            {
                //igore
            }
        }

        private void SalvarEnderecos(PessoaEntidade pessoa)
        {
            try
            {
                foreach (var endereco in pessoa.Enderecos)
                {
                    if (endereco.Id == 0)
                    {
                        endereco.Residente = pessoa;
                        Sessao.Persist(endereco);
                    }
                }
            }
            catch (LazyInitializationException e)
            {
                // ignroe
            }
        }

        private void SalvarTelefones(PessoaEntidade pessoa)
        {
            try
            {
                foreach (var telefone in pessoa.Telefones)
                {
                    if (telefone.Id == 0)
                    {
                        telefone.Contato = pessoa;
                        Sessao.Persist(telefone);
                    }
                }
            }
            catch (LazyInitializationException e)
            {
                //ignore
            }
        }

        public void SalvaAlteracoes(PessoaTelefone telefone)
        {
            if (telefone.Id == 0)
            {
                Sessao.Persist(telefone);
                Sessao.Flush();
                return;
            }

            Sessao.Update(telefone);
            Sessao.Flush();
        }

        public void Remove(PessoaTelefone telefone)
        {
            Sessao.Delete(telefone);
            Sessao.Flush();
        }

        public void SalvaAlteracoes(PessoaEndereco endereco)
        {
            if (endereco.Id == 0)
            {
                Sessao.Persist(endereco);
                Sessao.Flush();
                return;
            }

            Sessao.Update(endereco);
            Sessao.Flush();
        }

        public void Remove(PessoaEndereco endereco)
        {
            Sessao.Delete(endereco);
            Sessao.Flush();
        }

        public void SalvaAlteracoes(PessoaEmail email)
        {
            if (email.Id == 0)
            {
                Sessao.Persist(email);
                Sessao.Flush();
                return;
            }

            Sessao.Update(email);
            Sessao.Flush();
        }

        public void Remove(PessoaEmail email)
        {
            Sessao.Delete(email);
            Sessao.Flush();
        }

        public IEnumerable<Email> BuscarEmailsPelaPessoaId(int pessoaId)
        {
            PessoaEmail pessoaEmail = null;

            var query = Sessao.QueryOver(() => pessoaEmail)
                .Where(e => e.Pessoa.Id == pessoaId)
                .Select(e => e.Email);

            return query.List<Email>();
        }

        public IEnumerable<Email> BuscarEmails(PessoaEntidade pessoa)
        {
            return BuscarEmailsPelaPessoaId(pessoa.Id);
        }

        public IList<PessoaEntidade> PeloDocumentoUnico(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
            {
                return new List<PessoaEntidade>();
            }

            var query = Sessao.QueryOver<PessoaEntidade>()
                .Where(p => p.Cnpj.Valor == documento || p.Cpf.Valor == documento);

            var result = query.List();

            return result;
        }

        public bool EhUmaTransportadora(int pessoaId)
        {
            var query = Sessao.QueryOver<Transportadora>()
                .Where(i => i.Id == pessoaId);

            return query.RowCount() > 0;
        }
    }
}