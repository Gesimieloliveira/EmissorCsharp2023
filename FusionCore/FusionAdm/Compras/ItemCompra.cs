using System;
using System.Diagnostics.CodeAnalysis;
using FusionCore.Core.Estoque;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using FusionCore.Tributacoes.Calculadoras;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedAutoPropertyAccessor.
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.Compras
{
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public sealed class ItemCompra : Entidade, IMovimentavel
    {
        private ItemCompra()
        {
            // nhibernate
            CadastroEm = DateTime.Now;
            MovimentaEstoque = true;
        }

        public ItemCompra(NotaFiscalCompra nota) : this()
        {
            Nota = nota;
        }

        public int Id { get; set; }
        public NotaFiscalCompra Nota { get; private set; }
        public ProdutoDTO Produto { get; set; }
        public CfopDTO Cfop { get; private set; }
        public NcmDTO Ncm { get; set; }
        public string Cest { get; set; }
        public ProdutoUnidadeDTO Unidade { get; set; }
        public decimal Quantidade { get; set; }
        public bool MovimentaEstoque { get; private set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorDescontoTotal { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorFreteRateio { get; set; }
        public decimal ValorSeguroRateio { get; set; }
        public decimal ValorDespesasRateio { get; set; }
        public decimal ValorTotalCusto { get; private set; }
        public bool FreteCompoeIcms { get; set; }
        public bool DespesasCompoeIcms { get; set; }
        public bool IpiCompoeIcms { get; set; }
        public bool SeguroCompoeIcms { get; set; }
        public decimal FatorConversao { get; private set; }
        public string UnidadeConversao { get; private set; }
        public decimal QuantidadeConversao { get; private set; }
        public DateTime CadastroEm { get; private set; }
        public bool ImportadoDeXml { get; set; }
        public bool ImpostoManual { get; set; }
        public IpiCompra Ipi { get; set; }
        public PisCompra Pis { get; set; }
        public CofinsCompra Cofins { get; set; }
        public IcmsCompra Icms { get; set; }
        protected override int ReferenciaUnica => Id;

        public decimal CalculoPrecoCompraUnitario => decimal.Round(ValorTotal / QuantidadeConversao, 4);
        public decimal CalculoPrecoCustoUnitario => decimal.Round(ValorTotalCusto / QuantidadeConversao, 4);
        public decimal ValorTotalBruto => ValorTotal + ValorDescontoTotal;

        public EstoqueModel CriaMovimentoInclusao()
        {
            var model = new EstoqueModel(
                Produto,
                QuantidadeConversao,
                SessaoEstoque.UsuarioEvento,
                OrigemEventoEstoque.ItemAdicionadoCompra
            );

            return model;
        }

        public EstoqueModel CriaMovimentoRemocao()
        {
            var model = new EstoqueModel(
                Produto,
                QuantidadeConversao,
                SessaoEstoque.UsuarioEvento,
                OrigemEventoEstoque.ItemRemovidoCompra
            );

            return model;
        }

        public void RecalculaImpostos()
        {
            if (ImpostoManual)
            {
                return;
            }

            ReprocessaIpi();
            ReprocessaIcms();
            RecalculaPis();
            RecalculaCofins();
        }

        public void SetCfop(CfopDTO cfop)
        {
            Cfop = cfop;
        }

        public void SetConversao(ConversaoUnidade conversao)
        {
            FatorConversao = conversao.Fator;
            UnidadeConversao = conversao.Unidade;
            QuantidadeConversao = conversao.Quantidade;
        }

        private void ReprocessaIpi()
        {
            if (Ipi == null)
            {
                return;
            }

            var calc = new CalculadoraIpi
            {
                Aliquota = Ipi.Aliquota,
                ValorTributavel = ValorTotal
            };

            var res = calc.Calcula();

            Ipi.BaseCalculo = res.Bc;
            Ipi.ValorIpi = res.Valor;
        }

        private void ReprocessaIcms()
        {
            if (Icms == null)
            {
                return;
            }

            var valorIpi = Ipi?.ValorIpi ?? 0.00m;

            // Calculo ICMS
            {
                var calc = new CalculadoraIcms
                {
                    Aliquota = Icms.Aliquota,
                    ValorTributavel = ValorTotal,
                    Reducao = Icms.Reducao,
                    ValorIpi = IpiCompoeIcms ? valorIpi : 0,
                    ValorFrete = FreteCompoeIcms ? ValorFreteRateio : 0,
                    ValorSeguro = SeguroCompoeIcms ? ValorSeguroRateio : 0,
                    ValorOutros = DespesasCompoeIcms ? ValorDespesasRateio : 0,
                };

                var res = calc.Calcula();

                Icms.BaseCalculo = res.Bc;
                Icms.ValorIcms = res.Valor;
            }

            // Calculo da ST
            {
                var calc = new CalculadoraIcmsSt
                {
                    //TODO: Melhor cálculo de ICMS ST para ReprocessaIcms - ItemCompra
                    Aliquota = Icms.AliquotaSt,
                    ValorTributavel = ValorTotal,
                    Reducao = Icms.ReducaoSt,
                    Mva = Icms.Mva,
                    ValorIpi = IpiCompoeIcms ? valorIpi : 0,
                    ValorFrete = FreteCompoeIcms ? ValorFreteRateio : 0,
                    ValorSeguro = SeguroCompoeIcms ? ValorSeguroRateio : 0,
                    ValorOutros = DespesasCompoeIcms ? ValorDespesasRateio : 0,
                };

                var res = calc.Calcula();

                Icms.BaseCalculoSt = res.Bc;
                Icms.ValorSt = res.Valor;
            }
        }

        private void RecalculaPis()
        {
            if (Pis == null)
            {
                return;
            }

            // calculo pis
            {
                var calc = new CalculadoraPis
                {
                    Aliquota = Pis.Aliquota,
                    ValorTributavel = ValorTotal
                };

                var res = calc.Calcula();

                Pis.ValorPis = res.Valor;
                Pis.BaseCalculo = res.Bc;
            }
        }

        private void RecalculaCofins()
        {
            if (Cofins == null)
                return;

            // calculo cofins
            {
                var calc = new CalculadoraCofins
                {
                    Aliquota = Cofins.Aliquota,
                    ValorTributavel = ValorTotal
                };

                var res = calc.Calcula();

                Cofins.ValorCofins = res.Valor;
                Cofins.BaseCalculo = res.Bc;
            }
        }

        public void CalculaPrecoCusto()
        {
            ValorTotalCusto =
                ValorTotal +
                ValorDespesasRateio +
                ValorFreteRateio +
                ValorSeguroRateio +
                (Ipi?.ValorIpi ?? 0) +
                (Icms?.ValorSt ?? 0) +
                (Icms?.ValorFcpSt ?? 0);
        }
    }
}