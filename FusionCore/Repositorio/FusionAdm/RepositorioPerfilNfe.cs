using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Perfil;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioPerfilNfe : Repositorio<PerfilNfe, short>
    {
        public RepositorioPerfilNfe(ISession sessao) : base(sessao)
        {
        }

        public void SalvarAlteracoes(PerfilNfe perfil)
        {
            if (perfil.Id == 0)
            {
                Persiste(perfil);
                return;
            }

            Altera(perfil);
        }


        private void Persiste(PerfilNfe perfil)
        {
            Sessao.Persist(perfil);
            Sessao.Flush();
        }

        private void Altera(PerfilNfe perfil)
        {
            if (perfil.Transportadora == null)
            {
                Desvincular(perfil.Id, nameof(PerfilNfeTransportadora));
            }

            if (perfil.Destinatario == null)
            {
                Desvincular(perfil.Id, nameof(PerfilNfeDestinatario));
            }

            if (perfil.SimplesNacional == null)
            {
                Desvincular(perfil.Id, nameof(PerfilNfeSimplesNacional));
            }

            Sessao.Merge(perfil);
            Sessao.Flush();
        }

        private void Desvincular(short id, string child)
        {
            var query = Sessao.CreateQuery($"delete from {child} where Id = :id");
            query.SetInt32("id", id);
            query.ExecuteUpdate();
        }

        public void Deleta(PerfilNfe perfil)
        {
            Sessao.Delete(perfil);
            Sessao.Flush();
        }

        public IEnumerable<AbaPerfilNfeDTO> BuscaParaSelecaoAbaPerfil()
        {
            PerfilNfe perfil = null;
            EmissorFiscal emf = null;
            EmpresaDTO emp = null;
            EmissorFiscalNFE emfnfe = null;
            PerfilNfeDestinatario pedest = null;
            PerfilNfeTransportadora petransp = null;
            Cliente cliente = null;
            Transportadora transportadora = null;
            PessoaEntidade pCliente = null;
            Veiculo veiculo = null;

            AbaPerfilNfeDTO to = null;

            var query = Sessao.QueryOver(() => perfil)
                .SelectList(list => list
                    .Select(() => perfil.Id).WithAlias(() => to.Id)
                    .Select(() => perfil.Ativo).WithAlias(() => to.Ativo)
                    .Select(() => perfil.Descricao).WithAlias(() => to.Descricao)
                    .Select(() => perfil.FinalidadeEmissao).WithAlias(() => to.FinalidadeEmissao)
                    .Select(() => perfil.NaturezaOperacao).WithAlias(() => to.NaturezaOperacao)
                    .Select(() => perfil.TipoOperacao).WithAlias(() => to.TipoOperacao)
                    .Select(() => perfil.Observacao).WithAlias(() => to.Observacao)
                    .Select(() => emf.Id).WithAlias(() => to.EmissorFiscalId)
                    .Select(() => cliente.Id).WithAlias(() => to.DestinatarioId)
                    .Select(() => pCliente.Nome).WithAlias(() => to.NomeDestinatario)
                    .Select(() => transportadora.Id).WithAlias(() => to.TransportadoraId)
                    .Select(() => veiculo.Id).WithAlias(() => to.VeiculoId)
                    .Select(() => emp.Id).WithAlias(() => to.EmpresaId)
                    .Select(() => emp.RazaoSocial).WithAlias(() => to.RazaoSocialEmpresa)
                    .Select(() => emp.Cnpj).WithAlias(() => to.CnpjEmpresa)
                    .Select(() => emp.Cpf).WithAlias(() => to.CpfEmpresa)
                    .Select(() => emfnfe.Ambiente).WithAlias(() => to.AmbienteSefaz))
                .JoinAlias(() => perfil.EmissorFiscal, () => emf, JoinType.InnerJoin)
                .JoinAlias(() => emf.Empresa, () => emp, JoinType.InnerJoin)
                .JoinAlias(() => emf.EmissorFiscalNfe, () => emfnfe, JoinType.InnerJoin)
                .JoinAlias(() => perfil.Destinatario, () => pedest, JoinType.LeftOuterJoin)
                .JoinAlias(() => perfil.Transportadora, () => petransp, JoinType.LeftOuterJoin)
                .JoinAlias(() => pedest.Destinatario, () => cliente, JoinType.LeftOuterJoin)
                .JoinAlias(() => petransp.Transportadora, () => transportadora, JoinType.LeftOuterJoin)
                .JoinAlias(() => cliente.Pessoa, () => pCliente, JoinType.LeftOuterJoin)
                .JoinAlias(() => petransp.Veiculo, () => veiculo, JoinType.LeftOuterJoin);

            query.TransformUsing(Transformers.AliasToBean<AbaPerfilNfeDTO>());

            var lista = query.List<AbaPerfilNfeDTO>();

            return lista;
        }

        public IEnumerable<AbaPerfilNfeDTO> BuscaPerfilDeSaidaNormal()
        {
            PerfilNfe perfil = null;
            EmissorFiscal emf = null;
            EmpresaDTO emp = null;
            EmissorFiscalNFE emfnfe = null;
            PerfilNfeDestinatario pedest = null;
            PerfilNfeTransportadora petransp = null;
            Cliente cliente = null;
            Transportadora transportadora = null;
            PessoaEntidade pCliente = null;
            Veiculo veiculo = null;

            AbaPerfilNfeDTO to = null;

            var query = Sessao.QueryOver(() => perfil)
                .SelectList(list => list
                    .Select(() => perfil.Id).WithAlias(() => to.Id)
                    .Select(() => perfil.Ativo).WithAlias(() => to.Ativo)
                    .Select(() => perfil.Descricao).WithAlias(() => to.Descricao)
                    .Select(() => perfil.FinalidadeEmissao).WithAlias(() => to.FinalidadeEmissao)
                    .Select(() => perfil.NaturezaOperacao).WithAlias(() => to.NaturezaOperacao)
                    .Select(() => perfil.TipoOperacao).WithAlias(() => to.TipoOperacao)
                    .Select(() => perfil.Observacao).WithAlias(() => to.Observacao)
                    .Select(() => emf.Id).WithAlias(() => to.EmissorFiscalId)
                    .Select(() => cliente.Id).WithAlias(() => to.DestinatarioId)
                    .Select(() => pCliente.Nome).WithAlias(() => to.NomeDestinatario)
                    .Select(() => transportadora.Id).WithAlias(() => to.TransportadoraId)
                    .Select(() => veiculo.Id).WithAlias(() => to.VeiculoId)
                    .Select(() => emp.Id).WithAlias(() => to.EmpresaId)
                    .Select(() => emp.RazaoSocial).WithAlias(() => to.RazaoSocialEmpresa)
                    .Select(() => emp.Cnpj).WithAlias(() => to.CnpjEmpresa)
                    .Select(() => emfnfe.Ambiente).WithAlias(() => to.AmbienteSefaz))
                .JoinAlias(() => perfil.EmissorFiscal, () => emf, JoinType.InnerJoin)
                .JoinAlias(() => emf.Empresa, () => emp, JoinType.InnerJoin)
                .JoinAlias(() => emf.EmissorFiscalNfe, () => emfnfe, JoinType.InnerJoin)
                .JoinAlias(() => perfil.Destinatario, () => pedest, JoinType.LeftOuterJoin)
                .JoinAlias(() => perfil.Transportadora, () => petransp, JoinType.LeftOuterJoin)
                .JoinAlias(() => pedest.Destinatario, () => cliente, JoinType.LeftOuterJoin)
                .JoinAlias(() => petransp.Transportadora, () => transportadora, JoinType.LeftOuterJoin)
                .JoinAlias(() => cliente.Pessoa, () => pCliente, JoinType.LeftOuterJoin)
                .JoinAlias(() => petransp.Veiculo, () => veiculo, JoinType.LeftOuterJoin);


            var conjunctionWhere = Restrictions.Conjunction();

            conjunctionWhere.Add(Restrictions.Eq(Projections.Property(() => perfil.TipoOperacao), TipoOperacao.Saida));

            conjunctionWhere.Add(Restrictions.Eq(Projections.Property(() => perfil.FinalidadeEmissao),
                FinalidadeEmissao.Normal));

            query.Where(conjunctionWhere);

            query.TransformUsing(Transformers.AliasToBean<AbaPerfilNfeDTO>());

            var lista = query.List<AbaPerfilNfeDTO>();

            return lista;
        }

        public bool ExistePerfilNfeSaidaNormal()
        {
            PerfilNfe perfilAlias = null;

            var quantidadePerfilNormal = Sessao.QueryOver(() => perfilAlias)
                .Where(() => perfilAlias.FinalidadeEmissao == FinalidadeEmissao.Normal)
                .RowCount();

            return quantidadePerfilNormal != 0;
        }

        public PerfilNfeSimplesNacional GetSimplesNacional(short id)
        {
            var query = Sessao.QueryOver<PerfilNfeSimplesNacional>().Where(p => p.Parent.Id == id);
            return query.SingleOrDefault();
        }
    }
}