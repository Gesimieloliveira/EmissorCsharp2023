using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1551371784)]
    public class FN1551371784_SetDefaultPisCofins : Migration
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