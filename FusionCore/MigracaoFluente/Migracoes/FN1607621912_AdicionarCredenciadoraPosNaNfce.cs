using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1607621912)]
    public class FN1607621912_AdicionarCredenciadoraPosNaNfce : Migration
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