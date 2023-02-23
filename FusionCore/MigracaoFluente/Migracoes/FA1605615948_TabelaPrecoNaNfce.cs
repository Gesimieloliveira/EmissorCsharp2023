using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1605615948)]
    public class FA1605615948_TabelaPrecoNaNfce : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce").AddColumn("tabelaPreco_id")
                .AsInt32().Nullable()
                .ForeignKey("fk_nfce__tabela_preco", "tabela_preco", "id");
        }

        public override void Down()
        {
        }
    }
}