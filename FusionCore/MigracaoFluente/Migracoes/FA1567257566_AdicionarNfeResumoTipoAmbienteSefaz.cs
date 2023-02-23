using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1567257566)]
    public class FA1567257566_AdicionarNfeResumoTipoAmbienteSefaz : Migration
    {
        public override void Up()
        {
            Alter.Table("mde_resumo").AddColumn("ambienteSefaz").AsByte().NotNullable();
        }

        public override void Down()
        {
        }
    }
}