using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1569267753)]
    public class FA1569267753_MudarControleNumeroFiscalMDFe : Migration
    {
        public override void Up()
        {
            Alter.Table("mdfe").AddColumn("serieEmissao").AsInt16().NotNullable().WithDefaultValue(0);
            Alter.Table("mdfe").AddColumn("numeroFiscalEmissao").AsInt32().NotNullable().WithDefaultValue(0);
            Alter.Table("mdfe").AddColumn("codigoNumericoEmissao").AsInt32().NotNullable().WithDefaultValue(0);

            Execute.Sql(@"update mdfe 
                set serieEmissao = mdfe_emissao.serie,
                numeroFiscalEmissao = mdfe_emissao.numeroDocumento,
                codigoNumericoEmissao = mdfe_emissao.codigoNumerico
                from mdfe inner join mdfe_emissao
                on mdfe.id = mdfe_emissao.mdfe_id");

            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("serieEmissao");
            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("numeroFiscalEmissao");
            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("codigoNumericoEmissao");
        }

        public override void Down()
        {
        }
    }
}