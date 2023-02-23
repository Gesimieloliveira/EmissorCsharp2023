using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1649696268)]
    public class FA1649696268_AddMdeConsulta : Migration
    {
        public override void Up()
        {
            Create.Table("mde_consulta")
                .WithColumn("id").AsGuid().PrimaryKey("pk_mde_consulta")
                .WithColumn("documentoUnico").AsAnsiString(14).NotNullable()
                .WithColumn("ambienteSefaz").AsByte().NotNullable()
                .WithColumn("uf").AsFixedLengthAnsiString(2).NotNullable()
                .WithColumn("dataCadastro").AsDateTime().NotNullable()
                .WithColumn("dataResposta").AsDateTime().NotNullable()
                .WithColumn("nsuPesquisado").AsInt64().NotNullable()
                .WithColumn("ultimoNsu").AsInt64().NotNullable()
                .WithColumn("maiorNsu").AsInt64().NotNullable()
                .WithColumn("codigoStatus").AsInt32().NotNullable()
                .WithColumn("motivoStatus").AsAnsiString(500).NotNullable()
                .WithColumn("xmlResposta").AsXml().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("mde_consulta");
        }
    }
}