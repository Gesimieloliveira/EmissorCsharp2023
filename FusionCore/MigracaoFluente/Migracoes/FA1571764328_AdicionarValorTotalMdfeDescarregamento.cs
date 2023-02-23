using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1571764328)]
    public class FA1571764328_AdicionarValorTotalMdfeDescarregamento : Migration
    {
        public override void Up()
        {
            Alter.Table("mdfe_descarregamento")
                .AddColumn("valorTotal").AsDecimal(15, 2).WithDefaultValue(0);

            Delete.DefaultConstraint().OnTable("mdfe_descarregamento").InSchema("dbo").OnColumn("valorTotal");
        }

        public override void Down()
        {
        }
    }
}