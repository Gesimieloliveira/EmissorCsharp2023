using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1573129411)]
    public class FN1573129411_CadastroCertificadoDigitalNfce : Migration
    {
        public override void Up()
        {
            Create.Table("certificado_digital")
                .WithColumn("id").AsInt32().Identity().NotNullable().PrimaryKey("pk_certificado_digital")
                .WithColumn("empresa_id").AsInt16().NotNullable()
                .ForeignKey("fk_certificado_digital__empresa", "empresa", "id")
                .WithColumn("descricao").AsAnsiString(120).NotNullable()
                .WithColumn("tipo").AsByte().NotNullable()
                .WithColumn("serialRepositorio").AsAnsiString(255).NotNullable()
                .WithColumn("caminhoArquivo").AsAnsiString(255).NotNullable()
                .WithColumn("senha").AsAnsiString(255).NotNullable();
        }

        public override void Down()
        {
        }
    }
}