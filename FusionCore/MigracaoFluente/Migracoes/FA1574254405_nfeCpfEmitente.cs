using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1574254405)]
    public class FA1574254405_nfeCpfEmitente : Migration
    {
        public override void Up()
        {
            Alter.Table("nfe")
                .AddColumn("cpfEmitente").AsAnsiString(11).NotNullable().WithDefaultValue(string.Empty);

            Delete.DefaultConstraint().OnTable("nfe").InSchema("dbo").OnColumn("cpfEmitente");
        }

        public override void Down()
        {
        }
    }
}