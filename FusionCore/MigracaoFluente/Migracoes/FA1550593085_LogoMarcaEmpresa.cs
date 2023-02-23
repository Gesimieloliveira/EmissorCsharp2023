using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1550593085)]
    public class FA1550593085_LogoMarcaEmpresa : Migration
    {
        public override void Up()
        {
            Alter.Table("empresa").AddColumn("logo").AsCustom("image").Nullable();
        }

        public override void Down()
        {
            
        }
    }
}