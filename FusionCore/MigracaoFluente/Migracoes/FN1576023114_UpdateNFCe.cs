using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1576023114)]
    public class FN1576023114_UpdateNFCe : Migration
    {
        public override void Up()
        {
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