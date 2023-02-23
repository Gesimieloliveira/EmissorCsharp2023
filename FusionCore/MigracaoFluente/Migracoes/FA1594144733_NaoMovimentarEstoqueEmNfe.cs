using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1594144733)]
    public class FA1594144733_NaoMovimentarEstoqueEmNfe : Migration
    {
        public override void Up()
        {
            Alter
                .Table("nfe_item")
                .AddColumn("naoMovimentarEstoque")
                .AsBoolean()
                .WithDefaultValue(false)
                .NotNullable();

            Delete
                .DefaultConstraint()
                .OnTable("nfe_item")
                .InSchema("dbo")
                .OnColumn("naoMovimentarEstoque");
        }

        public override void Down()
        {
        }
    }
}