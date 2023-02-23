using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1560446864)]
    public class FA1560446864_AdicionarLogoNfce : Migration
    {
        public override void Up()
        {
            Alter.Table("empresa").AddColumn("logoNfce").AsCustom("image").Nullable();
        }

        public override void Down()
        {
        }
    }
}