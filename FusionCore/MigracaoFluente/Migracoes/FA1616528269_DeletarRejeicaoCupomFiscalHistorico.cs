using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1616528269)]
    public class FA1616528269_DeletarRejeicaoCupomFiscalHistorico : Migration
    {
        public override void Up()
        {
            Delete.Column("houveRejeicao").FromTable("cupom_fiscal_historico").InSchema("dbo");
            Delete.Column("rejeicao").FromTable("cupom_fiscal_historico").InSchema("dbo");
        }

        public override void Down()
        {
        }
    }
}