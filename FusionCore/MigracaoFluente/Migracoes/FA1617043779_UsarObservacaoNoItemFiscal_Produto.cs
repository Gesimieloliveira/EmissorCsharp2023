using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1617043779)]
    public class FA1617043779_UsarObservacaoNoItemFiscal_Produto : Migration
    {
        public override void Up()
        {
            Alter.Table("produto").AddColumn("usarObservacaoNoItemFiscal")
                .AsBoolean().WithDefaultValue(false).NotNullable();

            Delete.DefaultConstraint().OnTable("produto").InSchema("dbo").OnColumn("usarObservacaoNoItemFiscal");
        }

        public override void Down()
        {
        }
    }
}