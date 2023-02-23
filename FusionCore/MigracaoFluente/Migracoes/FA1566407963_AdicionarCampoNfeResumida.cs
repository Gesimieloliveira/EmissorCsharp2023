using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1566407963)]
    public class FA1566407963_AdicionarCampoNfeResumida : Migration
    {
        public override void Up()
        {
            Alter.Table("mde_resumo")
                .AddColumn("statusManifestacao").AsByte().NotNullable();
        }

        public override void Down()
        {
        }
    }
}