using System;
using System.Globalization;
using FusionCore.FusionAdm.Configuracoes;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;

namespace Fusion.Controles.Checkout
{
    public class CheckoutBalanca
    {
        private readonly ISessaoManager _sessaoManager;
        private Balanca _configuracaoBalanca;

        public CheckoutBalanca(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public bool IsBalancaAtiva
        {
            get
            {
                CarregarConfiguracao();
                return _configuracaoBalanca.Ativo;
            }
        }

        private void CarregarConfiguracao()
        {
            if (_configuracaoBalanca != null)
            {
                return;
            }

            using (var sessao = _sessaoManager.CriaSessao())
            {
                _configuracaoBalanca = new RepositorioBalanca(sessao).BuscarUnicaBalanca();
            }
        }

        public bool CompativelComBalanca(string barras)
        {
            if (barras.Length != 13 && barras.Length != 12)
            {
                return false;
            }

            CarregarConfiguracao();

            return byte.Parse(barras.Substring(0, 1)) == _configuracaoBalanca.DigitoVerificador;
        }

        public ProdutoDTO BuscaProduto(string barras)
        {
            var codigoNaBalanca = barras.Substring(1, _configuracaoBalanca.TamanhoCodigo);

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioProduto(sessao);

                var codigoBalanca = int.Parse(codigoNaBalanca);

                if (codigoBalanca == 0)
                    throw new InvalidOperationException("O código de balança não pode ser 0\nVerifique o tamanho do código de balança");

                var produto = repositorio.BuscaPeloCodigoBalanca(codigoBalanca);

                return produto;
            }
        }

        public decimal CalculaQuantidade(ProdutoDTO produto, string codigo)
        {
            var precoOuPeso = codigo.Substring(_configuracaoBalanca.InicioQuantificador - 1);

            if (codigo.Length == 13)
            {
                precoOuPeso = precoOuPeso.Substring(0, precoOuPeso.Length - 1);
            }

            if (_configuracaoBalanca.ModoDeOperacao == ModoDeOperacao.Preco)
            {
                var precoString = precoOuPeso.Insert(precoOuPeso.Length - _configuracaoBalanca.CasasDecimais, ".");
                var preco = decimal.Parse(precoString, new NumberFormatInfo() {NumberDecimalSeparator = "."});
                var quantidade = decimal.Round(preco / produto.PrecoVenda, 4);

                ChecaValorMenorQueZero(quantidade);

                return quantidade;
            }

            var pesoString = precoOuPeso.Insert(precoOuPeso.Length - _configuracaoBalanca.CasasDecimais, ".");
            var peso = decimal.Parse(pesoString, new NumberFormatInfo() { NumberDecimalSeparator = "." });

            ChecaValorMenorQueZero(peso);

            return peso;
        }

        private void ChecaValorMenorQueZero(decimal input)
        {
            if (input <= 0M)
            {
                throw new InvalidOperationException($"Quantidade calculada foi {input}, não pode ser menor ou igual a zero.");
            }
        }
    }
}