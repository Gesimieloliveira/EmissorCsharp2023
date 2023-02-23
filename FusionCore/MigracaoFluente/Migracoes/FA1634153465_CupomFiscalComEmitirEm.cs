using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1634153465)]
    public class FA1634153465_CupomFiscalComEmitirEm : Migration
    {
        public override void Up()
        {
            Alter.Table("cupom_fiscal")
                .AddColumn("emitirEm").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now);

            Delete.DefaultConstraint().OnTable("cupom_fiscal").InSchema("dbo")
                .OnColumn("emitirEm");

            Execute.Sql(@"update cupom_fiscal set cupom_fiscal.emitirEm = cupom_fiscal_finalizado.autorizadaEm
                        from cupom_fiscal_finalizado 
                        inner join cupom_fiscal
                        on cupom_fiscal.id = cupom_fiscal_finalizado.cupomFiscal_id");
        }

        public override void Down()
        {
        }
    }
}