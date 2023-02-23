using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1576023402)]
    public class FA1576023402_AjustesNfce : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce").AddColumn("tipoEmissao").AsByte().NotNullable().WithDefaultValue(1);
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("tipoEmissao");

            Execute.Sql(@"update nfce set nfce.tipoEmissao = b.tipoEmissao 
                from nfce as a inner join nfce_emissao as b on a.id = b.nfce_id");
            Execute.Sql(@"update nfce set nfce.codigoNumerico = b.codigoNumerico,
                nfce.serie = b.serie,
                nfce.numeroFiscal = b.numeroDocumento
                from nfce as a inner join nfce_emissao as b on a.id = b.nfce_id");
        }

        public override void Down()
        {
        }
    }
}