using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1599442667)]
    public class FA1599442667_CriarViewsParaAnaliseDeVendas : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript(ScriptHelper.CriarPath("criar_sp_drop_view_if_exists.sql"));

            Execute.EmbeddedScript(ScriptHelper.CriarPath("criar_view_vendas.sql"));
            Execute.EmbeddedScript(ScriptHelper.CriarPath("criar_view_vendas_com_itens.sql"));
        }

        public override void Down()
        {
        }
    }
}