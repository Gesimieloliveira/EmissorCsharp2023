using System;
using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.FastReport.DataSources
{
    public class DsFaturamento
    {
        public DsFaturamento()
        {
            TelefonesDestinatario = new List<DsTelefone>();
        }

        public int Id { get; set; }
        public DateTime FinalizadoEm { get; set; }
        public int ClienteId { get; set; }
        public string NomeCliente { get; set; }
        public string NomeVendedor { get; set; }
        public string CpfCliente { get; set; }
        public string CnpjCliente { get; set; }
        public string LogradouroCliente { get; set; }
        public string NumeroCliente { get; set; }
        public string BairroCliente { get; set; }
        public CidadeDTO CidadeCliente { get; set; }
        public int EmpresaId { get; set; }
        public string NomeEmpresa { get; set; }
        public string LogradouroEmpresa { get; set; }
        public string NumeroEmpresa { get; set; }
        public string BairroEmpresa { get; set; }
        public CidadeDTO CidadeEmpresa { get; set; }
        public string CnpjEmpresa { get; set; }
        public string IeEmpresa { get; set; }
        public byte[] Logo { get; set; }
        public string Observacao { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal TotalDesconto { get; set; }
        public decimal Total { get; set; }
        public decimal Troco { get; set; }
        public string EnderecoEmpresa => $"{LogradouroEmpresa}, {NumeroEmpresa}, {BairroEmpresa}, {CidadeEmpresa}";
        public string DocumentoCliente => !string.IsNullOrWhiteSpace(CpfCliente) ? CpfCliente : CnpjCliente;
        public string EnderecoCliente => $"{LogradouroCliente}, {NumeroCliente}, {BairroCliente}, {CidadeCliente}";
        public IList<DsPagamento> Pagamentos { get; set; }
        public IList<DsTelefone> TelefonesDestinatario { get; set; }
    }
}