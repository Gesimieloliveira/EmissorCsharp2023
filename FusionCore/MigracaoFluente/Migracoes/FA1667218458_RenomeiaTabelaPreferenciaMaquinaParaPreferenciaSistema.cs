using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1667218458)]
    public class FA1667218458_RenomeiaTabelaPreferenciaMaquinaParaPreferenciaSistema : Migration
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