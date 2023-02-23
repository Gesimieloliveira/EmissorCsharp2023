using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1567001097)]
    public class FA1567001097_AdicionarFretamentoCteOs : Migration
    {
        public override void Up()
        {
            Alter.Table("cte_os")
                .AddColumn("tipoFretamento").AsByte().NotNullable().WithDefaultValue(1)
                .AddColumn("viagemEm").AsDateTime().Nullable();

            Delete.DefaultConstraint().OnTable("cte_os").InSchema("dbo").OnColumn("tipoFretamento");
        }

        public override void Down()
        {
        }
    }
}