using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1590003866)]
    public class FA1590003866_CorrigeUnidadeTributavel : Migration
    {
        public override void Up()
        {
            Execute.Sql("update produto SET quantidadeUnidadeTributavel = 1 where quantidadeUnidadeTributavel = 0");
            Execute.Sql("update nfe_item SET quantidadeUnidadeTributavel = 1 where quantidadeUnidadeTributavel = 0");
        }

        public override void Down()
        {
        }
    }
}