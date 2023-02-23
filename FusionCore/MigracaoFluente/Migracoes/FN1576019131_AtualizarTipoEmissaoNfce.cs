using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1576019131)]
    public class FN1576019131_AtualizarTipoEmissaoNfce : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update nfce set nfce.tipoEmissao = b.tipoEmissao 
                from nfce as a inner join nfce_emissao as b on a.id = b.nfce_id");
        }

        public override void Down()
        {
        }
    }
}