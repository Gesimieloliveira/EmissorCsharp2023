using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1570742195)]
    public class FA1570742195_CorrecaoAnaliseDeLucroUserStory193 : Migration
    {
        public override void Up()
        {
            AjusteEstruturaFaturamento();
            AjusteEstruturaItensNfe();
        }

        private void AjusteEstruturaItensNfe()
        {
            Alter.Table("nfe_item")
                .AddColumn("produto_id").AsInt32().NotNullable().SetExistingRowsTo(0)
                .AddColumn("quantidade").AsDecimal(15, 4).NotNullable().SetExistingRowsTo(0)
                .AddColumn("siglaUnidade").AsAnsiString(10).NotNullable().SetExistingRowsTo("")
                .AddColumn("siglaUnidadeTributavel").AsAnsiString(10).NotNullable().SetExistingRowsTo("")
                .AddColumn("codigoUtilizado").AsAnsiString(60).NotNullable().SetExistingRowsTo("")
                .AddColumn("codigoBarras").AsAnsiString(25).NotNullable().SetExistingRowsTo("")
                .AddColumn("valorUnitario").AsDecimal(21, 10).NotNullable().SetExistingRowsTo(0)
                .AddColumn("totalBruto").AsDecimal(15, 2).NotNullable().SetExistingRowsTo(0)
                .AddColumn("porcentagemDescontoItem").AsDecimal(21, 10).NotNullable().SetExistingRowsTo(0)
                .AddColumn("totalDescontoItem").AsDecimal(15, 2).NotNullable().SetExistingRowsTo(0)
                .AddColumn("totalItem").AsDecimal(15, 2).NotNullable().SetExistingRowsTo(0)
                .AddColumn("precoCusto").AsDecimal(15, 4).NotNullable().SetExistingRowsTo(0)
                .AddColumn("precoVenda").AsDecimal(15, 4).NotNullable().SetExistingRowsTo(0)
                .AddColumn("totalFiscal").AsDecimal(15, 2).NotNullable().SetExistingRowsTo(0);

            Execute.Sql(@"update i set 
	            i.produto_id = m.produto_id,
	            i.quantidade = m.quantidade,
	            i.siglaUnidade = m.siglaUnidade,
	            i.siglaUnidadeTributavel = m.siglaUnidadeTributavel,
	            i.codigoUtilizado = m.codigoUtilizado,
	            i.codigoBarras = m.codigoBarras,
	            i.valorUnitario = m.valorUnitario,
	            i.totalBruto = m.totalBruto,
	            i.porcentagemDescontoItem = m.porcentagemDesconto,
	            i.totalDescontoItem = m.totalDesconto,
	            i.totalItem = m.total,
	            i.precoCusto = m.precoCusto,
	            i.precoVenda = m.precoVenda,
	            i.totalFiscal = i.valorFiscal
            from nfe_item i
            inner join nfe_item_mercadoria m on i.id = m.nfeItem_id;");

            Create.ForeignKey("fk_nfe_item_to_produto")
                .FromTable("nfe_Item").ForeignColumn("produto_id")
                .ToTable("produto").PrimaryColumn("id");

            Delete.Column("valorFiscal").FromTable("nfe_item");
            Delete.Table("nfe_item_mercadoria");
        }

        private void AjusteEstruturaFaturamento()
        {
            Alter.Table("faturamento_produto")
                .AddColumn("totalDescontoFixo").AsDecimal(15, 2).NotNullable().SetExistingRowsTo(0);

            Execute.Sql(@"update p 
	            set p.totalDescontoFixo = (p.total * v.percentualDesconto / 100) 
	            from faturamento_produto p 
	            inner join faturamento_venda v on v.id = p.faturamentoVenda_id");
        }

        public override void Down()
        {
        }
    }
}