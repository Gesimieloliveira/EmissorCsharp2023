using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1552924213)]
    public class FA1552924213_AlteracaoIdParaCsrtId : Migration
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