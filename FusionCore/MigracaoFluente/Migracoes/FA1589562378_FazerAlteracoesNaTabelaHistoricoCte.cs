using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1589562378)]
    public class FA1589562378_FazerAlteracoesNaTabelaHistoricoCte : Migration
    {
        public override void Up()
        {
            Alter.Table("cte_emissao_historico").AddColumn("statusConsultaRecibo").AsByte().NotNullable()
                .WithDefaultValue(0);
            Alter.Table("cte_emissao_historico").AddColumn("codigoAutorizacao").AsInt16().NotNullable().WithDefaultValue(0);
            Alter.Table("cte_emissao_historico").AddColumn("motivo").AsAnsiString(255).NotNullable().WithDefaultValue(string.Empty);
            Alter.Table("cte_emissao_historico").AddColumn("recebidoEm").AsDateTime().Nullable();

            Delete.DefaultConstraint().OnTable("cte_emissao_historico").InSchema("dbo").OnColumn("statusConsultaRecibo");
            Delete.DefaultConstraint().OnTable("cte_emissao_historico").InSchema("dbo").OnColumn("codigoAutorizacao");
            Delete.DefaultConstraint().OnTable("cte_emissao_historico").InSchema("dbo").OnColumn("motivo");
        }

        public override void Down()
        {
        }
    }
}