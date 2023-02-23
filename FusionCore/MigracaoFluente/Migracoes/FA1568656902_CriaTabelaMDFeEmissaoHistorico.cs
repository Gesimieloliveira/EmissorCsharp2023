using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1568656902)]
    public class FA1568656902_CriaTabelaMDFeEmissaoHistorico : Migration
    {
        public override void Up()
        {
            Create.Table("mdfe_emissao_historico")
                .WithColumn("id").AsInt32().Identity().NotNullable().PrimaryKey("pk_mdfe_emissao_historico")
                .WithColumn("mdfe_id").AsInt32().NotNullable()
                .ForeignKey("fk_mdfe_emissao_historico__mdfe", "mdfe", "id")
                .WithColumn("ambienteSefaz").AsByte().NotNullable()
                .WithColumn("tipoEmissao").AsByte().NotNullable()
                .WithColumn("chave").AsAnsiString(44).NotNullable()
                .WithColumn("finalizada").AsBoolean().NotNullable()
                .WithColumn("xmlEnvio").AsCustom("xml").NotNullable()
                .WithColumn("xmlRetorno").AsCustom("xml").Nullable()
                .WithColumn("criadoEm").AsDateTime().NotNullable()
                .WithColumn("enviadoEm").AsDateTime().Nullable();
        }

        public override void Down()
        {
        }
    }
}