using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1579096875)]
    public class FA1579096875_AjusteTaxaDeJurosConfiguracaoFinanceiro : Migration
    {
        public override void Up()
        {
            Alter.Table("configuracao_financeiro")
                .AddColumn("taxaDeJurosMensal").AsDecimal(6, 2).NotNullable().SetExistingRowsTo(0);
        }

        public override void Down()
        {
        }
    }
}