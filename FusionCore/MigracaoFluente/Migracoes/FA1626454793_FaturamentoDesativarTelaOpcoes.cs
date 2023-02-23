using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1626454793)]
    public class FA1626454793_FaturamentoDesativarTelaOpcoes : Migration
    {
        public override void Up()
        {
            Alter.Table("faturamento_preferencia")
                .AddColumn("desabilitarTelaOpcoes").AsBoolean().NotNullable().SetExistingRowsTo(false);
        }

        public override void Down()
        {
            
        }
    }
}