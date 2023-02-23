using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1589369498)]
    public class FA1589369498_GerarCte_Historico : Migration
    {
        public override void Up()
        {
            Create.Table("cte_emissao_historico")
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