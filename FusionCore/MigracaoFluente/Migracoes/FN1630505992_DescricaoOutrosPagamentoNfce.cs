using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1630505992)]
    public class FN1630505992_DescricaoOutrosPagamentoNfce : Migration
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