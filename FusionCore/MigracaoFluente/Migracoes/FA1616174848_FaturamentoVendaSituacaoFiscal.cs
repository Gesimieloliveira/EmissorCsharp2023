using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1616174848)]
    public class FA1616174848_FaturamentoVendaSituacaoFiscal : Migration
    {
        public override void Up()
        {
            Alter.Table("faturamento_venda")
                .AddColumn("situacaoFiscal").AsByte().WithDefaultValue(0).NotNullable();

            Delete.DefaultConstraint().OnTable("faturamento_venda").InSchema("dbo").OnColumn("situacaoFiscal");
        }

        public override void Down()
        {
        }
    }
}