using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1607609606)]
    public class FA1607609606_AdicionarCredenciadoraEmPosParaMFe : Migration
    {
        public override void Up()
        {
            Alter.Table("pos").AddColumn("credenciadora")
                .AsInt16().Nullable();
        }

        public override void Down()
        {
        }
    }
}