using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1584018605)]
    public class FA1584018605_MDFeLacradoCargaFechada : Migration
    {
        public override void Up()
        {
            Alter.Table("mdfe")
                .AddColumn("cargaFechada").AsBoolean().NotNullable().WithDefaultValue(false);

            Alter.Table("mdfe").AddColumn("cepCarregamento").AsAnsiString(8).NotNullable()
                .WithDefaultValue(string.Empty);

            Alter.Table("mdfe").AddColumn("cepDescarregamento").AsAnsiString(8).NotNullable()
                .WithDefaultValue(string.Empty);

            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("cargaFechada");
            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("cepCarregamento");
            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("cepDescarregamento");
        }

        public override void Down()
        {
        }
    }
}