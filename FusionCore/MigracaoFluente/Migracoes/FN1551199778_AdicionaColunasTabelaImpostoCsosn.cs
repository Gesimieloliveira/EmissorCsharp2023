using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1551199778)]
    public class FN1551199778_AdicionaColunasTabelaImpostoCsosn : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce_item_icms").AddColumn("aliquotaIcms").AsDecimal(15,4).WithDefaultValue(0);
            Alter.Table("nfce_item_icms").AddColumn("reducaoBcIcms").AsDecimal(15,4).WithDefaultValue(0);
            Alter.Table("nfce_item_icms").AddColumn("valorBcIcms").AsDecimal(15,2).WithDefaultValue(0);
            Alter.Table("nfce_item_icms").AddColumn("valorIcms").AsDecimal(15,2).WithDefaultValue(0);

            Delete.DefaultConstraint().OnTable("nfce_item_icms").InSchema("dbo").OnColumn("aliquotaIcms");
            Delete.DefaultConstraint().OnTable("nfce_item_icms").InSchema("dbo").OnColumn("reducaoBcIcms");
            Delete.DefaultConstraint().OnTable("nfce_item_icms").InSchema("dbo").OnColumn("valorBcIcms");
            Delete.DefaultConstraint().OnTable("nfce_item_icms").InSchema("dbo").OnColumn("valorIcms");
        }

        public override void Down()
        {
        }
    }
}