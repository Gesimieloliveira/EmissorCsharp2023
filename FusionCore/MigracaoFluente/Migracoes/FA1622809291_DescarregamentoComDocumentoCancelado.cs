using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1622809291)]
    public class FA1622809291_DescarregamentoComDocumentoCancelado : Migration
    {
        public override void Up()
        {
            const string tabela = "mdfe_descarregamento";
            const string colunaNova = "cancelado";

            Alter.Table(tabela).AddColumn(colunaNova).AsBoolean().WithDefaultValue(false)
                .NotNullable();

            Delete.DefaultConstraint().OnTable(tabela).InSchema("dbo").OnColumn(colunaNova);
        }

        public override void Down()
        {
            
        }
    }
}