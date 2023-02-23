using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1551286602)]
    public class FN1551286602_CorrecaoValorUnitarioItemDaNfce : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce_item").AlterColumn("valorUnitario").AsDecimal(17, 6).NotNullable();
            Execute.Sql("update nfce_item set quantidade = 1 where nfce_item.quantidade = 0");
            Execute.Sql("update nfce_item set valorUnitario=valorTotal/quantidade");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}