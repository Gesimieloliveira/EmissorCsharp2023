using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1615467651)]
    public class FA1615467651_AdicionarAmbienteSefazCupomFiscalHistorico : Migration
    {
        public override void Up()
        {
            Alter.Table("cupom_fiscal_historico")
                .AddColumn("ambienteSefaz").AsByte().NotNullable();
        }

        public override void Down()
        {
        }
    }
}