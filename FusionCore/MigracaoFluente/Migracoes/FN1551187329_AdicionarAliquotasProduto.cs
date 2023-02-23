using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1551187329)]
    public class FN1551187329_AdicionarAliquotasProduto : Migration
    {
        public override void Up()
        {
            Alter.Table("produto")
                .AddColumn("reducaoIcms")
                .AsDecimal(5, 2)
                .WithDefaultValue(0)
                .NotNullable();
            
            Delete
                .DefaultConstraint()
                .OnTable("produto")
                .InSchema("dbo")
                .OnColumn("reducaoIcms");
        }

        public override void Down()
        {
            
        }
    }
}