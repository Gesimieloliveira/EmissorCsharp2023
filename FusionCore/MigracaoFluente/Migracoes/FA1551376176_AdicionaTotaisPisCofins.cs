using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1551376176)]
    public class FA1551376176_AdicionaTotaisPisCofins : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce").AddColumn("totalBaseCalculoCofins").AsDecimal(15, 2).WithDefaultValue(0.0m);
            Alter.Table("nfce").AddColumn("totalCofins").AsDecimal(15, 2).WithDefaultValue(0.0m);
            Alter.Table("nfce").AddColumn("totalBaseCalculoPis").AsDecimal(15, 2).WithDefaultValue(0.0m);
            Alter.Table("nfce").AddColumn("totalPis").AsDecimal(15, 2).WithDefaultValue(0.0m);

            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("totalBaseCalculoCofins");
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("totalCofins");
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("totalBaseCalculoPis");
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("totalPis");
        }

        public override void Down()
        {
        }
    }
}