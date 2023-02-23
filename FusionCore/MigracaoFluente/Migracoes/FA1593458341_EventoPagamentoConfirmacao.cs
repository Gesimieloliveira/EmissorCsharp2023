using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1593458341)]
    public class FA1593458341_EventoPagamentoConfirmacao : Migration
    {
        public override void Up()
        {
            Alter.Table("mdfe_evento_pagamento")
                .AddColumn("autorizado").AsBoolean().NotNullable().WithDefaultValue(false);

            Alter.Table("mdfe_evento_pagamento")
                .AddColumn("xmlRetorno").AsCustom("xml").Nullable();

            Delete.DefaultConstraint().OnTable("mdfe_evento_pagamento")
                .InSchema("dbo").OnColumn("autorizado");
        }

        public override void Down()
        {
        }
    }
}