using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1622826524)]
    public class FA1622826524_UsarIpiTagPropriaItemNfe : Migration
    {
        public override void Up()
        {
            const string tabela = "nfe_item";
            const string usarIpiTagPropria = "usarIpiTagPropria";

            Alter.Table(tabela).AddColumn(usarIpiTagPropria).AsBoolean().WithDefaultValue(false).NotNullable();

            Delete.DefaultConstraint().OnTable(tabela).InSchema("dbo").OnColumn(usarIpiTagPropria);
        }

        public override void Down()
        {
        }
    }
}