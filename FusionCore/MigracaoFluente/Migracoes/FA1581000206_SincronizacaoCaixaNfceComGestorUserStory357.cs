using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1581000206)]
    public class FA1581000206_SincronizacaoCaixaNfceComGestorUserStory357 : Migration
    {
        public override void Up()
        {
            Alter.Table("conta_caixa_fluxo")
                .AddColumn("origemEvento").AsInt16().NotNullable()
                .SetExistingRowsTo(0);

            Execute.Sql("update conta_caixa_fluxo set origemEvento = 1 where substring(historico, 1, 8) = 'abertura'");
            Execute.Sql("update conta_caixa_fluxo set origemEvento = 2 where substring(historico, 1, 8) = 'fechamen'");
        }

        public override void Down()
        {
        }
    }
}