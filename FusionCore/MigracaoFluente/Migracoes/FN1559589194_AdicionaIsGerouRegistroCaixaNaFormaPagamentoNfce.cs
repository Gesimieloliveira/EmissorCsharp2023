using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1559589194)]
    public class FN1559589194_AdicionaIsGerouRegistroCaixaNaFormaPagamentoNfce : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce_forma_pagamento").AddColumn("isGerouRegistroCaixa").AsBoolean().NotNullable().WithDefaultValue(false);

            Delete
                .DefaultConstraint()
                .OnTable("nfce_forma_pagamento")
                .InSchema("dbo")
                .OnColumn("isGerouRegistroCaixa");
        }

        public override void Down()
        {
        }
    }
}