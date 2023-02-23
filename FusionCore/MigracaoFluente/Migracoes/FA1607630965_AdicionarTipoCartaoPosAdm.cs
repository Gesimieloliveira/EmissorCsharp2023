using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1607630965)]
    public class FA1607630965_AdicionarTipoCartaoPosAdm : Migration
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