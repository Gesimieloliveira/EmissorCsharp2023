using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1608572448)]
    public class FA1608572448_RemoverAutoAtivarObsIcmDoPerfilNfe : Migration
    {
        public override void Up()
        {
            Delete.Column("autoAtivarCreditoItem").FromTable("perfil_nfe");
        }

        public override void Down()
        {
        }
    }
}