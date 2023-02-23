using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1599589953)]
    public class FN1599589953_DeletarEmissaoNfCe : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"delete m from nfce_emissao m
inner join nfce n
on
n.id = m.nfce_id
where (n.status = 0 or n.status = 3) ");
        }

        public override void Down()
        {
        }
    }
}