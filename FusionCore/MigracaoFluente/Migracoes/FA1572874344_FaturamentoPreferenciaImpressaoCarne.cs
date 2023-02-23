using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1572874344)]
    public class FA1572874344_FaturamentoPreferenciaImpressaoCarne : Migration
    {
        public override void Up()
        {
            Alter.Table("faturamento_preferencia")
                .AddColumn("layoutImpressaoPromissoria").AsByte().NotNullable().WithDefaultValue(2);

            Delete.DefaultConstraint().OnTable("faturamento_preferencia").InSchema("dbo").OnColumn("layoutImpressaoPromissoria");
        }

        public override void Down()
        {
        }
    }
}