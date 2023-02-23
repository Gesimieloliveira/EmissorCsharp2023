using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1600350203)]
    public class FA1600350203_DeletarRegistroEmissaoMdfe : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"delete m from mdfe_emissao m
inner join mdfe n
on
n.id = m.mdfe_id
where n.status = 0 or n.status = 1");
        }

        public override void Down()
        {
        }
    }
}