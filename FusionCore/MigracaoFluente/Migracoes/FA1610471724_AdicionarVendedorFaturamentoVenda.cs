using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1610471724)]
    public class FA1610471724_AdicionarVendedorFaturamentoVenda : Migration
    {
        public override void Up()
        {
            Create.Table("faturamento_vendedor")
                .WithColumn("faturamentoVenda_id")
                .AsInt32()
                .PrimaryKey("pk_faturamento_vendedor")
                .ForeignKey("fk_faturamento_vendedor__faturamento_venda", "faturamento_venda", "id")
                .WithColumn("vendedor_id")
                .AsInt32()
                .ForeignKey("fk_faturamento_vendedor__pessoa_vendedor", "pessoa_vendedor", "pessoa_id")
                .NotNullable();
        }

        public override void Down()
        {
        }
    }
}