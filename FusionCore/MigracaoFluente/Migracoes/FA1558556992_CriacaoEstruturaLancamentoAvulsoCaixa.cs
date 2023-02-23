using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1558556992)]
    public class FA1558556992_CriacaoEstruturaLancamentoAvulsoCaixa : Migration
    {
        public override void Up()
        {
            CriarTabelaLancamentoAvulso();
        }

        private void CriarTabelaLancamentoAvulso()
        {
            Create.Table("caixa_lancamento")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey("pk_caixa_lancamento")
                .WithColumn("dataCriacao").AsDateTime2().NotNullable()
                .WithColumn("empresa_id").AsInt16().NotNullable()
                .WithColumn("usuario_id").AsInt32().NotNullable()
                .WithColumn("tipoOperacao").AsByte().NotNullable()
                .WithColumn("motivo").AsString().NotNullable()
                .WithColumn("valor").AsDecimal(15, 2).NotNullable();

            Create.ForeignKey("fk_caixa_lancamento_to_empresa")
                .FromTable("caixa_lancamento").ForeignColumn("empresa_id")
                .ToTable("empresa").PrimaryColumn("id");

            Create.ForeignKey("fk_caixa_lancamento_to_usuario")
                .FromTable("caixa_lancamento").ForeignColumn("usuario_id")
                .ToTable("usuario").PrimaryColumn("id");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}