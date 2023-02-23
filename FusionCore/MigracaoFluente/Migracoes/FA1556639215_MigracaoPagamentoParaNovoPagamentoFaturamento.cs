using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1556639215)]
    public class FA1556639215_MigracaoPagamentoParaNovoPagamentoFaturamento : Migration
    {
        public override void Up()
        {
            Alter.Table("faturamento_pagamento").AddColumn("migId").AsInt32().Nullable();

            Execute.Sql(
                @"insert into faturamento_pagamento(faturamentoVenda_id, especie, criadoEm, usuario_id, possuiParcelas, valor, tipoDocumento_id, registraFinanceiro, migId) 
                    select v.id, pe.especie, pg.criadoEm, pg.criadoPor_id, pe.possuiParcelas, pe.valor, pe.tipoDocumento_id, coalesce(td.registraFinanceiro,0), pe.id
                    from pagamento_especie pe 
                    inner join pagamento pg on pg.id = pe.pagamento_id
                    inner join faturamento_venda v on pe.pagamento_id = v.pagamento_id
                    left join tipo_documento td on td.id = pe.tipoDocumento_id;"
            );

            Execute.Sql(
                @"insert into faturamento_pagamento_parcela(pagamento_id, numero, vencimento, valor)
                    select pn.id, pp.numero, pp.vencimento, pp.valor
                    from pagamento_parcela pp
                    inner join faturamento_pagamento pn on pn.migId = pp.pagamentoEspecie_id"
            );

            Delete.Column("migId").FromTable("faturamento_pagamento");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}