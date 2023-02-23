using System;
using FusionCore.FusionNfce.Produto;
using FusionNfce.Visao.Principal.Contratos;

namespace FusionNfce.Visao.Principal.Implementacoes.Caixa
{
    public class ComandoCaixaAdicionaQuantidade : IComandoCaixa
    {
        private decimal _quantidade;
        private VendaModel _model;

        public void ExecutaAcao(VendaModel model, string cmd, ProdutoNfce produtoBuscaManual = null)
        {
            _model = model;
            ConverteEmQuantidade(cmd);
            var quantidade = QuantidadeAAdicionarEValido();

            _model.Quantidade = quantidade;

            _model.StatusCaixa = StatusCaixa.Normal;
        }

        private void ConverteEmQuantidade(string cmd)
        {
            try
            {
                var valor = cmd.Replace(".", ",");
                _quantidade = decimal.Parse(valor);
            }
            catch (OverflowException)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                _model.InformacaoAcaoBarras = "Venda";
                throw new InvalidOperationException("Quantidade informada e inválida por ser muito grande");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Quantidade informada é inválida", ex);
            }
        }

        private decimal QuantidadeAAdicionarEValido()
        {
            if (_quantidade == 0 || _quantidade < 0)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                throw new InvalidOperationException("Valor da quantidade tem que ser maior que 0");
            }

            if (_quantidade <= 9999) return _quantidade;

            _model.StatusCaixa = StatusCaixa.Normal;
            throw new ArgumentException("Valor máximo para quantidade é 9999");
        }
    }
}