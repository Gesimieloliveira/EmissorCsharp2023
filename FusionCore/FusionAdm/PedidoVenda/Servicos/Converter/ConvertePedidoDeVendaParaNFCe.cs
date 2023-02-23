using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Pagamento;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Usuario;
using FusionCore.FusionNfce.Venda;
using FusionCore.FusionNfce.Vendedores;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Tributacoes.Flags;
using NHibernate;

namespace FusionCore.FusionAdm.PedidoVenda.Servicos.Converter
{
    public class ConvertePedidoDeVendaParaNFCe
    {
        private readonly PedidoVenda _pedidoVenda;
        private readonly ISession _sessaoNfce;
        private readonly UsuarioNfce _usuario;
        private readonly RegimeTributario _regimeTributario;
        private FusionNfce.Fiscal.Nfce _nfce;

        public ConvertePedidoDeVendaParaNFCe(PedidoVenda pedidoVenda, ISession sessaoNfce, UsuarioNfce usuario, RegimeTributario regimeTributario)
        {
            _pedidoVenda = pedidoVenda;
            _sessaoNfce = sessaoNfce;
            _usuario = usuario;
            _regimeTributario = regimeTributario;
        }

        public void Executar()
        {
            if (_pedidoVenda.Empresa.Cnpj != SessaoSistemaNfce.Empresa().Cnpj)
                throw new InvalidOperationException("Empresa do Pedido de Venda é Diferente da Empresa Configurada no Terminal!");

            _nfce = new FusionNfce.Fiscal.Nfce
            {
                UsuarioCriacao = _usuario,
                Observacao = _pedidoVenda.Observacao.TrimOrEmpty(),
                RegimeTributario = _regimeTributario,
                TipoEmissao = SessaoSistemaNfce.TipoEmissao,
                TabelaPreco = _pedidoVenda.TabelaPreco != null ? new TabelaPrecoNfce(_pedidoVenda.TabelaPreco) : null
            };

            var vendedor = EncontraVendedor();

            new ConstruirNfce(_nfce, vendedor).Constroi();

            if (_pedidoVenda.Destinatario?.Cliente != null)
            {
                _nfce.Destinatario = ConverterDestinatario();
            }


            var itensNfce = ConverterItens();

            //AdicionaDescontoNoTotalComoFormaDePagamento();

            //ConverterFormasDePagamentos();

            MontaEmitente();

            _nfce.AddItens(itensNfce);

            //var desconto = _pedidoVenda.TotalDesconto;
            //_nfce.DistribuiDesconto(desconto);
        }

        private VendedorNfce EncontraVendedor()
        {
            if (_pedidoVenda.NaoContemVendedor()) return null;

            VendedorNfce vendedorEncontrado = null;

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                vendedorEncontrado = new RepositorioPessoaNfce(sessao).BuscarVendedorPorId(_pedidoVenda.Vendedor.Vendedor.Id);
            }

            if (vendedorEncontrado == null)
                throw new InvalidOperationException("Vendedor não encontrado no terminal.");

            return vendedorEncontrado;
        }

        private void ConverterFormasDePagamentos()
        {
            if (_pedidoVenda.Negociacao.Any(i => i.Especie != ETipoPagamento.Dinheiro))
            {
                return;
            }

            foreach (var n in _pedidoVenda.Negociacao)
            {
                var fpg = CriaFormaPagamento(n.Valor);

                if (n.Especie == ETipoPagamento.Dinheiro)
                {
                    fpg.IdFormaPagamento = FormaPagamentoNfce.Dinheiro;
                    fpg.Nome = "Dinheiro";
                }

                _nfce.AddFormaPagamento(fpg);
            }
        }

        private void AdicionaDescontoNoTotalComoFormaDePagamento()
        {
            var totalDesconto = _pedidoVenda.TotalDesconto -
                                _pedidoVenda.ItensPedidoVenda.Sum(item => item.TotalDesconto);

            if (totalDesconto != 0)
            {
                var formaPagamento = CriaFormaPagamento(totalDesconto);
                formaPagamento.IdFormaPagamento = FormaPagamentoNfce.AjusteSaldo;
                formaPagamento.AjusteTipo = AjusteTipo.Desconto;
                formaPagamento.IsAjuste = true;
                formaPagamento.Nome = "Desconto";

                _nfce.AddFormaPagamento(formaPagamento);
            }
        }

