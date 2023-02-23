using System;
using FusionCore.CadastroUsuario;
using FusionCore.ControleCaixa.Individual;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using NHibernate;

namespace FusionCore.ControleCaixa.Servicos
{
    public class ServicoRegistroDeCaixa
    {
        private readonly ISession _session;
        private readonly ELocalEventoCaixa _localEvento;
        private readonly RepositorioCaixaIndividual _repositorio;

        public ServicoRegistroDeCaixa(ISession session, ELocalEventoCaixa localEvento)
        {
            _session = session;
            _localEvento = localEvento;
            _repositorio = new RepositorioCaixaIndividual(session);
        }

        public void RegistrarVenda(IVendaRegistravelEmCaixa registravel, IUsuario usuario)
        {
            var caixaAberto = ObterCaixaParaUsuario(usuario);

            if (caixaAberto == null)
            {
                return;
            }

            foreach (var op in registravel.ObterOperacoes())
            {
                var fluxo = new Fluxo
                {
                    Caixa = caixaAberto,
                    Usuario = usuario,
                    DataOperacao = op.DataOperacao,
                    TipoOperacao = TipoOperacao.Entrada,
                    TipoPagamento = op.TipoPagamento,
                    ValorOperacao = op.Valor,
                    OrigemEvento = op.OrigemEvento,
                    Historico = "operação de venda"
                };

                _repositorio.Persistir(fluxo);
            }
        }

        private CaixaIndividual ObterCaixaParaUsuario(IUsuario usuario)
        {
            var caixaAberto = _repositorio.BuscarCaixaAberto(usuario, _localEvento);

            return caixaAberto;
        }

        public void RegistrarEstorno(IVendaRegistravelEmCaixa registravel, IUsuario usuario)
        {
            var caixaAberto = ObterCaixaParaUsuario(usuario);

            if (caixaAberto == null)
            {
                return;
            }

            foreach (var op in registravel.ObterOperacoes())
            {
                var fluxo = new Fluxo
                {
                    Caixa = caixaAberto,
                    Usuario = usuario,
                    DataOperacao = op.DataOperacao,
                    TipoOperacao = TipoOperacao.Saida,
                    TipoPagamento = op.TipoPagamento,
                    OrigemEvento = op.OrigemEvento,
                    Historico = "cancelamento/estorno de venda",
                    ValorOperacao = decimal.Negate(op.Valor),
                    EhUmEstorno = true
                };

                _repositorio.Persistir(fluxo);
            }
        }

        public void RegistrarLancamento(LancamentoAvulsoCaixa op)
        {
            var caixaAberto = ObterCaixaParaUsuario(op.UsuarioCriacao);

            if (caixaAberto == null && op.TipoLancamentoCaixa == TipoLancamentoCaixa.LancamentoCaixaIndividual)
            {
                throw new InvalidOperationException("Lançamentos em caixa precisa de um Caixa Individual aberto!");
            }

            var fluxo = new Fluxo
            {
                Caixa = caixaAberto,
                Usuario = op.UsuarioCriacao,
                DataOperacao = op.DataCriacao,
                TipoOperacao = op.TipoOperacao,
                TipoPagamento = ETipoPagamento.Dinheiro,
                OrigemEvento = EOrigemFluxoCaixaIndividual.LancamentoAvulso,
                Historico = op.Motivo,
                ValorOperacao = op.TipoOperacao == TipoOperacao.Saida ? decimal.Negate(op.ValorOperacao) : op.ValorOperacao
            };

            _repositorio.Persistir(fluxo);
        }

        public void RegistrarRecebimento(DocumentoReceberLancamento recebimento)
        {
            if (recebimento.TipoLancamento != TipoLancamento.Pagamento)
            {
                throw new InvalidOperationException("Só é possível registrar lançamento de recebimento no caixa!");
            } 
            
            var caixa = ObterCaixaParaUsuario(recebimento.UsuarioCriacao);
            if (caixa == null) return;

            var fluxo = new Fluxo
            {

                Caixa = caixa,
                Usuario = recebimento.UsuarioCriacao,
                DataOperacao = recebimento.CriadoEm.Value,
                TipoOperacao = TipoOperacao.Entrada,
                TipoPagamento = recebimento.TipoRecebimento?.ToCaixa() ?? ETipoPagamento.Dinheiro,
                OrigemEvento = EOrigemFluxoCaixaIndividual.DocumentoReceber,
                Historico = "lançamento de recebimento em documento a receber",
                ValorOperacao = recebimento.Valor
            };

            _repositorio.Persistir(fluxo);
        }

        public void RegistrarEstornoRecebimento(DocumentoReceberLancamento recebimento)
        {
            if (recebimento.TipoLancamento != TipoLancamento.Pagamento)
            {
                throw new InvalidOperationException("Só é possível registrar lançamento de recebimento no caixa!");
            }

            if (recebimento.Estornado == false)
            {
                throw new InvalidOperationException("Não foi possível registrar o lançamento, o mesmo ainda não foi estornado!");
            }
            
            var caixa = ObterCaixaParaUsuario(recebimento.UsuarioCriacao);
            if (caixa == null) return;

            var fluxo = new Fluxo
            {
                Caixa = caixa,
                Usuario = recebimento.UsuarioEstorno,
                DataOperacao = recebimento.DataEstorno.Value,
                TipoOperacao = TipoOperacao.Saida,
                TipoPagamento = recebimento.TipoRecebimento?.ToCaixa() ?? ETipoPagamento.Dinheiro,
                OrigemEvento = EOrigemFluxoCaixaIndividual.DocumentoReceber,
                Historico = "estorno de recebimento em documento a receber",
                ValorOperacao = decimal.Negate(recebimento.Valor),
                EhUmEstorno = true
            };

            _repositorio.Persistir(fluxo);
        }

        public void RegistrarDespesa(
            IUsuario usuario, 
            decimal valorOperacao, 
            DateTime dataOperacao, 
            ETipoPagamento tipoPagamento, 
            EOrigemFluxoCaixaIndividual origem, 
            string historico
        )
        {
            var caixa = ObterCaixaParaUsuario(usuario);
            if (caixa == null)
            {
                return;
            }

            var fluxo = new Fluxo
            {
                Caixa = caixa,
                Usuario = usuario,
                DataOperacao = dataOperacao,
                TipoOperacao = TipoOperacao.Saida,
                TipoPagamento = tipoPagamento,
                OrigemEvento = origem,
                Historico = historico,
                ValorOperacao = decimal.Negate(valorOperacao)
            };

            _repositorio.Persistir(fluxo);
        }

        public void RegistrarEstornoDespesa(
            IUsuario usuario, 
            decimal valorOperacao, 
            DateTime dataOperacao, 
            ETipoPagamento tipoPagamento, 
            EOrigemFluxoCaixaIndividual origem, 
            string historico
        ) {

            var caixa = ObterCaixaParaUsuario(usuario);
            if (caixa == null) return;

            var fluxo = new Fluxo
            {
                Caixa = caixa,
                Usuario = usuario,
                DataOperacao = dataOperacao,
                TipoOperacao = TipoOperacao.Entrada,
                TipoPagamento = tipoPagamento,
                OrigemEvento = origem,
                Historico = historico,
                ValorOperacao = valorOperacao,
                EhUmEstorno = true
            };

            _repositorio.Persistir(fluxo);
        }
    }
}