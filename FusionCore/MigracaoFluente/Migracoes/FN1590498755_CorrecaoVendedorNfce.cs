using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1590498755)]
    public class FN1590498755_CorrecaoVendedorNfce : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("fk_nfce_vendedor").OnTable("nfce");
            Delete.Column("vendedorId").FromTable("nfce");

            Alter.Table("nfce")
                .AddColumn("vendedor_id").AsInt32().Nullable()
                .ForeignKey("fk_nfce_vendedor", "vendedor", "id");
        }

        public override void Down()
        {
        }
    }
}