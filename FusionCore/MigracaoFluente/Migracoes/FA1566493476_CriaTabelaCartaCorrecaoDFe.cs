using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1566493476)]
    public class FA1566493476_CriaTabelaCartaCorrecaoDFe : Migration
    {
        public override void Up()
        {
            Create.Table("mde_carta_correcao")
                .WithColumn("id").AsInt32().Identity()
                .WithColumn("xml").AsCustom("xml").NotNullable()
                .WithColumn("mdeResumo_id").AsInt32().NotNullable();

            Create.ForeignKey("fk_mde_carta_correcao_to_mde_resumo")
                .FromTable("mde_carta_correcao").InSchema("dbo").ForeignColumn("mdeResumo_id")
                .ToTable("mde_resumo").InSchema("dbo").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}