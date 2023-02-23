using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Criptografia;
using NFe.Utils.NFe;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Compras
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class NotaFiscalCompra : Entidade
    {
        public static class Expressions
        {
            public static readonly Expression<Func<NotaFiscalCompra, object>> Itens = x => x._itens;
        }

        private readonly IList<ItemCompra> _itens = new List<ItemCompra>();
        private readonly IList<DuplicataCompra> _duplicatas = new List<DuplicataCompra>();


        private NotaFiscalCompra()
        {
            //nhibernate
            CadastradoEm = DateTime.Now;
            ImportadoDeXml = false;
            Xml = string.Empty;
            Uuid = GuuidHelper.Computar(nameof(NotaFiscalCompra) + DateTime.Now.ToLongDateString());
        }

        public NotaFiscalCompra(EmpresaDTO empresa) : this()
        {
            Empresa = empresa;
        }

        public int Id { get; set; }
        public int NumeroDocumento { get; set; }
        public short Serie { get; set; }
        public ChaveSefaz Chave { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public Transportadora Transportadora { get; set; }
        public ModalidadeFrete ModalidadeFrete { get; set; }
        public DateTime EmitidaEm { get; set; }
        public DateTime EntradaSaidaEm { get; set; }
        public DateTime CadastradoEm { get; private set; }
        public decimal TotalBcIcms { get; set; }
        public decimal ValorTotalIcms { get; set; }
        public decimal TotalBcIcmsSt { get; set; }
        public decimal ValorTotalIcmsSt { get; set; }
        public decimal ValorTotalItens { get; set; }
        public decimal ValorTotalIpi { get; set; }
        public decimal ValorTotalFrete { get; set; }
        public decimal ValorTotalSeguro { get; set; }
        public decimal ValorTotalDesconto { get; set; }
        public decimal ValorTotalOutros { get; set; }
        public decimal ValorTotal { get; set; }
        public bool ImportadoDeXml { get; private set; }
        public string Xml { get; private set; }
        public string Uuid { get; set; }
        public IEnumerable<ItemCompra> Itens => _itens.AsEnumerable();
        public IEnumerable<DuplicataCompra> Duplicatas => _duplicatas.AsEnumerable();
        protected override int ReferenciaUnica => Id;
        public int ContaOsItens => _itens.Count;
        public bool PossuiChave => !string.IsNullOrWhiteSpace(Chave.Chave);
        public Malote Malote { get; set; }

        public void InicializaLazy()
        {
            NHibernateUtil.Initialize(_itens);
            NHibernateUtil.Initialize(_duplicatas);
        }

        public void Adicionar(ItemCompra item)
        {
            _itens.Add(item);
        }

        public void Remover(ItemCompra item)
        {
            _itens.Remove(item);
        }

        public void Adicionar(DuplicataCompra duplicata)
        {
            _duplicatas.Add(duplicata);
        }

        public void Remover(DuplicataCompra duplicata)
        {
            _duplicatas.Remove(duplicata);
        }

        public void CalculaTotais()
        {
            CalculaCustosItens();

            CalculaImpostosDosItens();
            CalculaPrecoCustoDosItens();

            var totalFcpSt = _itens.Sum(i => i.Icms.ValorFcpSt);
            TotalBcIcms = _itens.Sum(i => i.Icms.BaseCalculo);
            ValorTotalIcms = _itens.Sum(i => i.Icms.ValorIcms);
            TotalBcIcmsSt = _itens.Sum(i => i.Icms.BaseCalculoSt);
            ValorTotalIcmsSt = _itens.Sum(i => i.Icms.ValorSt);
            ValorTotalIpi = _itens.Sum(i => i.Ipi.ValorIpi);
            ValorTotalDesconto = _itens.Sum(i => i.ValorDescontoTotal);
            ValorTotalItens = _itens.Sum(i => i.ValorTotalBruto);

            var totalImpostos = ValorTotalIcmsSt + ValorTotalIpi + totalFcpSt;
            var totalDespesas = ValorTotalFrete + ValorTotalOutros + ValorTotalSeguro;

            ValorTotal = ValorTotalItens + totalDespesas + totalImpostos - ValorTotalDesconto;
        }

        private void CalculaCustosItens()
        {
            ValorTotalOutros = _itens.Sum(i => i.ValorDespesasRateio);
            ValorTotalFrete = _itens.Sum(i => i.ValorFreteRateio);
            ValorTotalSeguro = _itens.Sum(i => i.ValorSeguroRateio);
        }

        public void RateiaOsCustosNosItens(decimal valorFrete, decimal valorSeguro, decimal valorDespesas)
        {
            var ratearFrete = ValorTotalFrete != valorFrete;
            var ratearSeguro = ValorTotalSeguro != valorSeguro;
            var ratearDespesas = ValorTotalOutros != valorDespesas;

            if (ratearFrete)
                ValorTotalFrete = valorFrete;

            if (ratearSeguro)
                ValorTotalSeguro = valorSeguro;

            if (ratearDespesas)
                ValorTotalOutros = valorDespesas;

            var totalItens = _itens.Sum(i => i.ValorTotal);

            var razaoSeguro = 0.00M;
            var razaoDespesas = 0.00M;
            var razaoFrete = 0.00M;

            if (totalItens > 0)
            {
                if (ratearSeguro)
                    razaoSeguro = ValorTotalSeguro / totalItens;

                if (ratearDespesas)
                    razaoDespesas = ValorTotalOutros / totalItens;

                if (ratearFrete)
                    razaoFrete = ValorTotalFrete / totalItens;
            }

            _itens.ForEach(i =>
            {
                if (ratearDespesas)
                    i.ValorDespesasRateio = decimal.Round(i.ValorTotal * razaoDespesas, 2);

                if (ratearSeguro)
                    i.ValorSeguroRateio = decimal.Round(i.ValorTotal * razaoSeguro, 2);

                if (ratearFrete)
                    i.ValorFreteRateio = decimal.Round(i.ValorTotal * razaoFrete, 2);
            });

            DistribuirRestanteUltimoItem(ratearFrete, ratearSeguro, ratearDespesas);
        }

        private void CalculaImpostosDosItens()
        {
            _itens.ForEach(i => i.RecalculaImpostos());
        }

        private void DistribuirRestanteUltimoItem(bool ratearFrete, bool ratearSeguro, bool ratearDespesas)
        {
            if (_itens == null || _itens.Count == 0)
            {
                return;
            }

            var ultimoItem = _itens.Last();

            var somaFrete = _itens.Sum(i => i.ValorFreteRateio);
            var somaSeguro = _itens.Sum(i => i.ValorSeguroRateio);
            var somaDespesas = _itens.Sum(i => i.ValorDespesasRateio);

            if (ratearFrete)
                ultimoItem.ValorFreteRateio += ValorTotalFrete - somaFrete;

            if (ratearSeguro)
                ultimoItem.ValorSeguroRateio += ValorTotalSeguro - somaSeguro;

            if (ratearDespesas)
                ultimoItem.ValorDespesasRateio += ValorTotalOutros - somaDespesas;
        }

        public void CalculaPrecoCustoDosItens()
        {
            _itens.ForEach(i => i.CalculaPrecoCusto());
        }

        public void InformarImportacaoXml(string stringXml)
        {
            Xml = NormalizaXmlParaPersistir(stringXml);
            ImportadoDeXml = true;
        }

        private static string NormalizaXmlParaPersistir(string input)
        {
            var xml = input;

            var nfeZeus = new NFe.Classes.NFe().CarregarDeXmlString(xml);

            return nfeZeus.ObterXmlString();
        }

        public bool PossuiFinanceiro()
        {
            return Malote != null;
        }

        public void ExisteItemInativoThrow()
        {
            if (Itens.Any(itemCompra => itemCompra.Produto.Ativo == false))
            {
                throw new InvalidOperationException(
                    "Existem produtos inativos nessa nota de compra.\n" +
                    "Produtos Inativos estão com as linhas de vermelho.");
            }
        }

        public bool XMLPossuiCobrancas()
        {
            return Xml != null && Xml.Contains("<cobr>") && Xml.Contains("</cobr>");
        }
    }
}