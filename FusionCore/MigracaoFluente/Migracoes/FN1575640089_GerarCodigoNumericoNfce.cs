using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1575640089)]
    public class FN1575640089_GerarCodigoNumericoNfce : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce").AddColumn("codigoNumerico").AsInt32().NotNullable().WithDefaultValue(0);
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("codigoNumerico");
        }

        public override void Down()
        {
        }
    }
}