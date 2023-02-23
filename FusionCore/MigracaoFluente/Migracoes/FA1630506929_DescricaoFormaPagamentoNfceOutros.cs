using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1630506929)]
    public class FA1630506929_DescricaoFormaPagamentoNfceOutros : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce_forma_pagamento")
                .AddColumn("descricaoOutros").AsAnsiString(60).NotNullable().SetExistingRowsTo(string.Empty);
        }

        public override void Down()
        {
        }
    }
}