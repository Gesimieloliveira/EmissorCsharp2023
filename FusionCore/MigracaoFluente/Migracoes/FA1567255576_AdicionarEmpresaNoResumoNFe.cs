using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1567255576)]
    public class FA1567255576_AdicionarEmpresaNoResumoNFe : Migration
    {
        public override void Up()
        {
            Alter.Table("mde_resumo").AddColumn("empresa_id").AsInt16()
                .NotNullable();

            Create.ForeignKey("fk_mde_resumo_to_empresa")
                .FromTable("mde_resumo").InSchema("dbo").ForeignColumn("empresa_id")
                .ToTable("empresa").InSchema("dbo").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}