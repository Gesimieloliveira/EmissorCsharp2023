using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1562083498)]
    public class FA1562083498_EstruturaConfiguracaoCaixa : Migration
    {
        public override void Up()
        {
            Create.Table("configuracao_controle_caixa")
                .WithColumn("id").AsGuid().PrimaryKey("pk_configuracao_controle_caixa")
                .WithColumn("controlaCaixaNoGestor").AsBoolean().NotNullable()
                .WithColumn("controlaCaixaNoNfce").AsBoolean().NotNullable();

            var row = new {id = Guid.Parse("D38B8AA2-7FD1-4FE0-ACB6-4E067487F62C"), controlaCaixaNoGestor = false, controlaCaixaNoNfce = false};

            Insert.IntoTable("configuracao_controle_caixa").Row(row);
        }

        public override void Down()
        {
        }
    }
}