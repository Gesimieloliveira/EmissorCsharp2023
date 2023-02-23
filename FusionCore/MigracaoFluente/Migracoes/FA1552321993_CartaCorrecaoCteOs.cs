using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1552321993)]
    public class FA1552321993_CartaCorrecaoCteOs : Migration
    {
        public override void Up()
        {
            Create.Table("cte_os_carta_correcao")
                .WithColumn("id").AsInt32().PrimaryKey("pk_cte_os_carta_correcao").Identity()
                .WithColumn("cteOs_id").AsInt32().NotNullable()
                .WithColumn("ocorreuEm").AsDateTime().NotNullable()
                .WithColumn("statusRetorno").AsInt32().NotNullable()
                .WithColumn("protocolo").AsAnsiString(15).NotNullable()
                .WithColumn("sequenciaEvento").AsByte().NotNullable()
                .WithColumn("xmlEnvio").AsCustom("xml").NotNullable()
                .WithColumn("xmlRetorno").AsCustom("xml").NotNullable()
                .WithColumn("chaveId").AsAnsiString(100).NotNullable();

            Create.Table("cte_os_info_correcao")
                .WithColumn("id").AsInt32().PrimaryKey("pk_cte_os_info_correcao").Identity()
                .WithColumn("grupo").AsAnsiString(20).NotNullable()
                .WithColumn("campo").AsAnsiString(20).NotNullable()
                .WithColumn("novoValor").AsAnsiString(500).NotNullable()
                .WithColumn("numeroItem").AsAnsiString(2).NotNullable()
                .WithColumn("cteOsCartaCorrecao_id").AsInt32().NotNullable();

            Create.ForeignKey("fk_cte_os_carta_correcao__cte_os").FromTable("cte_os_carta_correcao")
                .InSchema("dbo").ForeignColumn("cteOs_id").ToTable("cte_os").InSchema("dbo").PrimaryColumn("id");

            Create.ForeignKey("fk_cte_os_info_correcao__cte_os_carta_correcao").FromTable("cte_os_info_correcao")
                .InSchema("dbo").ForeignColumn("cteOsCartaCorrecao_id").ToTable("cte_os_carta_correcao").InSchema("dbo")
                .PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}