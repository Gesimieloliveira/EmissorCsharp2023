using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1552655351)]
    public class FN1552655351_AdicionarUnidadeTributavel : Migration
    {
        public override void Up()
        {
            Alter.Table("produto").AddColumn("produtoUnidadeTributavel_id").AsInt32().Nullable();

            Create.ForeignKey("fk_produto__produto_unidade_tributavel").FromTable("produto").InSchema("dbo")
                .ForeignColumn("produtoUnidadeTributavel_id").ToTable("produto_unidade").InSchema("dbo")
                .PrimaryColumn("id");


            Alter.Table("nfce_item")
                .AddColumn("siglaUnidadeTributavel").AsAnsiString(10).WithDefaultValue(string.Empty).NotNullable();

            Delete.DefaultConstraint().OnTable("nfce_item").InSchema("dbo").OnColumn("siglaUnidadeTributavel");
        }

        public override void Down()
        {
        }
    }
}