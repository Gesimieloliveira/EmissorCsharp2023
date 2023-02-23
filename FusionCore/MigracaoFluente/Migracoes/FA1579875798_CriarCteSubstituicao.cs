using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1579875798)]
    public class FA1579875798_CriarCteSubstituicao : Migration
    {
        public override void Up()
        {
            Delete
                .Table("cte_substituicao");

            Create
                .Table("cte_substituicao")
                .WithColumn("cte_id").AsInt32().NotNullable().PrimaryKey("pk_cte_substituicao")
                .ForeignKey("fk_cte_substituicao__cte", "cte", "id")
                .WithColumn("chaveSubstituido").AsAnsiString(44).NotNullable()
                .WithColumn("chaveAnulacao").AsAnsiString(44).NotNullable()
                .WithColumn("chaveNfePeloTomador").AsAnsiString(44).NotNullable()
                .WithColumn("chaveCtePeloTomador").AsAnsiString(44).NotNullable()
                .WithColumn("documentoUnico").AsAnsiString(44).NotNullable()
                .WithColumn("modeloDocumento").AsByte().NotNullable()
                .WithColumn("serie").AsAnsiString(3).NotNullable()
                .WithColumn("subserie").AsAnsiString(3).NotNullable()
                .WithColumn("numeroDocumentoFiscal").AsAnsiString(6).NotNullable()
                .WithColumn("valor").AsDecimal(15, 2).NotNullable()
                .WithColumn("emitidoEm").AsDateTime().NotNullable();
        }

        public override void Down()
        {
        }
    }
}