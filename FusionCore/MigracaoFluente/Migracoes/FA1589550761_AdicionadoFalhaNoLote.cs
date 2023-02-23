using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1589550761)]
    public class FA1589550761_AdicionadoFalhaNoLote : Migration
    {
        public override void Up()
        {
            Alter.Table("cte_emissao_historico")
                .AddColumn("falhaReceberLote").AsBoolean().NotNullable().WithDefaultValue(false);

            Delete.DefaultConstraint().OnTable("cte_emissao_historico").InSchema("dbo").OnColumn("falhaReceberLote");
        }

        public override void Down()
        {
        }
    }
}