using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1590004167)]
    public class FN1590004167_CorrigeUnidadeTributavel : Migration
    {
        public override void Up()
        {
            Execute.Sql("update produto SET quantidadeUnidadeTributavel = 1 where quantidadeUnidadeTributavel = 0");
            Execute.Sql("update nfce_item SET quantidadeUnidadeTributavel = 1 where quantidadeUnidadeTributavel = 0");
        }

        public override void Down()
        {
        }
    }
}