        private FormaPagamentoNfce CriaFormaPagamento(decimal valor)
        {
            var formaPagamento = new FormaPagamentoNfce
            {
                Nfce = _nfce,
                ValorPagamento = valor,
                TipoAmbiente = SessaoSistemaNfce.GetAmbienteSefaz()
            };
            return formaPagamento;
        }

        private void MontaEmitente()
        {
            _nfce.Emitente = new NfceEmitente
            {
                Nfce = _nfce,
                Empresa = SessaoSistemaNfce.Empresa()
            };
        }

        private List<NfceItem> ConverterItens()
        {
            var itensNfce = new List<NfceItem>();

            var total = _pedidoVenda.ItensPedidoVenda.Sum(i => i.Total);

            var k = _pedidoVenda.TotalDesconto / total;

            //_pedidoVenda.ItensPedidoVenda.

            var quantidade = _pedidoVenda.ItensPedidoVenda.Count();

            for (int i = 0; i < quantidade; i++)
            {
                var item = _pedidoVenda.ItensPedidoVenda.ToArray()[i];

                var totalDesconto = decimal.Round(item.Total * k + item.TotalDesconto, 2);

                var repositorioProduto = new RepositorioProdutoNfce(_sessaoNfce);
                var produtoNfce = repositorioProduto.GetPeloId(item.Produto.Id);

                ProdutoExisteNoTerminal(produtoNfce);

                var itemEspera = new ItemEspera(produtoNfce, produtoNfce.Id.ToString());
                var repositorioIbge = new RepositorioIbptNfce(_sessaoNfce);
                var repositorioTabelaPreco = new RepositorioTabelaPrecoNfce(_sessaoNfce);

                var nfceItem = NfceItem.ConstroiNfceItem(itemEspera, item.Numero, _nfce, repositorioIbge, repositorioTabelaPreco);

                var preco = PrecoItem.Factory(
                    item.Quantidade,
                    item.PrecoUnitario,
                    0.0m,
                    0.0m,
                    item.TotalBruto,
                    item.Total,
                    totalDesconto,
                    item.TotalBruto
                );

                nfceItem.DefinirPreco(preco);

                itensNfce.Add(nfceItem);
            }

            return itensNfce;
        }

        private static void ProdutoExisteNoTerminal(ProdutoNfce produtoNfce)
        {
            if (produtoNfce == null)
            {
                throw new InvalidOperationException("Produto não encontrado, aguardar sincronização de dados");
            }
        }

        private NfceDestinatario ConverterDestinatario()
        {
            var clienteNfce = new RepositorioPessoaNfce(_sessaoNfce).GetPeloId(_pedidoVenda.Destinatario.Cliente.Id);

            if (clienteNfce == null)
            {
                throw new InvalidOperationException("Cliente não encontrado, aguardar sincronização de dados");
            }

            if (clienteNfce.Enderecos.Count == 0)
            {
                throw new InvalidOperationException("Cliente não tem endereço, adicionar o mesmo no administrativo");
            }

            var primeiroEnderecoDaLista = clienteNfce.Enderecos.FirstOrDefault();

            var destinatario = new NfceDestinatario
            {
                Cliente = clienteNfce,
                Bairro = primeiroEnderecoDaLista.Bairro,
                Cep = primeiroEnderecoDaLista.Cep,
                Cidade = primeiroEnderecoDaLista.Cidade,
                Complemento = primeiroEnderecoDaLista.Complemento,
                DocumentoUnico = clienteNfce.DocumentoUnico,
                Email = clienteNfce.Emails.FirstOrDefault() == null ? string.Empty : clienteNfce.Emails.FirstOrDefault().Email,
                InscricaoEstadual = clienteNfce.InscricaoEstadual,
                Logradouro = primeiroEnderecoDaLista.Logradouro,
                Nfce = _nfce,
                Nome = clienteNfce.Nome,
                Numero = primeiroEnderecoDaLista.Numero
            };
            
            return destinatario;
        }

        public FusionNfce.Fiscal.Nfce ObterNfce()
        {
            return _nfce;
        }
    }
}