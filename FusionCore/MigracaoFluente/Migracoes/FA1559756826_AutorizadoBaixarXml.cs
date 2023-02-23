using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1559756826)]
    public class FA1559756826_AutorizadoBaixarXml : Migration
    {
        public override void Up()
        {
            Create.Table("autorizado_baixar_xml")
                .WithColumn("emissorFiscal_id").AsCustom("tinyint").PrimaryKey("pk_autorizado_baixar_xml")
                .WithColumn("descricao").AsAnsiString(75).NotNullable()
                .WithColumn("documentoUnico").AsAnsiString(14).NotNullable();

            Create.ForeignKey("fk_autorizado_baixar_xml__emissor_fiscal")
                .FromTable("autorizado_baixar_xml").InSchema("dbo").ForeignColumn("emissorFiscal_id")
                .ToTable("emissor_fiscal").InSchema("dbo").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}