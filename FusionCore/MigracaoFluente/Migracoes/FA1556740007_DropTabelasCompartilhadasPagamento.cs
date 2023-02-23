using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1556740007)]
    public class FA1556740007_DropTabelasCompartilhadasPagamento : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("fk_pedido_to_pagamento").OnTable("pedido_venda");
            Delete.ForeignKey("fk_faturamento_to_pagamento").OnTable("faturamento_venda");

            Delete.Column("pagamento_id").FromTable("pedido_venda");
            Delete.Column("pagamento_id").FromTable("faturamento_venda");

            Delete.Table("pagamento_parcela");
            Delete.Table("pagamento_especie");
            Delete.Table("pagamento");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}