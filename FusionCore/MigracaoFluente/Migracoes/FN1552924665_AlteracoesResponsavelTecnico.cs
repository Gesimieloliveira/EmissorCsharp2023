using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1552924665)]
    public class FN1552924665_AlteracoesResponsavelTecnico : Migration
    {
        public override void Up()
        {
            Alter.Table("responsavel_tecnico")
                .AddColumn("csrtId").AsInt32().WithDefaultValue(0).NotNullable();

            Delete.DefaultConstraint().OnTable("responsavel_tecnico").InSchema("dbo").OnColumn("csrtId");

            Delete.Column("id").FromTable("responsavel_tecnico").InSchema("dbo");
        }

        public override void Down()
        {
        }
    }
}