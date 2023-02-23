using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1647438517)]
    public class FA1647438517_AddTributacaoFederalCteOs : Migration
    {
        public override void Up()
        {
            Create.Table("cte_os_tributacao_federal")
                .WithColumn("cteOs_id").AsInt32().NotNullable()
                .WithColumn("valorPis").AsDecimal(15, 2).NotNullable()
                .WithColumn("valorCofins").AsDecimal(15, 2).NotNullable()
                .WithColumn("valorImpostoRenda").AsDecimal(15, 2).NotNullable()
                .WithColumn("valorInss").AsDecimal(15, 2).NotNullable()
                .WithColumn("valorClss").AsDecimal(15, 2).NotNullable();

            Create.ForeignKey("fk_cte_os__tributacao_federal")
                .FromTable("cte_os_tributacao_federal").ForeignColumn("cteOs_id")
                .ToTable("cte_os").PrimaryColumn("id");

            Alter.Table("cte_os_config_imposto")
                .AddColumn("usarTributacaoFederal").AsBoolean().NotNullable().SetExistingRowsTo(0);
        }

        public override void Down()
        {
            Delete.Table("cte_os_tributacao_federal");
            Delete.Column("usarTributacaoFederal").FromTable("cte_os_config_imposto");
        }
    }
}