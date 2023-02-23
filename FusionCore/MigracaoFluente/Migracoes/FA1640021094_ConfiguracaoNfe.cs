using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1640021094)]
    public class FA1640021094_ConfiguracaoNfe : Migration
    {
        public override void Up()
        {
            Create.Table("configuracao_nfe")
                .WithColumn("id").AsGuid().PrimaryKey("pk_configuracao_nfe")
                .WithColumn("mudarUrlCeara").AsBoolean();
        }

        public override void Down()
        {
        }
    }
}