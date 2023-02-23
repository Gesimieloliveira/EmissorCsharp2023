using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1615387353)]
    public class FA1615387353_AdicionaColunaTipoAmbienteSefaz : Migration
    {
        public override void Up()
        {
            Alter.Table("cupom_fiscal")
                .AddColumn("ambienteSefaz").AsByte().NotNullable();
        }

        public override void Down()
        {
        }
    }
}