using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1551371934)]
    public class FA1551371934_SetDefaultPisCofins : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"insert into nfce_item_pis (nfceItem_id, situacaoTributariaPis_id, aliquota, baseCalculo, valor)
                select i.id, '49', 0, 0, 0 from nfce_item as i");

            Execute.Sql(@"insert into nfce_item_cofins (nfceItem_id, situacaoTributariaCofins_id, aliquota, baseCalculo, valor)
                select i.id, '49', 0, 0, 0 from nfce_item as i");
        }

        public override void Down()
        {
        }
    }
}