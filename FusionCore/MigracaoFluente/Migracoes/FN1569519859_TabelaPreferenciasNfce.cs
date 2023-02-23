using System;
using System.Collections.Generic;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1569519859)]
    public class FN1569519859_TabelaPreferenciasNfce : Migration
    {
        public override void Up()
        {
            Create.Table("nfce_preferencia")
                .WithColumn("id").AsGuid().PrimaryKey("pk_nfce_preferencia")
                .WithColumn("isSolicitarTotalQuantidade").AsBoolean().NotNullable();

            Insert.IntoTable("nfce_preferencia")
                .InSchema("dbo")
                .Row(new Dictionary<string, object>()
                {
                    {"id", Guid.NewGuid().ToString()},
                    {"isSolicitarTotalQuantidade", false}
                });
        }

        public override void Down()
        {
        }
    }
}