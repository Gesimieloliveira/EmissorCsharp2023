using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1594142274)]
    public class FA1594142274_DescontoAlteraItemNfceParaAdm : Migration
    {
        public override void Up()
        {
            Alter
                .Table("nfce_item")
                .AddColumn("descontoAlteraItem")
                .AsDecimal(15, 2)
                .NotNullable()
                .WithDefaultValue(0.00m);

            Delete
                .DefaultConstraint()
                .OnTable("nfce_item")
                .InSchema("dbo")
                .OnColumn("descontoAlteraItem");
        }

        public override void Down()
        {
        }
    }
}