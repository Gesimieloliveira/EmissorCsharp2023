using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1594146423)]
    public class FA1594146423_MovimentarEstoqueConfiguracaoNFe : Migration
    {
        public override void Up()
        {
            Delete.Column("naoMovimentarEstoque")
                .FromTable("nfe_item").InSchema("dbo");

            Alter
                .Table("nfe_item")
                .AddColumn("movimentarEstoqueConfiguracao")
                .AsBoolean()
                .WithDefaultValue(true)
                .NotNullable();

            Delete
                .DefaultConstraint()
                .OnTable("nfe_item")
                .InSchema("dbo")
                .OnColumn("movimentarEstoqueConfiguracao");
        }

        public override void Down()
        {
        }
    }
}