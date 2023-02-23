using System;
using FusionCore.CadastroEmpresa;
using FusionCore.CadastroUsuario;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Vendas.Faturamentos;
using PessoaVendedor = FusionCore.FusionAdm.Pessoas.Vendedor;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Models
{
    public static class FaturamentoCheckoutModels
    {
        public class Vendedor
        {
            public Vendedor(PessoaVendedor vendedor = null)
            {
                Id = vendedor?.Id;
                Nome = vendedor is null
                    ? null
                    : $"{vendedor.Id:D3} - {vendedor.Nome}";
            }

            public int? Id { get; set; }
            public string Nome { get; set; }
        }

        public class Faturamento
        {
            public Faturamento(FaturamentoVenda faturamento = null)
            {
                Numero = faturamento?.Id;
                DataCriacao = faturamento?.CriadoEm;
                UsuarioCriou = faturamento?.CriadoPor.Login;
                NomeCliente = faturamento?.Destinatario?.Cliente.Nome;
                Total = faturamento?.Total ?? 0M;
            }

            public int? Numero { get; set; }
            public DateTime? DataCriacao { get; set; }
            public string UsuarioCriou { get; set; }
            public string NomeCliente { get; set; }
            public decimal Total { get; set; }
        }

        public class Item
        {
            public Item(FaturamentoProduto item)
            {
                Id = item.Id;
                Numero = item.Numero;
                ProdutoId = item.Produto.Id;
                Descricao = item.Produto.Nome;
                UnidadeMedida = item.SiglaUnidade;
                Quantidade = item.Quantidade;
                PrecoUnitario = item.PrecoUnitario;
                Total = item.Total;
            }

            public int Id { get; set; }
            public int Numero { get; set; }
            public int ProdutoId { get; set; }
            public string Descricao { get; set; }
            public string UnidadeMedida { get; set; }
            public decimal Quantidade { get; set; }
            public string QuantidadeMedida => $"{Quantidade:N3} {UnidadeMedida}";
            public decimal PrecoUnitario { get; set; }
            public decimal Total { get; set; }
        }

        public class Empresa
        {
            public Empresa(IEmpresa empresa)
            {
                Id = empresa.Id;
                Nome = empresa.RazaoSocial;
            }

            public int Id { get; set; }
            public string Nome { get; set; }
        }

        public class Usuario
        {
            public Usuario(IUsuario usuario)
            {
                Id = usuario.Id;
                Nome = usuario.Login;
            }

            public int Id { get; set; }
            public string Nome { get; set; }
        }

        public class TabelaPreco
        {
            public TabelaPreco(ITabelaPreco tabela = null)
            {
                Id = tabela?.Id;
                Descricao = tabela?.Descricao;
            }

            public int? Id { get; set; }
            public string Descricao { get; set; }
        }
    }
}