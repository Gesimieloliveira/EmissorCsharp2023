using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1556977228)]
    public class FA1556977228_AtualizacaoPisCofins : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update produto 
                set produto.situacaoTributariaPis_id = '49',
                produto.situacaoTributariaCofins_id = '49'
                where
                produto.situacaoTributariaCofins_id = '50'
                or 
                produto.situacaoTributariaPis_id = '50'
                or
                produto.situacaoTributariaCofins_id = '51'
                or 
                produto.situacaoTributariaPis_id = '51'
                or
                produto.situacaoTributariaCofins_id = '52'
                or 
                produto.situacaoTributariaPis_id = '52'
                or
                produto.situacaoTributariaCofins_id = '53'
                or 
                produto.situacaoTributariaPis_id = '53'
                or
                produto.situacaoTributariaCofins_id = '54'
                or 
                produto.situacaoTributariaPis_id = '54'
                or
                produto.situacaoTributariaCofins_id = '55'
                or 
                produto.situacaoTributariaPis_id = '55'
                or
                produto.situacaoTributariaCofins_id = '56'
                or 
                produto.situacaoTributariaPis_id = '56'
                or
                produto.situacaoTributariaCofins_id = '60'
                or 
                produto.situacaoTributariaPis_id = '60'
                or 
                produto.situacaoTributariaCofins_id = '61'
                or 
                produto.situacaoTributariaPis_id = '61'
                or
                produto.situacaoTributariaCofins_id = '62'
                or 
                produto.situacaoTributariaPis_id = '62'
                or
                produto.situacaoTributariaCofins_id = '63'
                or 
                produto.situacaoTributariaPis_id = '63'
                or
                produto.situacaoTributariaCofins_id = '64'
                or 
                produto.situacaoTributariaPis_id = '64'
                or
                produto.situacaoTributariaCofins_id = '65'
                or 
                produto.situacaoTributariaPis_id = '65'
                or
                produto.situacaoTributariaCofins_id = '66'
                or 
                produto.situacaoTributariaPis_id = '66'
                or
                produto.situacaoTributariaCofins_id = '67'
                or 
                produto.situacaoTributariaPis_id = '67'
                or
                produto.situacaoTributariaCofins_id = '70'
                or 
                produto.situacaoTributariaPis_id = '70'
                or
                produto.situacaoTributariaCofins_id = '71'
                or 
                produto.situacaoTributariaPis_id = '71'
                or
                produto.situacaoTributariaCofins_id = '72'
                or 
                produto.situacaoTributariaPis_id = '72'
                or
                produto.situacaoTributariaCofins_id = '73'
                or 
                produto.situacaoTributariaPis_id = '73'
                or
                produto.situacaoTributariaCofins_id = '74'
                or 
                produto.situacaoTributariaPis_id = '74'
                or
                produto.situacaoTributariaCofins_id = '75'
                or 
                produto.situacaoTributariaPis_id = '75'
                or
                produto.situacaoTributariaCofins_id = '98'
                or 
                produto.situacaoTributariaPis_id = '98'
                or
                produto.situacaoTributariaCofins_id = '99'
                or 
                produto.situacaoTributariaPis_id = '99'
                ");
        }

        public override void Down()
        {
        }
    }
}