using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1549992109)]
    public class FA1549992109_PedidoComNovoPagamento : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"alter table pedido_venda add pagamento_id uniqueidentifier null;");
            Execute.Sql(@"alter table pedido_venda add constraint fk_pedido_to_pagamento foreign key (pagamento_id) references pagamento(id);");

            Execute.Sql(@"alter table pagamento add migPedido_id int null;");
            Execute.Sql(@"alter table pagamento_especie add migEspecie_id int null;");

            Execute.Sql(@"insert into pagamento(migPedido_id, id, criadoEm, criadoPor_id, valor) 
	            select pfp.pedidoVenda_id, NEWID(), max(pfp.criadoEm), max(pfp.usuario_id), sum(pfp.valor) from pedido_forma_pagamento pfp group by pfp.pedidoVenda_id;");

            Execute.Sql(@"insert into pagamento_especie(migEspecie_id, pagamento_id, especie, valor, possuiParcelas)
	            select pfp.id, pg.id, pfp.tipo, pfp.valor, 0 from pedido_forma_pagamento pfp inner join pagamento pg on pfp.pedidoVenda_id = pg.migPedido_id;");

            Execute.Sql(@"insert into pagamento_parcela(pagamentoEspecie_id, numero, vencimento, valor, tipoDocumento_id)
	            select pe.id, pvp.numero, pvp.vencimento, pvp.valor, pfp.tipoDocumento_id
	            from pedido_parcela pvp 
	            inner join pagamento_especie pe on pe.migEspecie_id = pvp.pedidoFormaPagamento_id
	            inner join pedido_forma_pagamento pfp on pfp.id = pe.migEspecie_id;");

            Execute.Sql(@"update pagamento_especie set possuiParcelas=0 where especie = 0;");
            Execute.Sql(@"update pedido_venda set pagamento_id = (select pg.id from pagamento pg where pg.migPedido_id =pedido_venda.id);");

            Execute.Sql(@"alter table pagamento drop column migPedido_id;");
            Execute.Sql(@"alter table pagamento_especie drop column migEspecie_id;");

            Execute.Sql(@"drop table pedido_parcela;");
            Execute.Sql(@"drop table pedido_forma_pagamento;");
        }

        public override void Down()
        {
            
        }
    }
}