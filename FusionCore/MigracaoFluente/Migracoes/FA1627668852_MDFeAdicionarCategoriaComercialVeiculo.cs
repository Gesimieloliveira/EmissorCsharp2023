using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1627668852)]
    public class FA1627668852_MDFeAdicionarCategoriaComercialVeiculo : Migration
    {
        public override void Up()
        {
            Alter.Table("mdfe").AddColumn("categoriaComercialVeiculo").AsInt16().NotNullable()
                .WithDefaultValue(2);

            Delete.DefaultConstraint().OnTable("mdfe")
                .InSchema("dbo")
                .OnColumn("categoriaComercialVeiculo");
        }

        public override void Down()
        {
        }
    }
}