using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1664157040)]
    public class FA1664157040_AddSolicitarPesoUnidade : Migration
    {
        public override void Up()
        {
            Create.Column("solicitarPeso")
                .OnTable("produto_unidade")
                .AsBoolean()
                .NotNullable()
                .SetExistingRowsTo(false);
        }

        public override void Down()
        {
            Delete.Column("solicitarPeso").FromTable("produto_unidade");
        }
    }
}