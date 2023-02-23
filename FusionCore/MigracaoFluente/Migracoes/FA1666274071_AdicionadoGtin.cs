using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1666274071)]
    public class FA1666274071_AdicionadoGtin : Migration
    {
        public override void Up()
        {
            Alter.Table("produto_alias")
                .AddColumn("isGtin")
                .AsBoolean().WithDefaultValue(false);

            Delete.DefaultConstraint()
                .OnTable("produto_alias")
                .InSchema("dbo")
                .OnColumn("isGtin");
        }

        public override void Down()
        {
        }
    }
}