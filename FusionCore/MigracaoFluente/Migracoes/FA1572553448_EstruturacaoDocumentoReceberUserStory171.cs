using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1572553448)]
    public class FA1572553448_EstruturacaoDocumentoReceberUserStory171 : Migration
    {
        public override void Up()
        {
            AdaptacaoTabelaDocumentosAReceber();
            AdaptacaoTabelaDeLancamentos();
            AdicionarTabelaDeCancelamento();
            AlterarQuitacaoParaRecebimento();
        }

        private void AdaptacaoTabelaDocumentosAReceber()
        {
            Rename.Column("juros").OnTable("documento_receber").To("totalJuros");
            Rename.Column("desconto").OnTable("documento_receber").To("totalDesconto");

            Alter.Table("documento_receber")
                .AddColumn("ultimoCalculoJuros").AsDateTime().Nullable()
                .AddColumn("usuarioCriacao_id").AsInt32().Nullable()
                .AddColumn("dataQuitacao").AsDateTime().Nullable()
                .AlterColumn("valorOriginal").AsDecimal(15, 2).NotNullable()
                .AlterColumn("valorAjustado").AsDecimal(15, 2).NotNullable()
                .AlterColumn("valorQuitado").AsDecimal(15, 2).NotNullable()
                .AlterColumn("totalDesconto").AsDecimal(15, 2).NotNullable()
                .AlterColumn("totalJuros").AsDecimal(15,2).NotNullable();

            Execute.Sql("update documento_receber set usuarioCriacao_id = coalesce((select top 1 ev.usuario_id from documento_receber_evento ev where ev.documentoReceber_id = documento_receber.id), 1);");
            Execute.Sql("update documento_receber set ultimoCalculoJuros = (select top 1 drl.criadoEm from documento_receber_lancamento drl where drl.documentoReceber_id = documento_receber.id and drl.tipoLancamento = 0 order by id desc);");
            Execute.Sql("update dr set dr.dataQuitacao = (select top 1 drl.criadoEm from documento_receber_lancamento drl where drl.documentoReceber_id = dr.id order by id desc) from documento_receber dr where dr.situacao = 1;");

            Alter.Table("documento_receber")
                .AlterColumn("usuarioCriacao_id").AsInt32().NotNullable()
                .ForeignKey("fk_documento_receber_to_usuario", "usuario", "id");
        }

        private void AdaptacaoTabelaDeLancamentos()
        {
            Alter.Table("documento_receber_lancamento").AddColumn("cancelado").AsBoolean().NotNullable().SetExistingRowsTo(0);
            Alter.Table("documento_receber_lancamento").AddColumn("usuarioCriacao_id").AsInt32().Nullable();
            Alter.Table("documento_receber_lancamento").AddColumn("usuarioEstorno_id").AsInt32().Nullable();

            Execute.Sql("update documento_receber_lancamento set usuarioCriacao_id = (select top 1 ev.usuario_id from documento_receber_evento ev where ev.documentoReceber_id = documento_receber_lancamento.documentoReceber_id);");

            Alter.Table("documento_receber_lancamento")
                .AlterColumn("usuarioCriacao_id").AsInt32().NotNullable()
                .ForeignKey("fk_documento_receber_lancamento_to_usuario", "usuario", "id");

            Create.ForeignKey("fk_documento_receber_lancamento_estorno_to_usuario")
                .FromTable("documento_receber_lancamento").ForeignColumn("usuarioEstorno_id")
                .ToTable("usuario").PrimaryColumn("id");
        }

        private void AdicionarTabelaDeCancelamento()
        {
            Create.Table("documento_receber_cancelamento")
                .WithColumn("documentoReceber_id").AsInt32().PrimaryKey("pk_documento_receber_cancelamento")
                .WithColumn("usuarioCancelamento_id").AsInt32().NotNullable()
                .WithColumn("dataCancelamento").AsDateTime().NotNullable();

            Create.ForeignKey("fk_documento_receber_cancelamento_to_receber")
                .FromTable("documento_receber_cancelamento").ForeignColumn("documentoReceber_id")
                .ToTable("documento_receber").PrimaryColumn("id");

            Create.ForeignKey("fk_documento_receber_cancelamento_to_usuario")
                .FromTable("documento_receber_cancelamento").ForeignColumn("usuarioCancelamento_id")
                .ToTable("usuario").PrimaryColumn("id");
        }

        private void AlterarQuitacaoParaRecebimento()
        {
            Execute.Sql("update documento_receber_lancamento set tipoLancamento = 2 where tipoLancamento = 3;");
            Execute.Sql("update documento_receber_lancamento set tipoLancamentoTexto = 'Recebimento' where tipoLancamento = 2;");
        }

        public override void Down()
        {
        }
    }
}