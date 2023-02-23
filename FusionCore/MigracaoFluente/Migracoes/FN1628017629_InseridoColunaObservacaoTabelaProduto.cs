using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1628017629)]
    public class FN1628017629_InseridoColunaObservacaoTabelaProduto : Migration
    {
        public override void Up()
        {
            Alter.Table("produto")
                .AddColumn("observacao").AsString(500).Nullable().SetExistingRowsTo(string.Empty);
        }

        public override void Down()
        {
        }
    }
}