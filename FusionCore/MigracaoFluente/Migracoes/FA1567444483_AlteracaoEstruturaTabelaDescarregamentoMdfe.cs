using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1567444483)]
    public class FA1567444483_AlteracaoEstruturaTabelaDescarregamentoMdfe : Migration
    {
        public override void Up()
        {
            CriarEstruturaNovaDescarregamento();
            MigrarNotasFiscais();
            MigrarCtes();
            MigrarProdutosPerigosos();
            DeletarEstruturaAnterior();
        }

        private void CriarEstruturaNovaDescarregamento()
        {
            Create.Table("mdfe_descarregamento")
                .WithColumn("id").AsInt32().Identity().PrimaryKey("pk_mdfe_descarregamento")
                .WithColumn("mdfe_id").AsInt32().NotNullable()
                .WithColumn("cidade_id").AsInt32().NotNullable()
                .WithColumn("chaveDocumento").AsAnsiString(44).NotNullable()
                .WithColumn("modeloDocumento").AsInt32().NotNullable()
                .WithColumn("segundoCodigoBarras").AsAnsiString(36).NotNullable()
                .WithColumn("MigracaoDocCte_id").AsInt32().Nullable()
                .WithColumn("MigracaoDocNfe_id").AsInt32().Nullable();

            Create.ForeignKey("fk_mdfe_descarregamento_to_mdfe")
                .FromTable("mdfe_descarregamento").ForeignColumn("mdfe_id")
                .ToTable("mdfe").PrimaryColumn("id");

            Create.ForeignKey("fk_mdfe_descarregamento_to_cidade")
                .FromTable("mdfe_descarregamento").ForeignColumn("cidade_id")
                .ToTable("cidade").PrimaryColumn("id");
        }

        private void MigrarNotasFiscais()
        {
            var insertInto = "insert into mdfe_descarregamento(mdfe_id, cidade_id, chaveDocumento, modeloDocumento, segundoCodigoBarras, MigracaoDocNfe_id)";

            var values = "select imd.mdfe_id, imd.cidade_id, doc.chaveNFe, 55, coalesce(doc.segundoCodigoBarras, ''), doc.id " +
                         "from mdfe_info_municipio_descarga imd " +
                         "inner join mdfe_doc_nfe doc on doc.mdfeInfoMunicipio_id = imd.id";

            Execute.Sql($"{insertInto} {values}");
        }

        private void MigrarCtes()
        {
            var insertInto = "insert into mdfe_descarregamento(mdfe_id, cidade_id, chaveDocumento, modeloDocumento, segundoCodigoBarras, MigracaoDocCte_id)";

            var values = "select imd.mdfe_id, imd.cidade_id, doc.chaveCTe, 57, coalesce(doc.segundoCodigoBarras, ''), doc.id " +
                         "from mdfe_info_municipio_descarga imd " +
                         "inner join mdfe_doc_cte doc on doc.mdfeInfoMunicipio_id = imd.id";

            Execute.Sql($"{insertInto} {values}");
        }

        private void MigrarProdutosPerigosos()
        {
            Alter.Table("mdfe_produto_perigoso")
                .AddColumn("mdfeDescarregamento_id").AsInt32().Nullable();

            Create.ForeignKey("fk_mdfe_produto_perigoso_to_mdfe_descarregamento")
                .FromTable("mdfe_produto_perigoso").ForeignColumn("mdfeDescarregamento_id")
                .ToTable("mdfe_descarregamento").PrimaryColumn("id");

            Execute.Sql("update pp set pp.mdfeDescarregamento_id = d.id from mdfe_produto_perigoso pp inner join mdfe_descarregamento d on d.MigracaoDocCte_id = pp.mdfeDocCte_id;");
            Execute.Sql("update pp set pp.mdfeDescarregamento_id = d.id from mdfe_produto_perigoso pp inner join mdfe_descarregamento d on d.MigracaoDocNfe_id = pp.mdfeDocNfe_id;");

            Alter.Table("mdfe_produto_perigoso")
                .AlterColumn("mdfeDescarregamento_id").AsInt32().NotNullable();
        }

        private void DeletarEstruturaAnterior()
        {
            Delete.Column("MigracaoDocCte_id").FromTable("mdfe_descarregamento");
            Delete.Column("MigracaoDocNfe_id").FromTable("mdfe_descarregamento");

            Delete.ForeignKey("fk_mdfe_produto_perigoso__mdfe_doc_cte").OnTable("mdfe_produto_perigoso");
            Delete.ForeignKey("fk_mdfe_produto_perigoso__mdfe_doc_nfe").OnTable("mdfe_produto_perigoso");

            Delete.Column("mdfeDocCte_id").FromTable("mdfe_produto_perigoso");
            Delete.Column("mdfeDocNfe_id").FromTable("mdfe_produto_perigoso");

            Delete.Table("mdfe_doc_cte");
            Delete.Table("mdfe_doc_nfe");
        }

        public override void Down()
        {
        }
    }
}