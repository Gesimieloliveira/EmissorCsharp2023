using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1585076833)]
    public class FN1585076833_ClienteAtivoInativo : Migration
    {
        public override void Up()
        {
            Alter.Table("cliente").AddColumn("ativo").AsBoolean().WithDefaultValue(true).NotNullable();

            Delete.DefaultConstraint().OnTable("cliente").InSchema("dbo").OnColumn("ativo");
        }

        public override void Down()
        {
        }
    }
}