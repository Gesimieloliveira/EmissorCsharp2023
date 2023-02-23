using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1640287144)]
    public class FA1640287144_AddColunaTipoRecebimentoTabelaDocReceberLancamento : Migration
    {
        public override void Up()
        {
            Alter.Table("documento_receber_lancamento").AddColumn("tipoRecebimento").AsByte().Nullable();
            Update.Table("documento_receber_lancamento").Set(new { tipoRecebimento = 0}).Where(new { tipoLancamento = 2});
        }

        public override void Down()
        {
            
        }
    }
}