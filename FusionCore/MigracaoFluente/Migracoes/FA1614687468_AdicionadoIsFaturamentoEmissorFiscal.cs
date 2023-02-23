using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1614687468)]
    public class FA1614687468_AdicionadoIsFaturamentoEmissorFiscal : Migration
    {
        public override void Up()
        {
            Alter.Table("emissor_fiscal")
                .AddColumn("isFaturamento").AsBoolean().NotNullable().WithDefaultValue(false);

            Delete.DefaultConstraint()
                .OnTable("emissor_fiscal").InSchema("dbo")
                .OnColumn("isFaturamento");
        }

        public override void Down()
        {
        }
    }
}