using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1549992107)]
    public class FA1549992107_FaturamentoComNovoPagamento : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"alter table faturamento_venda add pagamento_id uniqueidentifier null;");
            Execute.Sql(@"alter table faturamento_venda add constraint fk_faturamento_to_pagamento foreign key (pagamento_id) references pagamento(id);");

            Execute.Sql(@"alter table pagamento add migracao_faturamentoId int null;");
            Execute.Sql(@"alter table pagamento_especie add migracao_faturamentoEspecieId int null;");

            Execute.Sql(@"insert into pagamento(migracao_faturamentoId, id, criadoEm, criadoPor_id, valor) 
	            select f.faturamentoVenda_id, NEWID(), max(f.criadoEm), max(f.usuarioCriacao_id), sum(f.valor) from faturamento_forma_pagamento f group by f.faturamentoVenda_id");

            Execute.Sql(@"insert into pagamento_especie(migracao_faturamentoEspecieId, pagamento_id, especie, valor, possuiParcelas)
            	select pfp.id, pg.id, pfp.tipo, pfp.valor, 0 from faturamento_forma_pagamento pfp inner join pagamento pg on pfp.faturamentoVenda_id = pg.migracao_faturamentoId;");

            Execute.Sql(@"insert into pagamento_parcela(pagamentoEspecie_id, numero, vencimento, valor, tipoDocumento_id)
	            select pe.id, pvp.numero, pvp.vencimento, pvp.valor, coalesce(pfp.tipoDocumento_id, 1)
	            from faturamento_parcela pvp 
	            inner join pagamento_especie pe on pe.migracao_faturamentoEspecieId = pvp.faturamentoFormaPagamento_id
	            inner join faturamento_forma_pagamento pfp on pfp.id = pe.migracao_faturamentoEspecieId;");

            Execute.Sql(@"update pagamento_especie set possuiParcelas=0 where especie = 0;");
            Execute.Sql(@"update faturamento_venda set pagamento_id = (select pg.id from pagamento pg where pg.migracao_faturamentoId = faturamento_venda.id);");

            Execute.Sql(@"alter table pagamento drop column migracao_faturamentoId;");
            Execute.Sql(@"alter table pagamento_especie drop column migracao_faturamentoEspecieId;");

            Execute.Sql(@"drop table faturamento_parcela;");
            Execute.Sql(@"drop table faturamento_forma_pagamento;");
        }

        public override void Down()
        {
            
        }
    }
}