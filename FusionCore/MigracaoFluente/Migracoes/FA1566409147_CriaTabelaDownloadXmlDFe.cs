using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1566409147)]
    public class FA1566409147_CriaTabelaDownloadXmlDFe : Migration
    {
        public override void Up()
        {
            Create.Table("mde_download_xml")
                .WithColumn("mdeResumo_id").AsInt32().PrimaryKey("pk_mde_download_xml")
                .WithColumn("xml").AsCustom("xml").NotNullable();

            Create.ForeignKey("fk_mde_download_xml_to_mde_resumo")
                .FromTable("mde_download_xml").InSchema("dbo").ForeignColumn("mdeResumo_id")
                .ToTable("mde_resumo").InSchema("dbo").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}