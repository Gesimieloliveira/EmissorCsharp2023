using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1602620169)]
    public class FA1602620169_AdicionandoTipoLancamentoCaixaNoCaixaLancamento : Migration
    {
        public override void Up()
        {
            Alter.Table("caixa_lancamento")
                .AddColumn("tipoLancamentoCaixa").AsByte()
                .NotNullable().WithDefaultValue(1);

            Delete.DefaultConstraint().OnTable("caixa_lancamento")
                .InSchema("dbo").OnColumn("tipoLancamentoCaixa");
        }

        public override void Down()
        {
        }
    }
}