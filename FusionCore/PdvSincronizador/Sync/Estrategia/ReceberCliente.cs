using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate.Util;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class ReceberCliente : SincronizacaoBase
    {
        public override string Tag { get; } = @"receber-cliente";

        public override void Sincronizar(DateTime ultimaSincronizacao)
        {

            var pessoasAdm = ObterPessoasAlteradas(ultimaSincronizacao);
            
            if (pessoasAdm.Count == 0)
                return;

            var clienteRepositorio = new ClienteRepositorio(SessaoPdv);
            var transacao = SessaoPdv.BeginTransaction();

            try
            {
                pessoasAdm.ForEach(pessoaAdm =>
                {
                    var clientePdv = CriarCliente(pessoaAdm);
                    var endereco = (PessoaEndereco) pessoaAdm.Enderecos.FirstOrNull();

                    PreencherEndereco(clientePdv, endereco);
                    PreencherTipoDePessoa(pessoaAdm, clientePdv);
                    clienteRepositorio.Salvar(clientePdv);
                });

                transacao.Commit();
                RegistraEvento = true;
            }
            catch (Exception)
            {
                transacao.Rollback();
                throw;
            }
        }

        private IList<Cliente> ObterPessoasAlteradas(DateTime ultimaSincronizacao)
        {
            var repositorio = new RepositorioPessoa(SessaoAdm);

            var pessoas = repositorio.BuscaClientesParaSincronizacao(ultimaSincronizacao);

            return pessoas;
        }

        private static ClienteDt CriarCliente(Cliente pessoaAdm)
        {
            return new ClienteDt
            {
                Id = pessoaAdm.Id,
                Nome = pessoaAdm.Nome,
                Endereco = string.Empty
            };
        }

        private static void PreencherEndereco(ClienteDt clientePdv, PessoaEndereco endereco)
        {
            if (endereco == null)
                return;

            clientePdv.Endereco = endereco.Logradouro;
        }

        private static void PreencherTipoDePessoa(Cliente pessoaAdm, ClienteDt clientePdv)
        {
            if (Equals(pessoaAdm.Tipo, PessoaTipo.Fisica))
            {
                clientePdv.Cpf = pessoaAdm.Cpf.ToString();
            }
            else if (Equals(pessoaAdm.Tipo, PessoaTipo.Juridica))
            {
                clientePdv.Cnpj = pessoaAdm.Cnpj.Valor;
            }
        }
    }
}