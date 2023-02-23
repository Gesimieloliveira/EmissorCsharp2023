using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1610234111)]
    public class FN1610234111_ConfiguracaoTabelaPrecoPadraoPreferenciasTerminal : Migration
    {
        public override void Up()
        {
            Create.Column("tabelaPrecoPadrao_id")
                .OnTable("preferencia_terminal")
                .AsInt32().Nullable();

            Create.Column("confirmacaoTabelaPadraoAntesVenda")
                .OnTable("preferencia_terminal")
                .AsBoolean()
                .NotNullable()
                .SetExistingRowsTo(false);
        }

        public override void Down()
        {
        }
    }
}