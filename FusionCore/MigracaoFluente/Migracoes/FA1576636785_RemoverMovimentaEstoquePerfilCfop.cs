using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1576636785)]
    public class FA1576636785_RemoverMovimentaEstoquePerfilCfop : Migration
    {
        public override void Up()
        {
            Delete.Column("movimentaEstoque").FromTable("perfil_cfop");
        }

        public override void Down()
        {
        }
    }
}