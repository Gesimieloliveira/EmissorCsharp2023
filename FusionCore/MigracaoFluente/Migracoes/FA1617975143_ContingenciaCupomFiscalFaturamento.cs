using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1617975143)]
    public class FA1617975143_ContingenciaCupomFiscalFaturamento : Migration
    {
        public override void Up()
        {
            Create.Table("cupom_fiscal_contingencia")
                .WithColumn("id").AsInt32().Identity().PrimaryKey("pk_cupom_fiscal_contingencia")
                .WithColumn("entrouEm").AsDateTime().NotNullable()
                .WithColumn("finalizaEm").AsDateTime().NotNullable();
        }

        public override void Down()
        {
        }
    }
}