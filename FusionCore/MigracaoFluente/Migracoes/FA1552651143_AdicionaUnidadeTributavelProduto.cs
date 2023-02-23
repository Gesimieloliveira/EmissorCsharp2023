using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1552651143)]
    public class FA1552651143_AdicionaUnidadeTributavelProduto : Migration
    {
        public override void Up()
        {
            Alter.Table("produto").AddColumn("produtoUnidadeTributavel_id").AsInt32().Nullable();

            Create.ForeignKey("fk_produto__produto_unidade_tributavel").FromTable("produto").InSchema("dbo")
                .ForeignColumn("produtoUnidadeTributavel_id").ToTable("produto_unidade").InSchema("dbo")
                .PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}