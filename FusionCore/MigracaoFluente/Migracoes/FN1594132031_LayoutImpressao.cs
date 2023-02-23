using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1594132031)]
    public class FN1594132031_LayoutImpressao : Migration
    {
        public override void Up()
        {
            Alter.Table("preferencia_terminal")
                .AddColumn("layoutImpressao")
                .AsByte()
                .WithDefaultValue(0)
                .NotNullable();

            Delete.DefaultConstraint()
                .OnTable("preferencia_terminal")
                .InSchema("dbo")
                .OnColumn("layoutImpressao");
        }

        public override void Down()
        {
            
        }
    }
}