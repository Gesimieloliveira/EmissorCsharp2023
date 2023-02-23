using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1567258723)]
    public class FA1567258723_CriarTipoAmbienteDistribuicaoDFe : Migration
    {
        public override void Up()
        {
            Alter.Table("mde_distribuicao").AddColumn("ambienteSefaz").AsByte().NotNullable();
        }

        public override void Down()
        {
        }
    }
}