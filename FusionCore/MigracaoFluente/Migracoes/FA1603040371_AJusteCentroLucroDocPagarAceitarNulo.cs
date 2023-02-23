using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1603040371)]
    public class FA1603040371_AJusteCentroLucroDocPagarAceitarNulo : Migration
    {
        public override void Up()
        {
            Alter.Column("centroCusto_id")
                .OnTable("documento_pagar").AsInt16().Nullable();
        }

        public override void Down()
        {
        }
    }
}