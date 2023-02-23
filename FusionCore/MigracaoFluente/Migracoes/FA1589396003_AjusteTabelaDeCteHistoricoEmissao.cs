using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1589396003)]
    public class FA1589396003_AjusteTabelaDeCteHistoricoEmissao : Migration
    {
        public override void Up()
        {
            Delete.Table("cte_emissao_historico");

            Create.Table("cte_emissao_historico")
                .WithColumn("id").AsInt32().Identity().NotNullable().PrimaryKey("pk_cte_emissao_historico")
                .WithColumn("cte_id").AsInt32().NotNullable().ForeignKey("fk_cte_emissao_historico__cte", "cte", "id")
                .WithColumn("ambienteSefaz").AsByte().NotNullable()
                .WithColumn("tipoEmissao").AsByte().NotNullable()
                .WithColumn("chave").AsAnsiString(44).NotNullable()
                .WithColumn("finalizada").AsBoolean().NotNullable()
                .WithColumn("xmlEnvio").AsCustom("xml").NotNullable()
                .WithColumn("xmlRetorno").AsCustom("xml").Nullable()
                .WithColumn("criadoEm").AsDateTime().NotNullable()
                .WithColumn("enviadoEm").AsDateTime().Nullable()
                .WithColumn("numeroRecibo").AsAnsiString(15)
                .WithColumn("xmlLote").AsCustom("xml").Nullable();
        }

        public override void Down()
        {
        }
    }
}