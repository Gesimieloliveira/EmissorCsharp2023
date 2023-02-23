using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1583244884)]
    public class FN1583244884_RemoverAcentosSatPadrao : Migration
    {
        public override void Up()
        {
            Delete.Column("removerAcento").FromTable("emissor_fiscal_sat");
        }

        public override void Down()
        {
        }
    }
}