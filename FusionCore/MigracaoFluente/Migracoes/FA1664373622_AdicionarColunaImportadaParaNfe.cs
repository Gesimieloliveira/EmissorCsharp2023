using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1664373622)]
    public class FA1664373622_AdicionarColunaImportadaParaNfe : Migration
    {
        public override void Up()
        {
            Alter.Table("cupom_fiscal")
                .AddColumn("importadaParaNfe").AsBoolean().NotNullable()
                .WithDefaultValue(false);

            Delete.DefaultConstraint().OnTable("cupom_fiscal")
                .InSchema("dbo")
                .OnColumn("importadaParaNfe");
        }

        public override void Down()
        {
        }
    }
}