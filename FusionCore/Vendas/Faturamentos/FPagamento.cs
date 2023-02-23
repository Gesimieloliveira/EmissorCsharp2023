using System;
using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.Vendas.Faturamentos
{
    public class FPagamento : Entidade
    {
        private readonly IList<FParcela> _parcelas = new List<FParcela>();

        private FPagamento()
        {
            // nhibernate
        }

        public int Id { get; private set; }
        protected override int ReferenciaUnica => Id;
        public FaturamentoVenda Faturamento { get; private set; }
        public ETipoPagamento Especie { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public UsuarioDTO CriadoPor { get; private set; }
        public ITipoDocumento TipoDocumento { get; private set; }
        public decimal Valor { get; private set; }
        public bool PossuiParcelas { get; private set; }
        public bool RegistraFinanceiro { get; private set; }

        public IEnumerable<FParcela> Parcelas => _parcelas;

        public static FPagamento CriarNoDinheiro(
            UsuarioDTO usuario,
            decimal valor
        )
        {
            var e = new FPagamento
            {
                CriadoPor = usuario,
                Valor = valor,
                CriadoEm = DateTime.Now,
                Especie = ETipoPagamento.Dinheiro,
                PossuiParcelas = false,
                TipoDocumento = null
            };

            return e;
        }

        public static FPagamento CriarNoPrazo(
            UsuarioDTO usuario,
            ITipoDocumento tipoDocumento,
            IList<FParcela> parcelas
        )
        {
            var e = new FPagamento
            {
                CriadoPor = usuario,
                CriadoEm = DateTime.Now,
                Especie = ETipoPagamento.CreditoLoja,
                TipoDocumento = tipoDocumento,
                PossuiParcelas = true,
                RegistraFinanceiro = tipoDocumento.RegistraFinanceiro
            };

            foreach (var p in parcelas)
            {
                p.AnexarPagamento(e);
                e.Valor += p.Valor;
                e._parcelas.Add(p);
            }

            return e;
        }

        public static FPagamento CriarNoCartaoCredito(UsuarioDTO usuario, decimal valor)
        {
            var e = new FPagamento
            {
                CriadoPor = usuario,
                CriadoEm = DateTime.Now,
                Especie = ETipoPagamento.CartaoCredito,
                Valor = valor
            };

            return e;
        }

        public static FPagamento CriarNoCartaoDebito(UsuarioDTO usuario, decimal valor)
        {
            var e = new FPagamento
            {
                CriadoPor = usuario,
                CriadoEm = DateTime.Now,
                Especie = ETipoPagamento.CartaoDebito,
                Valor = valor
            };

            return e;
        }

        public static FPagamento CriarNoPix(UsuarioDTO usuario, decimal valor)
        {
            var e = new FPagamento
            {
                CriadoPor = usuario,
                CriadoEm = DateTime.Now,
                Especie = ETipoPagamento.Pix,
                Valor = valor
            };

            return e;
        }

        public override string ToString()
        {
            var especieText = Especie.GetDescription();

            return Especie == ETipoPagamento.CreditoLoja
                ? $"{especieText} {_parcelas.Count}x"
                : especieText;
        }

        public void AnexarVenda(FaturamentoVenda faturamento)
        {
            if (Faturamento != null)
            {
                throw new InvalidOperationException("Pagamento já possui uma Venda Anexada!");
            }

            Faturamento = faturamento;
        }

        public Malote CriaMalote(
            EmpresaDTO empresa,
            Cliente cliente,
            OrigemDocumento origem,
            Guid documentoGid,
            UsuarioDTO usuario
        )
        {
            if (!PossuiParcelas)
            {
                throw new InvalidOperationException("Pagamento não possui parcelas para geração de malote");
            }

            if (RegistraFinanceiro != true)
            {
                throw new InvalidOperationException("Não foi possível criar malote. Documento não gera financeiro!");
            }

            var malote = Malote.Cria(origem, documentoGid.ToString(), usuario, 0);

            foreach (var p in Parcelas)
            {
                var doc = new DocumentoReceber
                {
                    Cliente = cliente,
                    EmitidoEm = DateTime.Now,
                    Empresa = empresa,
                    Malote = malote,
                    NumeroDocumento = string.Empty,
                    Situacao = Situacao.Aberto,
                    Parcela = (byte) p.Numero,
                    TipoDocumento = TipoDocumento,
                    ValorDocumento = p.Valor,
                    ValorOriginal = p.Valor,
                    Vencimento = p.Vencimento,
                    UsuarioCriacao = usuario
                };

                malote.DocumentosReceber.Add(doc);
            }

            return malote;
        }
    }
}