using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1669751220)]
    public class FA1669751220_AtualizaViewsVendas : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript(ScriptHelper.CriarPath("update_1_view_vendas_pagamentos.sql"));
            Execute.EmbeddedScript(ScriptHelper.CriarPath("update_5_view_vendas.sql"));
            Execute.EmbeddedScript(ScriptHelper.CriarPath("update_5_view_vendas_com_itens.sql"));
        }

        public override void Down()
        {
           
        }
    }
}