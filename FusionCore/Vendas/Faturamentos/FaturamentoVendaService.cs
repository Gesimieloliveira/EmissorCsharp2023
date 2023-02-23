using System;
using FusionCore.Configuracoes;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using FusionCore.ControleCaixa;
using FusionCore.Vendas.Repositorio;
using FusionCore.ControleCaixa.Facades;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.TabelasDePrecos;

namespace FusionCore.Vendas.Faturamentos
{
    public class FaturamentoVendaService
    {
        private readonly ISession _session;
        private readonly UsuarioDTO _usuario;
        private readonly ServicoRegistroDeCaixa _servicoCaixa;
        private readonly RepositorioFaturamento _repositorio;

        public FaturamentoVendaService(ISession session, UsuarioDTO usuario)
        {
            _usuario = usuario;
            _session = session;
            _servicoCaixa = new ServicoRegistroDeCaixa(session, ELocalEventoCaixa.Gestao);
            _repositorio = new RepositorioFaturamento(session);
        }

        public void FaturarProduto(FaturamentoVenda faturamento, ProdutoDTO produto, decimal quantidade)
        {
            ThrowExceptionFaturamentoNaoAberto(faturamento);
            ProdutoAjuda.ValidarUnidadeMedidaPodeFracionar(produto, quantidade);
            BoqueioEstoqueHelper.ChecaEstoqueNegativoAdm(produto, quantidade);

            var movimentarEstoque = new RepositorioMovimentaEstoqueFaturamento(_session)
                .ObterConfiguracaoEstoqueFaturamento()
                .MovimentarEstoque;

            var proximoNumero = (short)(faturamento.Produtos.Count() + 1);

            var item = new FaturamentoProduto(faturamento, _usuario, produto, proximoNumero, produto.PrecoVenda, quantidade)
            {
                MovimentaEstoque = movimentarEstoque
            };

            if (faturamento.TabelaPreco != null)
            {
                var repositorioTabelaPrecos = new RepositorioTabelaPreco(_session);
                var tabelaPrecos = repositorioTabelaPrecos.GetPeloId(faturamento.TabelaPreco.Id);
                item.AplicarTabelaPrecos(tabelaPrecos);
            }

            faturamento.AddItem(item);

            _repositorio.Salvar(faturamento);
        }

        private void ThrowExceptionFaturamentoNaoAberto(FaturamentoVenda faturamento)
        {
            var estadoAtual = _repositorio.ObterEstadoFaturamento(faturamento);
            if (estadoAtual != Estado.Aberto)
                throw new InvalidComObjectException("Faturamento não está mais Aberto");
        }

        public void Finalizar(FaturamentoVenda faturamento, IEnumerable<FPagamento> pagamentos, decimal troco)
        {
            ThrowExceptionFaturamentoNaoAberto(faturamento);
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_usuario);

            faturamento.Pagar(pagamentos.ToList());
            faturamento.Finalizar(troco);

            if (faturamento.Malote != null)
            {
                new RepositorioMalote(_session).Persiste(faturamento.Malote);
            }

            _repositorio.Salvar(faturamento);
            _servicoCaixa.RegistrarVenda(faturamento, _usuario);
        }

        public void Cancelar(FaturamentoVenda faturamento, string justificativa)
        {
            var estadoAtual = _repositorio.ObterEstadoFaturamento(faturamento);
            if (estadoAtual == Estado.Cancelado)
                throw new InvalidOperationException("Faturameto já está cancelado");

            var estavaFinalizado = estadoAtual == Estado.Finalizado;

            faturamento.Cancelar(_repositorio, justificativa);

            _repositorio.CancelarEstoque(faturamento, _usuario);
            _repositorio.Salvar(faturamento);

            if (estavaFinalizado)
            {
                _servicoCaixa.RegistrarEstorno(faturamento, _usuario);
            }
        }

        public void AplicarDesconto(FaturamentoVenda faturamento, decimal percentual)
        {
            ThrowExceptionFaturamentoNaoAberto(faturamento);

            faturamento.AplicarPercentualDesconto(percentual);
            _repositorio.Salvar(faturamento);
        }

        public void VincularDestinatario(FaturamentoVenda faturamento, Cliente cliente)
        {
            ThrowExceptionFaturamentoNaoAberto(faturamento);

            var endereco = cliente.PriorizarEnderecoEntrega();
            faturamento.VincularDestinatario(cliente, endereco);
            _repositorio.Salvar(faturamento);
        }

        public void VincularVendedor(FaturamentoVenda faturamento, Vendedor vendedor)
        {
            ThrowExceptionFaturamentoNaoAberto(faturamento);
            faturamento.DefineVendedor(vendedor);
            _repositorio.Salvar(faturamento);
        }

        public void AlterarObservacao(FaturamentoVenda faturamento, string observacao)
        {
            ThrowExceptionFaturamentoNaoAberto(faturamento);
            faturamento.Observacao = observacao;
            _repositorio.Salvar(faturamento);
        }

        public void RemoverItem(FaturamentoVenda faturamento, int itemId)
        {
            ThrowExceptionFaturamentoNaoAberto(faturamento);
            faturamento.RemoverProduto(itemId);
            _repositorio.Salvar(faturamento);
        }

        public void AplicarTabelaPrecos(FaturamentoVenda faturamento, int tabelaId)
        {
            ThrowExceptionFaturamentoNaoAberto(faturamento);
            var repositorioTabela = new RepositorioTabelaPreco(_session);
            var tabelaPrecos = repositorioTabela.GetPeloId(tabelaId);

            faturamento.AplicarTabelaPrecos(tabelaPrecos);

            _repositorio.Salvar(faturamento);
        }

        public void RemoverTabelaPrecos(FaturamentoVenda faturamento)
        {
            ThrowExceptionFaturamentoNaoAberto(faturamento);
            faturamento.RemoverTabelaPrecos();
            _repositorio.Salvar(faturamento);
        }
    }
}