using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1586173111)]
    public class FA1586173111_FlagImportacaoNfceParaNfe : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce").AddColumn("importadaParaNfe").AsBoolean().NotNullable().WithDefaultValue(false);
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("importadaParaNfe");
        }

        public override void Down()
        {
            
        }
    }
}