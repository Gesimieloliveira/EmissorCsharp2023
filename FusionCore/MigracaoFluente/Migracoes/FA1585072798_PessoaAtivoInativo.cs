using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1585072798)]
    public class FA1585072798_PessoaAtivoInativo : Migration
    {
        public override void Up()
        {
            Alter.Table("pessoa").AddColumn("ativo").AsBoolean().WithDefaultValue(true).NotNullable();

            Delete.DefaultConstraint().OnTable("pessoa").InSchema("dbo").OnColumn("ativo");
        }

        public override void Down()
        {
        }
    }
}