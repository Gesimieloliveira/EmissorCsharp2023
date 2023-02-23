using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Financeiro
{
    public sealed class DocumentoPagar
    {
        private readonly IList<DocumentoPagarLancamento> _lancamentos;

        // ReSharper disable once EmptyConstructor nHibernate
        public DocumentoPagar()
        {
            _lancamentos = new List<DocumentoPagarLancamento>();
        }

        public int Id { get; set; }
        public string NumeroDocumento { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public string Descricao { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public DateTime? EmitidoEm { get; set; }
        public DateTime? Vencimento { get; set; } = DateTime.Now;
        public Situacao Situacao { get; set; } = Situacao.Aberto;
        public byte Parcela { get; set; } = 1;
        public Malote Malote { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorAjustado { get; set; }
        public decimal Juros { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal ValorQuitado { get; set; }
        public decimal ValorEmAberto => ValorAjustado - ValorQuitado;

        public IEnumerable<DocumentoPagarLancamento> Lancamentos => _lancamentos;

        public void Estornar()
        {
            if (PossuiLancamentoNaoEstornado())
            {
                throw new InvalidOperationException("Documento precisa estar com todos os lançamento estornados!");
            }

            Situacao = Situacao.Cancelado;
        }

        public bool EstaCancelado()
        {
            return Situacao == Situacao.Cancelado;
        }

        public bool EstaQuitado()
        {
            return Situacao == Situacao.Quitado;
        }

        public bool NaoEstaQuitado()
        {
            return Situacao != Situacao.Quitado;
        }

        public bool PossuiLancamentoNaoEstornado()
        {
            return _lancamentos.Any(i => i.Estornado != true);
        }

        public void AdicionarJuros(decimal juros, string descricao)
        {
            ThrowExceptionSeDocumentoNaoAberto();

            var lancamentoJuros = DocumentoPagarLancamento.Cria(descricao, juros, TipoLancamento.Juro, this);

            _lancamentos.Add(lancamentoJuros);

            Juros += juros;
            ValorAjustado += juros;
        }

        public void AdicionarDesconto(decimal valorDesconto, string descricao)
        {
            ThrowExceptionSeDocumentoNaoAberto();

            if (valorDesconto > ValorEmAberto)
            {
                throw new InvalidOperationException("Desconto não pode ser maior que o valor restante do documento!");
            }

            var lancamento = DocumentoPagarLancamento.Cria(descricao, valorDesconto, TipoLancamento.Desconto, this);

            _lancamentos.Add(lancamento);

            Desconto += valorDesconto;
            ValorAjustado -= valorDesconto;

            if (ValorEmAberto == 0)
            {
                Situacao = Situacao.Quitado;
            }
        }

        public void AdicionarPagamento(decimal valorPago, string descricao)
        {
            ThrowExceptionSeDocumentoNaoAberto();

            if (valorPago > ValorEmAberto)
            {
                throw new InvalidOperationException("Pagamento não pode ser maior que o valor restante do documento!");
            }

            _lancamentos.Add(DocumentoPagarLancamento.Cria(descricao, valorPago, TipoLancamento.Pagamento, this));

            ValorQuitado += valorPago;

            if (ValorEmAberto == 0)
            {
                Situacao = Situacao.Quitado;
            }
        }

        public void EstornarJuros(decimal valor)
        {
            ThrowExceptionSeDocumentoNaoAberto();

            Juros -= valor;
            ValorAjustado -= valor;

            if (ValorEmAberto == 0)
            {
                Situacao = Situacao.Quitado;
            }
        }

        private void ThrowExceptionSeDocumentoNaoAberto()
        {
            if (Situacao != Situacao.Aberto)
            {
                throw new InvalidOperationException("Documento precisa estar aberto para receber essa operação!");
            }
        }

        public void EstornarDesconto(decimal valor)
        {
            ThrowExceptionSeDocumentoNaoAberto();

            Desconto -= valor;
            ValorAjustado += valor;

            if (ValorEmAberto != 0)
            {
                Situacao = Situacao.Aberto;
            }
        }

        public void EstornarPagamento(decimal valor)
        {
            ValorQuitado -= valor;

            if (ValorEmAberto != 0)
            {
                Situacao = Situacao.Aberto;
            }
        }
    }
}