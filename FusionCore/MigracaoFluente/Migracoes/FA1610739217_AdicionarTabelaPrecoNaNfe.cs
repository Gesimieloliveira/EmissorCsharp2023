using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1610739217)]
    public class FA1610739217_AdicionarTabelaPrecoNaNfe : Migration
    {
        public override void Up()
        {
            Alter.Table("nfe")
                .AddColumn("tabelaPreco_id")
                .AsInt32().ForeignKey("fk_nfe__tabela_preco", "tabela_preco", "id")
                .Nullable();
        }

        public override void Down()
        {
        }
    }
}