using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1622825417)]
    public class FA1622825417_UsarIpiTagPropriaPerfilNfe : Migration
    {
        public override void Up()
        {
            const string tabela = "perfil_nfe";
            const string usarIpiTagPropria = "usarIpiTagPropria";

            Alter.Table(tabela).AddColumn(usarIpiTagPropria).AsBoolean().WithDefaultValue(false).NotNullable();

            Delete.DefaultConstraint().OnTable(tabela).InSchema("dbo").OnColumn(usarIpiTagPropria);
        }

        public override void Down()
        {
        }
    }
}