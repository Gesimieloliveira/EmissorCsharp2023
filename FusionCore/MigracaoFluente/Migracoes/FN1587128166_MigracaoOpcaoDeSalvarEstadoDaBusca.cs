using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1587128166)]
    public class FN1587128166_MigracaoOpcaoDeSalvarEstadoDaBusca : Migration
    {
        public override void Up()
        {
            Alter.Table("preferencia_terminal").AddColumn("salvarUltimaBuscaProduto").AsBoolean().NotNullable()
                .WithDefaultValue(false);

            Delete.DefaultConstraint().OnTable("preferencia_terminal").InSchema("dbo").OnColumn("salvarUltimaBuscaProduto");
        }

        public override void Down()
        {
        }
    }
}