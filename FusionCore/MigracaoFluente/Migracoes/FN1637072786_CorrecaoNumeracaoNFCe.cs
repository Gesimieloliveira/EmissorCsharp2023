using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1637072786)]
    public class FN1637072786_CorrecaoNumeracaoNFCe : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update nfce set nfce.numeroFiscal = nfce_emissao.numeroDocumento,
                nfce.serie = nfce_emissao.serie,
                nfce.codigoNumerico = nfce_emissao.codigoNumerico,
                nfce.sincronizado = 0
                from nfce inner join nfce_emissao
                on nfce.id = nfce_emissao.nfce_id
                where numeroFiscal != numeroDocumento");
        }

        public override void Down()
        {
        }
    }
}