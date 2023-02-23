using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1569613899)]
    public class FA1569613899_UpdateVeiculosPropriosComProprietario : Migration
    {
        public override void Up()
        {
            Execute.Sql("update veiculo set pessoa_id = null where tipoProprietario = 0");
        }

        public override void Down()
        {
        }
    }
}