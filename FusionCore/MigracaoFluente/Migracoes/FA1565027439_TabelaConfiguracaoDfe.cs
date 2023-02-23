using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1565027439)]
    public class FA1565027439_TabelaConfiguracaoDfe : Migration
    {
        public override void Up()
        {
            Create.Table("configuracao_dfe")
                .WithColumn("id").AsGuid().NotNullable()
                .WithColumn("ambienteSefaz").AsByte().NotNullable()
                .WithColumn("isQrCodeCte").AsBoolean().NotNullable()
                .WithColumn("isQrCodeCteOs").AsBoolean().NotNullable()
                .WithColumn("isQrCodeMdfe").AsBoolean().NotNullable()
                .WithColumn("uf_id").AsByte().NotNullable();

            Create.PrimaryKey("pk_configuracao_dfe").OnTable("configuracao_dfe")
                .Columns("id", "ambienteSefaz");

            Create.ForeignKey("fk_configuracao_dfe__uf")
                .FromTable("configuracao_dfe").InSchema("dbo")
                .ForeignColumn("uf_id").ToTable("uf").InSchema("dbo")
                .PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}