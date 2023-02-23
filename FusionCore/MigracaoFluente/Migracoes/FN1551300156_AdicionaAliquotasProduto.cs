using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1551300156)]
    public class FN1551300156_AdicionaAliquotasProduto : Migration
    {
        public override void Up()
        {
            Alter.Table("produto").AddColumn("aliquotaPis").AsDecimal(5, 2).WithDefaultValue(0.0m);
            Alter.Table("produto").AddColumn("aliquotaCofins").AsDecimal(5, 2).WithDefaultValue(0.0m);
            Alter.Table("produto").AddColumn("situacaoTributariaPis_id").AsAnsiString(2).WithDefaultValue("49");
            Alter.Table("produto").AddColumn("situacaoTributariaCofins_id").AsAnsiString(2).WithDefaultValue("49");

            Delete.DefaultConstraint().OnTable("produto").InSchema("dbo").OnColumn("aliquotaPis");
            Delete.DefaultConstraint().OnTable("produto").InSchema("dbo").OnColumn("aliquotaCofins");
            Delete.DefaultConstraint().OnTable("produto").InSchema("dbo").OnColumn("situacaoTributariaPis_id");
            Delete.DefaultConstraint().OnTable("produto").InSchema("dbo").OnColumn("situacaoTributariaCofins_id");

            Create.ForeignKey("fk_produto__situacao_tributaria_pis").FromTable("produto").InSchema("dbo")
                .ForeignColumn("situacaoTributariaPis_id")
                .ToTable("situacao_tributaria_pis").InSchema("dbo").PrimaryColumn("id");

            Create.ForeignKey("fk_produto__situacao_tributaria_cofins").FromTable("produto").InSchema("dbo")
                .ForeignColumn("situacaoTributariaCofins_id")
                .ToTable("situacao_tributaria_cofins").InSchema("dbo").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}