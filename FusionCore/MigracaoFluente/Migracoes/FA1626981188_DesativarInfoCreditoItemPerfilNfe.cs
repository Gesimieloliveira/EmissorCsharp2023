using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1626981188)]
    public class FA1626981188_DesativarInfoCreditoItemPerfilNfe : Migration
    {
        public override void Up()
        {
            Alter.Table("perfil_nfe")
                 .AddColumn("desativarInfoCreditoItem").AsBoolean().NotNullable().SetExistingRowsTo(false);
        }

        public override void Down()
        {
            
        }
    }
}