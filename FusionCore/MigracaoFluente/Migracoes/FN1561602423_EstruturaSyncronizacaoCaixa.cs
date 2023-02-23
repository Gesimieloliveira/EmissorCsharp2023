using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1561602423)]
    public class FN1561602423_EstruturaSyncronizacaoCaixa : Migration
    {
        public override void Up()
        {
            CriarTabelaSyncDeLancamentos();
            CriarTabelaSyncDeRegistroDoCaixa();
            CriarTabelaSyncDeCaixaIndividual();
        }

        private void CriarTabelaSyncDeLancamentos()
        {
            Create.Table("sync_caixa_lancamento")
                .WithColumn("caixaLancamento_id").AsGuid().PrimaryKey("pk_sync_caixa_lancamento");

            Create.ForeignKey("fk_sync_caixa_lancamento_to_caixa_lancamento")
                .FromTable("sync_caixa_lancamento").ForeignColumn("caixaLancamento_id")
                .ToTable("caixa_lancamento").PrimaryColumn("id");
        }

        private void CriarTabelaSyncDeRegistroDoCaixa()
        {
            Create.Table("sync_caixa_registro")
                .WithColumn("caixaRegistro_id").AsGuid().PrimaryKey("pk_sync_caixa_registro");

            Create.ForeignKey("fk_sync_caixa_registro_to_caixa_registro")
                .FromTable("sync_caixa_registro").ForeignColumn("caixaRegistro_id")
                .ToTable("caixa_registro").PrimaryColumn("id");
        }

        private void CriarTabelaSyncDeCaixaIndividual()
        {
            Create.Table("sync_caixa_individual")
                .WithColumn("caixaIndividual_id").AsGuid().PrimaryKey("pk_sync_caixa_individual");

            Create.ForeignKey("fk_sync_caixa_individual_to_caixa_individual")
                .FromTable("sync_caixa_individual").ForeignColumn("caixaIndividual_id")
                .ToTable("caixa_individual").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}