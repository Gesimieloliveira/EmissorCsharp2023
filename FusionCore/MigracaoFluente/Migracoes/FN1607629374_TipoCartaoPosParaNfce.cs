using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1607629374)]
    public class FN1607629374_TipoCartaoPosParaNfce : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce_forma_pagamento")
                .AddColumn("tipoCartaoPos").AsByte().Nullable();
        }

        public override void Down()
        {
        }
    }
}