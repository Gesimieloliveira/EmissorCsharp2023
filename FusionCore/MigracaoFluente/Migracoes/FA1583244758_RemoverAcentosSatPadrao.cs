using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1583244758)]
    public class FA1583244758_RemoverAcentosSatPadrao : Migration
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