using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1594146881)]
    public class FA1594146881_MovimentarEstoquePerfilNfe : Migration
    {
        public override void Up()
        {
            Alter
                .Table("perfil_nfe")
                .AddColumn("movimentarEstoqueProduto")
                .AsBoolean().WithDefaultValue(true)
                .NotNullable();

            Delete
                .DefaultConstraint()
                .OnTable("perfil_nfe")
                .InSchema("dbo")
                .OnColumn("movimentarEstoqueProduto");
        }

        public override void Down()
        {
        }
    }
}