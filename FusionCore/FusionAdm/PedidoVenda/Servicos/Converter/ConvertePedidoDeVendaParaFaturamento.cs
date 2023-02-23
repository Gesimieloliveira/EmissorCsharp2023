using System.Data;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Vendas.Repositorio;

namespace FusionCore.FusionAdm.PedidoVenda.Servicos.Converter
{
    public class ConvertePedidoDeVendaParaFaturamento
    {
        private readonly PedidoVenda _pedidoVenda;
        private readonly UsuarioDTO _usuario;

        public ConvertePedidoDeVendaParaFaturamento(PedidoVenda pedidoVenda, UsuarioDTO usuario)
        {
            _pedidoVenda = pedidoVenda;
            _usuario = usuario;
        }

        public FaturamentoVenda Executar()
        {
            var faturamento = new FaturamentoVenda(_pedidoVenda.Empresa, _usuario, _pedidoVenda.TabelaPreco)
            {
                Observacao = _pedidoVenda.Observacao.TrimOrEmpty()
            };

            if (_pedidoVenda.Destinatario?.Cliente != null)
            {
                var cliente = _pedidoVenda.Destinatario.Cliente;
                faturamento.VincularDestinatario(cliente, cliente.GetEnderecoPrincipal());
            }

            if (_pedidoVenda.Vendedor?.Vendedor != null)
            {
                var vendedor = _pedidoVenda.Vendedor.Vendedor;
                faturamento.DefineVendedor(vendedor);
            }

            foreach (var item in _pedidoVenda.ItensPedidoVenda)
            {
                faturamento.FaturarProdutoDePedido(
                    item.Produto,
                    _usuario,
                    item.PrecoUnitario,
                    item.Quantidade,
                    item.PercentualDesconto
                );
            }

            faturamento.AplicarDesconto(_pedidoVenda.TotalDesconto);
            faturamento.CalcularTotais();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var repositorioPedidoVenda = new RepositorioPedidoVenda(sessao);
                var repoFaturamentos = new RepositorioFaturamento(sessao);

                repoFaturamentos.Salvar(faturamento);
                repositorioPedidoVenda.RetirarDaReservaEstoquePedidoVenda(_pedidoVenda, _usuario, OrigemEventoEstoque.PedidoVendaReservaEfetuadaFaturamento);
                repositorioPedidoVenda.BaixaDeOrçamentoEstoquePedidoVenda(_pedidoVenda, _usuario, OrigemEventoEstoque.PedidoVendaOrcamentoEfetuadaFaturamento);

                _pedidoVenda.Faturar();

                repositorioPedidoVenda.SalvarAlteracoes(_pedidoVenda);

                transacao.Commit();
            }

            return faturamento;
        }
    }
}