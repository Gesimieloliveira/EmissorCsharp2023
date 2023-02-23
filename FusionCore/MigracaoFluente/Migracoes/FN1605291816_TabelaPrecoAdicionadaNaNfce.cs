using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1605291816)]
    public class FN1605291816_TabelaPrecoAdicionadaNaNfce : Migration
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