using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1551471667)]
    public class FA1551471667_ConfiguracaoPartilhaIcmsPerfilNfe : Migration
    {
        public override void Up()
        {
            Alter.Table("perfil_nfe")
                .AddColumn("autoAtivarPartilhaIcms").AsBoolean().NotNullable().SetExistingRowsTo(true);
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}