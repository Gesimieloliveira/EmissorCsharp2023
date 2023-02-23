using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1596582297)]
    public class FA1596582297_SeguroAverbacaoMDFE : Migration
    {
        public override void Up()
        {
            Create.Table("mdfe_seguro_averbacao")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey("pk_mdfe_seguro_averbacao").Identity()
                .WithColumn("mdfeSeguroCarga_id").AsInt32().NotNullable().ForeignKey("fk_mdfe_seguro_carga__mdfe_seguro_averbacao", "mdfe_seguro_carga", "id")
                .WithColumn("averbacao").AsAnsiString(40).NotNullable();
        }

        public override void Down()
        {
        }
    }
}