using System;
using System.Collections.Generic;
using FusionCore.FusionNfce.Fiscal.Converter;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace FusionCore.NfceSincronizador.Sync.Produtos
{
    public class ReceberProduto : ISincronizavelPadrao
    {
        private const int ExecutarCommit = 100;
        private int HoraDeComitar { get; set; }
        private ISession _sessaoServidor = null;
        private ISession _sessaoNfce = null;
        private ITransaction _transacaoServidor = null;
        private ITransaction _transacaoNfce = null;
        private RepositorioProduto _repositorioProdutoServidor = null;
        private RepositorioProdutoNfce _repositorioProdutoNfce = null;
        private RepositorioNcm _repositorioNcmServidor = null;
        private RepositorioSincronizacaoPendente _repositorioSincronizacaoPendente = null;


        public void RealizarSincronizacao()
        {
            try
            {
                var todasSincronizacaoPendentes = TentaAcharConfiguracaoNoServidorSeAverBinding();

                foreach (var pendente in todasSincronizacaoPendentes)
                {
                    ResetaValores();

                    var produtoEstoque = _repositorioProdutoServidor.GetEstoquePeloId(int.Parse(pendente.Referencia));
                    var ncm = _repositorioNcmServidor.GetPeloId(produtoEstoque.ProdutoDTO.Ncm);

                    var converterProduto = new ConverterProdutoAdmParaProdutoNfce(produtoEstoque, ncm);

                    converterProduto.DeletarAliasHandler += delegate (object sender, ProdutoNfce produto)
                    {
                        _repositorioProdutoNfce.DeletarProdutoAlias(produto.Id);
                    };

                    var produtoNfce = converterProduto.Executar();

                    _repositorioProdutoNfce.Salvar(produtoNfce);

                    if (produtoNfce.IsTemAlias())
                        foreach (var produtoAlias in produtoNfce.ProdutosAlias)
                        {
                            _repositorioProdutoNfce.Salvar(produtoAlias);
                        }

                    DeletarSincronizacaoPendente(pendente);

                    HoraDeComitar++;

                    ExecutaCommit();
                }

                if (HoraDeComitar == 0) return;
                HoraDeComitar = ExecutarCommit;
                ExecutaCommit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void DeletarSincronizacaoPendente(SincronizacaoPendente pendente)
        {
            _repositorioSincronizacaoPendente.Deletar(pendente);
        }

        private void ResetaValores()
        {
            if (HoraDeComitar != 0) return;

            _sessaoServidor = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao();
            _sessaoNfce = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();
            _transacaoServidor = _sessaoServidor.BeginTransaction();
            _transacaoNfce = _sessaoNfce.BeginTransaction();

            _repositorioProdutoServidor = new RepositorioProduto(_sessaoServidor);
            _repositorioProdutoNfce = new RepositorioProdutoNfce(_sessaoNfce);
            _repositorioNcmServidor = new RepositorioNcm(_sessaoServidor);
            _repositorioSincronizacaoPendente = new RepositorioSincronizacaoPendente(_sessaoServidor);
        }

        private void ExecutaCommit()
        {
            if (HoraDeComitar % ExecutarCommit != 0) return;

            _sessaoServidor.Flush();
            _sessaoServidor.Clear();
            _sessaoNfce.Flush();
            _sessaoNfce.Clear();

            _transacaoServidor.Commit();
            _transacaoNfce.Commit();
            HoraDeComitar = 0;
        }

        private IEnumerable<SincronizacaoPendente> TentaAcharConfiguracaoNoServidorSeAverBinding()
        {
            var sessaoServidor = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao();
            var sessaoNfce = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

            using (sessaoServidor)
            using (sessaoNfce)
            {
                var repositorioTerminalOfflineConfiguracao = new RepositorioConfiguracaoTerminalNfce(sessaoNfce);
                var repositorioSincronizacaoPendente = new RepositorioSincronizacaoPendente(sessaoServidor);

                var configuracaoTerminalNfce = repositorioTerminalOfflineConfiguracao.GetPeloId(1);

                sessaoNfce.Clear();

                if (configuracaoTerminalNfce == null)
                {
                    configuracaoTerminalNfce = SessaoSistemaNfce.Configuracao;
                }

                if (configuracaoTerminalNfce == null)
                {
                    throw new InvalidOperationException("Este terminal não foi configurado. Preciso que configure!");
                }


                var todasSincronizacaoPendentes = repositorioSincronizacaoPendente.BuscaTodosParaSincronizacao(
                    EntidadeSincronizavel.Produto, configuracaoTerminalNfce.TerminalOfflineId);

                return todasSincronizacaoPendentes;
            }
        }
    }
}