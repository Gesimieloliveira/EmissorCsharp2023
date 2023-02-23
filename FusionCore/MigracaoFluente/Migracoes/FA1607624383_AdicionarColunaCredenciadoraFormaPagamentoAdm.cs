using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1607624383)]
    public class FA1607624383_AdicionarColunaCredenciadoraFormaPagamentoAdm : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce_forma_pagamento").AddColumn("credenciadora").AsInt16().Nullable();
        }

        public override void Down()
        {
        }
    }
}