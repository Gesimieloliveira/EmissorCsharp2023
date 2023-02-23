using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using OpenAC.Net.Core.Extensions;


namespace FusionCore.FusionAdm.Financeiro
{
    public sealed class DocumentoReceber : EntidadeBase<int>, IDocumentoReceber
    {
        private readonly IList<DocumentoReceberLancamento> _lancamentos;

        public DocumentoReceber()
        {
            _lancamentos = new List<DocumentoReceberLancamento>();

            EmitidoEm = DateTime.Now;
            Situacao = Situacao.Aberto;
            NumeroDocumento = string.Empty;
            Descricao = string.Empty;
            Parcela = 1;
        }

        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public string NumeroDocumento { get; set; }
        public Cliente Cliente { get; set; }
        public CentroLucro CentroLucro { get; set; }
        public string Descricao { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public UsuarioDTO UsuarioCriacao { get; set; }
        public DateTime EmitidoEm { get; set; }
        public DateTime Vencimento { get; set; }
        public Situacao Situacao { get; set; }
        public DateTime? DataQuitacao { get; set; }
        public byte Parcela { get; set; }
        public Malote Malote { get; set; }
        public ITipoDocumento TipoDocumento { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorDocumento { get; set; }
        public decimal ValorQuitado { get; set; }
        public decimal TotalDesconto { get; set; }
        public decimal TotalJuros { get; private set; }
        public decimal ValorRestante => ValorDocumento + TotalJuros - TotalDesconto - ValorQuitado;
        public decimal ValorRestanteVencido => EstaVencido ? ValorRestante : 0.00M;
        public decimal ValorJurosPendente => DocumentoReceberHelper.CalcularJurosPendente(this);
        public decimal ValorRestanteCorrigido => DocumentoReceberHelper.CorrigirValorRestante(this);
        public bool EstaVencido => Situacao == Situacao.Aberto && Vencimento < DateTime.Today;
        public DateTime? UltimoCalculoJuros { get; private set; }
        public EventoCancelamento Cancelamento { get; private set; }

        public IReadOnlyList<DocumentoReceberLancamento> Lancamentos => _lancamentos.ToList();

        public void CancelarDocumento(UsuarioDTO usuario)
        {
            if (Situacao == Situacao.Cancelado)
            {
                throw new InvalidOperationException("Documento já foi cancelado");
            }

            foreach (var i in _lancamentos)
            {
                if (i.Estornado)
                {
                    continue;
                }

                i.Estornar(usuario);
                i.Cancelar(usuario);
            }

            Cancelamento = new EventoCancelamento(this, DateTime.Now, usuario);
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

        public void CriarMalote(OrigemDocumento origem, UsuarioDTO usuario)
        {
            var malote = Malote.Cria(origem, string.Empty, usuario, 0.00M);

            AnexarMalote(malote);
        }

        public void AnexarMalote(Malote malote)
        {
            if (Malote != null)
            {
                throw new InvalidOperationException("Documento a Receber já possui um Malote Anexado!");
            }

            malote.DocumentosReceber.Add(this);
            Malote = malote;
        }

        public void GerarJuros(UsuarioDTO usuario)
        {
            var valorJuros = DocumentoReceberHelper.CalcularJurosPendente(this);

            if (valorJuros <= 0)
            {
                return;
            }

            var juros = new DocumentoReceberLancamento
            {
                UsuarioCriacao = usuario,
                Descricao = string.Empty,
                DocumentoReceber = this,
                TipoLancamento = TipoLancamento.Juro,
                TipoLancamentoTexto = TipoLancamento.Juro.GetDescription(),
                Valor = valorJuros
            };

            _lancamentos.Add(juros);

            TotalJuros += juros.Valor;
            UltimoCalculoJuros = DateTime.Now;

            AlterarStatusDocumento();
        }

        public void FornecerDesconto(decimal valorDesconto, UsuarioDTO usuario)
        {
            if (valorDesconto == 0)
            {
                return;
            }

            if (valorDesconto > ValorRestanteCorrigido)
            {
                throw new InvalidOperationException(
                    "Valor desconto não pode ser maior que Valor restante do documento");
            }

            var desconto = new DocumentoReceberLancamento
            {
                DocumentoReceber = this,
                UsuarioCriacao = usuario,
                Descricao = string.Empty,
                TipoLancamento = TipoLancamento.Desconto,
                TipoLancamentoTexto = TipoLancamento.Desconto.GetDescription(),
                Valor = valorDesconto
            };

            _lancamentos.Add(desconto);

            TotalDesconto += valorDesconto;

            AlterarStatusDocumento();
        }

        public DocumentoReceberLancamento AdicionarRecebimento(decimal valorRecebido, UsuarioDTO usuario, ETipoRecebimento tipoRecebimento)
        {
            if (valorRecebido <= 0)
            {
                throw new InvalidOperationException("Preciso de um Recebimento com valor superior a R$ 0.00");
            }

            var recebimento = new DocumentoReceberLancamento
            {
                UsuarioCriacao = usuario,
                Descricao = string.Empty,
                DocumentoReceber = this,
                TipoLancamento = TipoLancamento.Pagamento,
                TipoLancamentoTexto = TipoLancamento.Pagamento.GetDescription(),
                Valor = valorRecebido,
                TipoRecebimento = tipoRecebimento
            };

            _lancamentos.Add(recebimento);

            ValorQuitado += valorRecebido;

            AlterarStatusDocumento();

            return recebimento;
        }

        private void AlterarStatusDocumento()
        {
            if (ValorRestante > 0.00M)
            {
                Situacao = Situacao.Aberto;
                DataQuitacao = null;
                return;
            }

            Situacao = Situacao.Quitado;
            DataQuitacao = DateTime.Now;
        }

        public void AjustarValorPara(decimal novoValor, UsuarioDTO usuario)
        {
            if (novoValor == ValorDocumento)
            {
                return;
            }

            if (Id == 0)
            {
                ValorOriginal = novoValor;
                ValorDocumento = novoValor;

                return;
            }

            var diferencaValor = novoValor - ValorDocumento;
            var tipo = diferencaValor < 0 ? TipoLancamento.AjusteParaMenos : TipoLancamento.AjusteParaMais;

            if ((ValorRestante + diferencaValor) < 0)
            {
                throw new InvalidOperationException("Valor restante do documento não pode ficar negativo!");
            }

            var lancamento = new DocumentoReceberLancamento
            {
                UsuarioCriacao = usuario,
                Descricao = "ajuste de valor por alteração no formulário",
                DocumentoReceber = this,
                TipoLancamento = tipo,
                TipoLancamentoTexto = tipo.GetDescription(),
                Valor = Math.Abs(diferencaValor)
            };

            _lancamentos.Add(lancamento);

            ValorDocumento = novoValor;
            AlterarStatusDocumento();
        }

        public DocumentoReceberLancamento EstonarUltimoLancamento(UsuarioDTO usuario)
        {
            if (Situacao == Situacao.Cancelado)
            {
                throw new InvalidOperationException("Não é possível estornar item de um documento cancelado!");
            }

            var lancamento = _lancamentos.LastOrDefault(i => i.Estornado == false);

            if (lancamento == null)
            {
                throw new InvalidOperationException("Não encontrei lançamento para ser estornado!");
            }

            lancamento.Estornar(usuario);

            if (lancamento.TipoLancamento == TipoLancamento.Juro)
            {
                var ultimoJuros = _lancamentos.LastOrDefault(
                    i => i.Estornado == false && i.TipoLancamento == TipoLancamento.Juro);

                UltimoCalculoJuros = ultimoJuros?.CriadoEm;
                TotalJuros -= lancamento.Valor;
                AlterarStatusDocumento();

                return lancamento;
            }

            if (lancamento.TipoLancamento == TipoLancamento.Pagamento)
            {
                ValorQuitado -= lancamento.Valor;
                AlterarStatusDocumento();

                return lancamento;
            }

            if (lancamento.TipoLancamento == TipoLancamento.AjusteParaMais)
            {
                ValorDocumento -= lancamento.Valor;
                AlterarStatusDocumento();
                return lancamento;
            }

            if (lancamento.TipoLancamento == TipoLancamento.AjusteParaMenos)
            {
                ValorDocumento += lancamento.Valor;
                AlterarStatusDocumento();

                return lancamento;
            }

            return lancamento;
        }
    }
}