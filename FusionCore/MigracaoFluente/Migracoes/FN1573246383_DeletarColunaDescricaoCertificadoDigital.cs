using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1573246383)]
    public class FN1573246383_DeletarColunaDescricaoCertificadoDigital : Migration
    {
        public override void Up()
        {
            Delete.Column("descricao").FromTable("certificado_digital");
        }

        public override void Down()
        {
        }
    }
}