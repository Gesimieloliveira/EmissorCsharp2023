using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1566396810)]
    public class FA1566396810_CriaTabelaResumoNfe : Migration
    {
        public override void Up()
        {
            Create.Table("mde_resumo")
                .WithColumn("id").AsInt32().Identity().PrimaryKey("pk_mde_resumo")
                .WithColumn("chave").AsAnsiString(44).NotNullable()
                .WithColumn("serie").AsInt16().NotNullable()
                .WithColumn("numeroFiscal").AsAnsiString(9).NotNullable()
                .WithColumn("documentoUnicoEmitente").AsAnsiString(14).NotNullable()
                .WithColumn("inscricaoEstadualEmitente").AsAnsiString(14).NotNullable()
                .WithColumn("emitidaEm").AsDateTime().NotNullable()
                .WithColumn("tipoOperacao").AsByte().NotNullable()
                .WithColumn("Valor").AsDecimal(15, 2).NotNullable()
                .WithColumn("autorizacaoEm").AsDateTime().NotNullable()
                .WithColumn("numeroProtocolo").AsAnsiString(15).NotNullable()
                .WithColumn("razaoSocialEmitente").AsAnsiString(60).NotNullable()
                .WithColumn("statusNfe").AsByte().NotNullable()
                .WithColumn("xml").AsCustom("xml").NotNullable();
        }

        public override void Down()
        {
        }
    }
}