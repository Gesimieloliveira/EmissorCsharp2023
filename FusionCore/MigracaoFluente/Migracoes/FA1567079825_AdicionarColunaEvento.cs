using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1567079825)]
    public class FA1567079825_AdicionarColunaEvento : Migration
    {
        public override void Up()
        {
            Alter.Table("mde_evento_manifestacao")
                .AddColumn("evento").AsByte().NotNullable();
        }

        public override void Down()
        {
        }
    }
}