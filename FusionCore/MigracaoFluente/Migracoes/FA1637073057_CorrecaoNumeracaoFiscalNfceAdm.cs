using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1637073057)]
    public class FA1637073057_CorrecaoNumeracaoFiscalNfceAdm : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update nfce set nfce.numeroFiscal = nfce_emissao.numeroDocumento,
                nfce.serie = nfce_emissao.serie,
                nfce.codigoNumerico = nfce_emissao.codigoNumerico
                from nfce inner join nfce_emissao
                on nfce.id = nfce_emissao.nfce_id
                where numeroFiscal != numeroDocumento");
        }

        public override void Down()
        {
        }
    }
}