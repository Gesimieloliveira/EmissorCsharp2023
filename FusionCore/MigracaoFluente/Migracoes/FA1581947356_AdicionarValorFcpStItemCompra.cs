using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1581947356)]
    public class FA1581947356_AdicionarValorFcpStItemCompra : Migration
    {
        public override void Up()
        {
            Alter.Table("nf_compra_item_icms")
                .AddColumn("valorFcpSt").AsDecimal(15, 2).NotNullable().WithDefaultValue(0);
            Alter.Table("nf_compra_item_icms")
                .AddColumn("bcFcpSt").AsDecimal(15, 2).NotNullable().WithDefaultValue(0);
            Alter.Table("nf_compra_item_icms")
                .AddColumn("percentualFcpSt").AsDecimal(15, 2).NotNullable().WithDefaultValue(0);


            Delete.DefaultConstraint().OnTable("nf_compra_item_icms")
                .InSchema("dbo").OnColumn("valorFcpSt");
            Delete.DefaultConstraint().OnTable("nf_compra_item_icms")
                .InSchema("dbo").OnColumn("bcFcpSt");
            Delete.DefaultConstraint().OnTable("nf_compra_item_icms")
                .InSchema("dbo").OnColumn("percentualFcpSt");
        }

        public override void Down()
        {
        }
    }
}