using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1615221634)]
    public class FA1615221634_TabelasCupomFiscalTransmissao : Migration
    {
        public override void Up()
        {
            Create.Table("cupom_fiscal")
                .WithColumn("id").AsInt32().Identity().NotNullable().PrimaryKey("pk_cupom_fiscal")
                .WithColumn("emissorFiscal_id").AsByte().NotNullable()
                    .ForeignKey("fk_cupom_fiscal__emissor_fiscal", "emissor_fiscal", "id")
                .WithColumn("criadoEm").AsDateTime().NotNullable()
                .WithColumn("numeroFiscal").AsInt32().NotNullable()
                .WithColumn("serie").AsInt16().NotNullable()
                .WithColumn("codigoNumerico").AsInt32().NotNullable()
                .WithColumn("tipoEmissao").AsByte().NotNullable()
                .WithColumn("situacaoFiscal").AsByte().NotNullable()
                .WithColumn("usuario_id").AsInt32().NotNullable()
                    .ForeignKey("fk_cupom_fiscal__usuario", "usuario", "id")
                .WithColumn("faturamentoVenda_id").AsInt32().NotNullable()
                    .ForeignKey("fk_cupom_fiscal__faturamento_venda", "faturamento_venda", "id");


            Create.Table("cupom_fiscal_historico")
                .WithColumn("id").AsInt32().Identity().NotNullable().PrimaryKey("pk_cupom_fiscal_historico")
                .WithColumn("criadoEm").AsDateTime().NotNullable()
                .WithColumn("numeroFiscal").AsInt32().NotNullable()
                .WithColumn("serie").AsInt16().NotNullable()
                .WithColumn("codigoNumerico").AsInt32().NotNullable()
                .WithColumn("tentouEm").AsDateTime().NotNullable()
                .WithColumn("falhaEnvioLote").AsBoolean().NotNullable()
                .WithColumn("finalizado").AsBoolean().NotNullable()
                .WithColumn("chave").AsAnsiString(44).NotNullable()
                .WithColumn("respostaLote").AsCustom("xml").Nullable()
                .WithColumn("envio").AsCustom("xml").NotNullable()
                .WithColumn("resposta").AsCustom("xml").Nullable()
                .WithColumn("houveRejeicao").AsBoolean().NotNullable()
                .WithColumn("rejeicao").AsAnsiString(255).NotNullable()
                .WithColumn("cupomFiscal_id").AsInt32().NotNullable()
                    .ForeignKey("fk_cupom_fiscal_historico__cupom_fiscal", "cupom_fiscal", "id");

            Create.Table("cupom_fiscal_finalizado")
                .WithColumn("cupomFiscal_id").AsInt32().NotNullable().PrimaryKey("pk_cupom_fiscal_finalizado")
                    .ForeignKey("fk_cupom_fiscal_finalizado__cupom_fiscal", "cupom_fiscal", "id")
                .WithColumn("criadoEm").AsDateTime().NotNullable()
                .WithColumn("chave").AsAnsiString(44).NotNullable()
                .WithColumn("protocolo").AsAnsiString(15).NotNullable()
                .WithColumn("autorizadaEm").AsDateTime().NotNullable()
                .WithColumn("xmlAutorizado").AsCustom("xml").NotNullable();
        }

        public override void Down()
        {
        }
    }
}