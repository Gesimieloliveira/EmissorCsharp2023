using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1576015817)]
    public class FN1576015817_FusionNfceTipoEmissao : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce").AddColumn("tipoEmissao").AsByte().NotNullable().WithDefaultValue(1);

            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("tipoEmissao");
        }

        public override void Down()
        {
        }
    }
}