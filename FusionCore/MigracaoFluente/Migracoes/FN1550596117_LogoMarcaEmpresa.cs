using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1550596117)]
    public class FN1550596117_LogoMarcaEmpresa : Migration
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