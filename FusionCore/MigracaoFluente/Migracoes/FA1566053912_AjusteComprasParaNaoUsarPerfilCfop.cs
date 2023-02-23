using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1566053912)]
    public class FA1566053912_AjusteComprasParaNaoUsarPerfilCfop : Migration
    {
        public override void Up()
        {
            Alter.Table("nf_compra_item").AddColumn("cfop_id").AsAnsiString(4).Nullable();

            Create.ForeignKey("fk_nf_compra_item_to_cfop")
                .FromTable("nf_compra_item").ForeignColumn("cfop_id")
                .ToTable("cfop").PrimaryColumn("id");

            Execute.Sql("update i set i.cfop_id = p.cfop_id from nf_compra_item i inner join perfil_cfop p on p.id = i.perfilCfop_id;");

            Alter.Table("nf_compra_item").AlterColumn("cfop_id").AsAnsiString(4).NotNullable();

            Delete.ForeignKey("fk_nf_compra_item__cfop").OnTable("nf_compra_item");
            Delete.Column("perfilCfop_id").FromTable("nf_compra_item");
        }

        public override void Down()
        {
        }
    }
}