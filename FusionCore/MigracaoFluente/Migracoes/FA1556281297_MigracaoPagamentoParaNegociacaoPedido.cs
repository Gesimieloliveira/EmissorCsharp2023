using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1556281297)]
    public class FA1556281297_MigracaoPagamentoParaNegociacaoPedido : Migration
    {
        public override void Up()
        {
            Alter.Table("pedido_negociacao").AddColumn("migId").AsInt32().Nullable();

            Execute.Sql(
                @"insert into pedido_negociacao(pedidoVenda_id, especie, criadoEm, usuario_id, possuiParcelas, valor, tipoDocumento_id, migId) 
                    select pv.id, pe.especie, pg.criadoEm, pg.criadoPor_id, pe.possuiParcelas, pe.valor, pe.tipoDocumento_id, pe.id
                    from pagamento_especie pe 
                    inner join pagamento pg on pg.id = pe.pagamento_id
                    inner join pedido_venda pv on pe.pagamento_id = pv.pagamento_id;"
            );

            Execute.Sql(
                @"insert into pedido_negociacao_parcela(pedidoNegociacao_id, numero, vencimento, valor)
                    select pn.id, pp.numero, pp.vencimento, pp.valor
                    from pagamento_parcela pp
                    inner join pedido_negociacao pn on pn.migId = pp.pagamentoEspecie_id"
            );

            Delete.Column("migId").FromTable("pedido_negociacao");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}