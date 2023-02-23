using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1579714128)]
    public class FA1579714128_CteAnulacao : Migration
    {
        public override void Up()
        {
            Alter.Table("cte").AddColumn("chaveCteAnulacao").AsAnsiString(44).WithDefaultValue(string.Empty)
                .NotNullable();

            Delete.DefaultConstraint().OnTable("cte").InSchema("dbo").OnColumn("chaveCteAnulacao");
        }

        public override void Down()
        {
        }
    }
}