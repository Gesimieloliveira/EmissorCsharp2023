using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1607624057)]
    public class FN1607624057_AdicionarCredenciadoraFormaPagamentoNfce : Migration
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