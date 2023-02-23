using System;
using System.Globalization;
using FusionCore.FusionAdm.Configuracoes;
using FusionCore.FusionNfce.ConfiguracaoBalanca;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Servico.BuscarProduto;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Venda;
using FusionCore.Repositorio.FusionNfce;
using FusionNfce.Visao.Principal.Contratos;
using NHibernate;

namespace FusionNfce.Visao.Principal.Implementacoes.Caixa
{
    public class ComandoCaixaNormal : IComandoCaixa
    {
        private VendaModel _model;
        private ProdutoNfce _produto;

        public void ExecutaAcao(VendaModel model, string cmd, ProdutoNfce produtoBuscaManual = null)
        {
            _model = model;

            ValidaFormasPagamento();

            var configuracaoBalanca = SessaoSistemaNfce.ConfiguracoesBalanca;

            if (configuracaoBalanca.Ativo && produtoBuscaManual == null)
            {
                _produto = BuscaProdutoPorCodigoBalanca(cmd);

                if (_produto != null)
                {
                    CalculaQuantidadeBalanca(cmd, _produto, configuracaoBalanca);
                }
            }

            if (_produto == null)
            {
                _produto = produtoBuscaManual ?? BuscaProdutoPorCodigoBarrasOuCodigo(cmd);
            }

            VerificaProdutoAtivo(_produto);
            VerificaPrecoVenda(_produto);

            if (_produto.PrecisaSolicitarTotal() || SessaoSistemaNfce.Preferencia?.SolicitaInformacaoItem == true)
            {
                _model.ProdutoManual = _produto;
                _model.HabilitarCodigoBarras = false;
                _model.OnSelecionaConteudoTextBox();

                return;
            }

            _produto.ThrowPodeFracionar(_model.Quantidade);

            var espera = new ItemEspera(_produto, cmd);
            model.AdicionaProdutoNaListaDeEspera(espera);
        }

        private void VerificaProdutoAtivo(ProdutoNfce produto)
        {
            if (produto.Ativo == false)
            {
                throw new InvalidOperationException($"Produto está inativo\n{produto.Nome}");
            }
        }

        private void ValidaFormasPagamento()
        {
            if (_model.IsTemFormaDePagamento())
            {
                throw new InvalidOperationException("Verifiquei que houve tentativa de transmissão ou existem pagamentos lançados, cancele a mesma ou transmita\n ou tente limpar as formas de pagamento");
            }
        }

        private void CalculaQuantidadeBalanca(string codigo, ProdutoNfce produto, BalancaNfce configuracao)
        {
            var quatificadorBase = codigo.Substring(configuracao.InicioQuantificador - 1);

            if (codigo.Length == 13)
            {
                quatificadorBase = quatificadorBase.Substring(0, quatificadorBase.Length - 1);
            }

            var quantificadorString = quatificadorBase.Insert(quatificadorBase.Length - configuracao.CasasDecimais, ".");
            var quantificador = decimal.Parse(quantificadorString, new NumberFormatInfo() {NumberDecimalSeparator = "."});

            if (configuracao.ModoDeOperacao == ModoDeOperacao.Preco)
            {
                var quantidade = decimal.Round(quantificador / produto.PrecoVenda, 4);
                ChecaValorMenorQueZero(quantidade);
                _model.Quantidade = quantidade;

                return;
            }

            ChecaValorMenorQueZero(quantificador);
            _model.Quantidade = quantificador;
        }

        private void ChecaValorMenorQueZero(decimal input)
        {
            if (input <= 0M)
            {
                throw new InvalidOperationException($"Quantidade calculada foi {input}, não pode ser menor ou igual a zero.");
            }
        }

        private ProdutoNfce BuscaProdutoPorCodigoBalanca(string codigoBarra)
        {
            if (codigoBarra.Length < 8)
            {
                return null;
            }

            var tamanhoCodigo = SessaoSistemaNfce.ConfiguracoesBalanca.TamanhoCodigo;

            try
            {
                var repositorioProduto =
                    new RepositorioProdutoNfce(GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda)
                        .AbrirSessao());

                using (repositorioProduto)
                {
                    var digitoVerificador = byte.Parse(codigoBarra.Substring(0, 1));
                    if (SessaoSistemaNfce.ConfiguracoesBalanca.DigitoVerificador == digitoVerificador)
                    {
                        var codigoBalanca = int.Parse(codigoBarra.Substring(1, tamanhoCodigo));

                        if (codigoBalanca == 0)
                            throw new InvalidOperationException("O código de balança não pode ser 0\nVerifique o tamanho do código de balança");

                        var produto = repositorioProduto.BuscaPorCodigoBalanca(codigoBalanca);

                        if (produto != null) NHibernateUtil.Initialize(produto.ProdutosAlias);

                        return produto;
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException(
                    @"Código de balança está incorreto pois o mesmo tem que ter o tamanho exato de " + tamanhoCodigo,
                    ex);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException(@"Código de balança deve aver apenas números");
            }

            return null;
        }

        private void VerificaPrecoVenda(ProdutoNfce produto)
        {
            if (produto.PrecoVenda <= 0)
            {
                throw new InvalidOperationException("Produto esta sem preço de venda");
            }
        }

        private static ProdutoNfce BuscaProdutoPorCodigoBarrasOuCodigo(string codigoBarras)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                return new BuscarProdutoFrenteCaixa(sessao, codigoBarras).Buscar();
            }
        }
    }
}