using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1610720417)]
    public class FA1610720417_AdicionarTabelaPrecoFaturamento : Migration
    {
        public override void Up()
        {
            Alter.Table("faturamento_venda")
                .AddColumn("tabelaPreco_id")
                .AsInt32().ForeignKey("fk_faturamento_venda__tabela_preco", "tabela_preco", "id")
                .Nullable();
        }

        public override void Down()
        {
        }
    }
}