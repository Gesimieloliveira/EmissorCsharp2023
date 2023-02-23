using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1590518650)]
    public class FA1590518650_AdicionarVendedorNfce_Adm : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce")
                .AddColumn("vendedor_id").AsInt32().Nullable()
                .ForeignKey("fk_nfce_vendedor", "pessoa_vendedor", "pessoa_id");
        }

        public override void Down()
        {
        }
    }
}