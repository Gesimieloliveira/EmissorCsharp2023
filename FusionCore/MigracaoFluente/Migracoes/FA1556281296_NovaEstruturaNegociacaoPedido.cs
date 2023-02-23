using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1556281296)]
    public class FA1556281296_NovaEstruturaNegociacaoPedido : Migration
    {
        public override void Up()
        {
            Create.Table("pedido_negociacao")
                .WithColumn("id").AsInt32().NotNullable().Identity()
                .WithColumn("pedidoVenda_id").AsInt32().NotNullable()
                .WithColumn("especie").AsByte().NotNullable()
                .WithColumn("criadoEm").AsDateTime().NotNullable()
                .WithColumn("usuario_id").AsInt32().NotNullable()
                .WithColumn("valor").AsDecimal(15, 2).NotNullable()
                .WithColumn("possuiParcelas").AsBoolean().NotNullable()
                .WithColumn("tipoDocumento_id").AsInt16().Nullable();

            Create.PrimaryKey("pk_pedido_negociacao")
                .OnTable("pedido_negociacao").Column("id");

            Create.ForeignKey("fk_pedido_negociacao_to_pedido")
                .FromTable("pedido_negociacao").ForeignColumn("pedidoVenda_id")
                .ToTable("pedido_venda").PrimaryColumn("id");

            Create.ForeignKey("fk_pedido_negociacao_to_documento")
                .FromTable("pedido_negociacao").ForeignColumn("tipoDocumento_id")
                .ToTable("tipo_documento").PrimaryColumn("id");

            Create.ForeignKey("fk_pedido_negociacao_to_usuario")
                .FromTable("pedido_negociacao").ForeignColumn("usuario_id")
                .ToTable("usuario").PrimaryColumn("id");

            Create.Table("pedido_negociacao_parcela")
                .WithColumn("id").AsInt32().NotNullable().Identity()
                .WithColumn("pedidoNegociacao_id").AsInt32().NotNullable()
                .WithColumn("numero").AsInt16().NotNullable()
                .WithColumn("vencimento").AsDateTime().NotNullable()
                .WithColumn("valor").AsDecimal(15, 2).NotNullable();

            Create.PrimaryKey("pk_pedido_negociacao_parcela")
                .OnTable("pedido_negociacao_parcela").Column("id");

            Create.ForeignKey("fk_pedido_negociacao_parcela_to_negociacao")
                .FromTable("pedido_negociacao_parcela").ForeignColumn("pedidoNegociacao_id")
                .ToTable("pedido_negociacao").PrimaryColumn("id");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}