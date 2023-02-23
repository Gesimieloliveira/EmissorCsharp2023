using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1666642350)]
    public class FA1666642350_AtualizacaoDasViewsDeVendas : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript(ScriptHelper.CriarPath("criar_view_vendas_pagamentos.sql"));
            Execute.EmbeddedScript(ScriptHelper.CriarPath("update_4_view_vendas.sql"));
            Execute.EmbeddedScript(ScriptHelper.CriarPath("update_4_view_vendas_com_itens.sql"));
        }

        public override void Down()
        {
            
        }
    }
}