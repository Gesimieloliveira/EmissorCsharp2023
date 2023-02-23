using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1616763858)]
    public class FA1616763858_PreVisualizarFaturamento : Migration
    {
        public override void Up()
        {
            Alter.Table("faturamento_preferencia").AddColumn("preVisualizar")
                .AsBoolean().WithDefaultValue(false).NotNullable();

            Delete.DefaultConstraint().OnTable("faturamento_preferencia").InSchema("dbo")
                .OnColumn("preVisualizar");

            Delete.Column("imprimeAposFaturar").FromTable("faturamento_preferencia").InSchema("dbo");
        }

        public override void Down()
        {
        }
    }
}