using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1556639214)]
    public class FA1556639214_NovaEstruturaPagamentoFaturamento : Migration
    {
        public override void Up()
        {
            Create.Table("faturamento_pagamento")
                .WithColumn("id").AsInt32().NotNullable().Identity()
                .WithColumn("faturamentoVenda_id").AsInt32().NotNullable()
                .WithColumn("especie").AsByte().NotNullable()
                .WithColumn("criadoEm").AsDateTime().NotNullable()
                .WithColumn("usuario_id").AsInt32().NotNullable()
                .WithColumn("valor").AsDecimal(15, 2).NotNullable()
                .WithColumn("possuiParcelas").AsBoolean().NotNullable()
                .WithColumn("tipoDocumento_id").AsInt16().Nullable()
                .WithColumn("registraFinanceiro").AsBoolean().NotNullable();

            Create.PrimaryKey("pk_faturamento_pagamento")
                .OnTable("faturamento_pagamento").Column("id");

            Create.ForeignKey("fk_faturamento_pg_to_faturamento")
                .FromTable("faturamento_pagamento").ForeignColumn("faturamentoVenda_id")
                .ToTable("faturamento_venda").PrimaryColumn("id");

            Create.ForeignKey("fk_faturamento_pg_to_documento")
                .FromTable("faturamento_pagamento").ForeignColumn("tipoDocumento_id")
                .ToTable("tipo_documento").PrimaryColumn("id");

            Create.ForeignKey("fk_faturamento_pg_to_usuario")
                .FromTable("faturamento_pagamento").ForeignColumn("usuario_id")
                .ToTable("usuario").PrimaryColumn("id");

            Create.Table("faturamento_pagamento_parcela")
                .WithColumn("id").AsInt32().NotNullable().Identity()
                .WithColumn("pagamento_id").AsInt32().NotNullable()
                .WithColumn("numero").AsInt16().NotNullable()
                .WithColumn("vencimento").AsDateTime().NotNullable()
                .WithColumn("valor").AsDecimal(15, 2).NotNullable();

            Create.PrimaryKey("pk_faturamento_pagamento_parcela")
                .OnTable("faturamento_pagamento_parcela").Column("id");

            Create.ForeignKey("fk_faturamento_parcela_to_pagamento")
                .FromTable("faturamento_pagamento_parcela").ForeignColumn("pagamento_id")
                .ToTable("faturamento_pagamento").PrimaryColumn("id");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}