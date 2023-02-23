using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1616442393)]
    public class FA1616442393_FaturamentoVendaCancelamento : Migration
    {
        public override void Up()
        {
            Alter.Table("faturamento_venda")
                .AddColumn("cancelamentoJustificativa").AsAnsiString(255).WithDefaultValue(string.Empty).NotNullable();

            
        }

        public override void Down()
        {
        }
    }
}