using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1617988806)]
    public class FA1617988806_AdicionarContingenciaCupomFiscal : Migration
    {
        public override void Up()
        {
            Alter.Table("cupom_fiscal").AddColumn("contingencia_id")
                .AsInt32().Nullable();
        }

        public override void Down()
        {
        }
    }
}