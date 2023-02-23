using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1575988290)]
    public class FN1575988290_AdicionaSerieENumeroNaNfce : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce").AddColumn("numeroFiscal").AsInt32().NotNullable().WithDefaultValue(0);
            Alter.Table("nfce").AddColumn("serie").AsInt16().NotNullable().WithDefaultValue(0);

            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("numeroFiscal");
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("serie");
        }

        public override void Down()
        {
        }
    }
}