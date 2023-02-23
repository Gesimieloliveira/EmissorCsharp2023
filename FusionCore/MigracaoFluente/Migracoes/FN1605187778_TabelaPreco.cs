using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1605187778)]
    public class FN1605187778_TabelaPreco : Migration
    {
        public override void Up()
        {
            Create.Table("tabela_preco")
                .WithColumn("id").AsInt32().PrimaryKey("pk_tabela_preco")
                .WithColumn("descricao").AsAnsiString(120).NotNullable()
                .WithColumn("tipoAjustePreco").AsByte().NotNullable()
                .WithColumn("percentualAjuste").AsDecimal(15, 2).NotNullable()
                .WithColumn("apenasItensDaLista").AsBoolean().NotNullable();

            Create.Table("ajuste_diferenciado")
                .WithColumn("id").AsInt32().PrimaryKey("pk_ajuste_diferenciado")
                .WithColumn("tabelaPreco_id").AsInt32().ForeignKey("fk_ajuste_diferenciado__tabela_preco", "tabela_preco", "id").NotNullable()
                .WithColumn("produto_id").AsInt32().ForeignKey("fk_ajuste_diferenciado__produto", "produto", "id").NotNullable()
                .WithColumn("percentualAjuste").AsDecimal(15, 2).NotNullable();
        }

        public override void Down()
        {
        }
    }
}