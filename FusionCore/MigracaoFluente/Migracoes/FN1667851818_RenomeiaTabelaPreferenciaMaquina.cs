using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1667851818)]
    public class FN1667851818_RenomeiaTabelaPreferenciaMaquina : Migration
    {
        public override void Up()
        {
            Rename.Table("preferencia_maquina").To("preferencia_sistema");
        }

        public override void Down()
        {
        }
    }
}