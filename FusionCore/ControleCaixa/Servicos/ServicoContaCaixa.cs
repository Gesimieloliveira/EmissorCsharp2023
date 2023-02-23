using System;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.Core.Flags;
using NHibernate;

namespace FusionCore.ControleCaixa.Servicos
{
    public class ServicoContaCaixa
    {
        private readonly RepositorioContaCaixa _repositorioContaCaixa;

        public ServicoContaCaixa(ISession sessao)
        {
            _repositorioContaCaixa = new RepositorioContaCaixa(sessao);
        }

        public bool PrecisaRegistrarAberturaDoCaixa(CaixaIndividual caixa)
        {
            return _repositorioContaCaixa.ExisteRegistroDeAberturaDoCaixa(caixa) == false;
        }

        public bool PrecisaRegistrarFechamentoDoCaixa(CaixaIndividual caixa)
        {
            if (caixa.EstadoAtual != EEstadoCaixa.Fechado)
            {
                return false;
            }

            return _repositorioContaCaixa.ExisteRegistroDeFechamentoDoCaixa(caixa) == false;
        }

        public void RegistrarAberturaCaixaLoja(CaixaIndividual caixa)
        {
            var totalOperacao = decimal.Negate(caixa.SaldoInicial);
            var saldoAnterior = _repositorioContaCaixa.ObtemSaldoAtualCaixaLoja();
            var usuarioOperacao = _repositorioContaCaixa.Parse(caixa.Usuario);
            var novoSaldo = saldoAnterior + totalOperacao;

            var fluxoContaCaixa = new FluxoContaCaixa(
                caixa.DataAbertura,
                usuarioOperacao,
                TipoOperacao.Saida,
                EOrigemFluxoContaCaixa.AberturaDeCaixaIndividual,
                totalOperacao,
                novoSaldo,
                "abertura de caixa individual - saldo inicial"
            );

            fluxoContaCaixa.AnexarComoReferencia(caixa.Id);

            _repositorioContaCaixa.Persistir(fluxoContaCaixa);
        }

        public void RegistrarLancamentoCaixaLojaAvulso(LancamentoAvulsoCaixa operacao)
        {
            var totalOperacao = operacao.TipoOperacao == TipoOperacao.Entrada 
                ? operacao.ValorOperacao 
                : decimal.Negate(operacao.ValorOperacao);

            var saldoAnterior = _repositorioContaCaixa.ObtemSaldoAtualCaixaLoja();
            var usuarioOperacao = _repositorioContaCaixa.Parse(operacao.UsuarioCriacao);
            var novoSaldo = saldoAnterior + totalOperacao;

            var fluxoContaCaixa = new FluxoContaCaixa(
                operacao.DataCriacao,
                usuarioOperacao,
                operacao.TipoOperacao,
                EOrigemFluxoContaCaixa.LancamentoAvulso,
                totalOperacao,
                novoSaldo,
                operacao.Motivo
            );

            fluxoContaCaixa.AnexarComoReferencia(operacao.Id);
            _repositorioContaCaixa.Persistir(fluxoContaCaixa);
        }

        public void RegistrarFechamentoEmCaiaLoja(CaixaIndividual caixa)
        {
            if (caixa.EstadoAtual != EEstadoCaixa.Fechado)
            {
                throw new InvalidOperationException("Caixa precisa estar fechado para ser regsitrado o fechamento no caixa loja!");
            }

            if (caixa.DataFechamento == null)
            {
                throw new InvalidOperationException("Data do fechamento do caixa não pode ser vazia ao registrar o fechamento no caixa loja!");
            }

            var totalOperacao = caixa.SaldoInformado;
            var saldoAnterior = _repositorioContaCaixa.ObtemSaldoAtualCaixaLoja();
            var usuarioOperacao = _repositorioContaCaixa.Parse(caixa.Usuario);
            var novoSaldo = saldoAnterior + totalOperacao;

            var fluxoContaCaixa = new FluxoContaCaixa(
                caixa.DataFechamento.Value,
                usuarioOperacao,
                TipoOperacao.Entrada,
                EOrigemFluxoContaCaixa.FechamentoDeCaixaIndividual,
                totalOperacao,
                novoSaldo,
                "fechamento de caixa individual"
            );

            fluxoContaCaixa.AnexarComoReferencia(caixa.Id);

            _repositorioContaCaixa.Persistir(fluxoContaCaixa);
        }
    }
}