using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1574250651)]
    public class FA1574250651_AdicionarCpfNaEmpresa : Migration
    {
        public override void Up()
        {
            Alter.Table("empresa").AddColumn("cpf").AsAnsiString(11).NotNullable().WithDefaultValue(string.Empty);
            Delete.DefaultConstraint().OnTable("empresa").InSchema("dbo").OnColumn("cpf");
        }

        public override void Down()
        {
        }
    }
}