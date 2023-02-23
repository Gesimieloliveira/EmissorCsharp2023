using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1602962822)]
    public class FA1602962822_PadronizacaoDasViewsDeVendas : Migration
    {
        public override void Up()
        {
            Execute.Sql("exec sp_drop_view_if_exists 'dbo', 'view_vendas';");
            Execute.Sql("exec sp_drop_view_if_exists 'dbo', 'view_vendas_com_itens';");
            Execute.Sql("exec sp_drop_view_if_exists 'dbo', 'view_vendas_fiscais';");
            Execute.Sql("exec sp_drop_view_if_exists 'dbo', 'view_vendas_fiscais_com_itens';");

            Execute.EmbeddedScript(ScriptHelper.CriarPath("update_3_view_vendas.sql"));
            Execute.EmbeddedScript(ScriptHelper.CriarPath("update_3_view_vendas_com_itens.sql"));
        }

        public override void Down()
        {
        }
    }
}