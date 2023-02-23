using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1577468378)]
    public class FA1577468378_PreencherFlagImportacaoMDe : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update mde set mde.isImportada = 1
                from mde_resumo as mde
                inner join nf_compra as nf 
                on mde.chave = nf.chave
                where mde.chave = nf.chave");
        }

        public override void Down()
        {
        }
    }
}