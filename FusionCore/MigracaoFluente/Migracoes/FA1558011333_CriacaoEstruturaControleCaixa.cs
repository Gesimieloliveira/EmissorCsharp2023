using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1558011333)]
    public class FA1558011333_CriacaoEstruturaControleCaixa : Migration
    {
        public override void Up()
        {
            if (Schema.Table("registro_caixa").Exists())
            {
                //compatibilidade com branchs
                Delete.Table("registro_caixa");
            }

            CriarEtruturaParaRegistroDeCaixa();
            CriarEstruturaParaCaixaIndividual();
            CriarEstruturaParaCalculoCaixa();
            CriarEstruturaParaRegistrosDoCalculo();
            CriarEstruturaParaTotalizacaoDoCalculo();
        }

        private void CriarEtruturaParaRegistroDeCaixa()
        {
            Create.Table("caixa_registro")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey("pk_caixa_registro")
                .WithColumn("dataCriacao").AsDateTime2().NotNullable()
                .WithColumn("empresa_id").AsInt16().NotNullable()
                .WithColumn("usuarioCriacao_id").AsInt32().NotNullable()
                .WithColumn("origemEvento").AsByte().NotNullable()
                .WithColumn("tipoPagamento").AsByte().NotNullable()
                .WithColumn("tipoOperacao").AsByte().NotNullable()
                .WithColumn("valorRegistro").AsDecimal(15, 2).NotNullable()
                .WithColumn("ehUmEstorno").AsBoolean().NotNullable();

            Create.ForeignKey("fk_caixa_registro_to_empresa")
                .FromTable("caixa_registro").ForeignColumn("empresa_id")
                .ToTable("empresa").PrimaryColumn("id");

            Create.ForeignKey("fk_caixa_registro_to_usuario")
                .FromTable("caixa_registro").ForeignColumn("usuarioCriacao_id")
                .ToTable("usuario").PrimaryColumn("id");
        }

        private void CriarEstruturaParaCaixaIndividual()
        {
            Create.Table("caixa_individual")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey("pk_caixa_individual")
                .WithColumn("usuario_id").AsInt32().NotNullable()
                .WithColumn("estado").AsByte().NotNullable()
                .WithColumn("dataAbertura").AsDateTime().NotNullable()
                .WithColumn("saldoInicial").AsDecimal(15,2).NotNullable()
                .WithColumn("dataFechamento").AsDateTime().Nullable();

            Create.ForeignKey("fk_caixa_individual_to_usuario")
                .FromTable("caixa_individual").ForeignColumn("usuario_id")
                .ToTable("usuario").PrimaryColumn("id");
        }

        private void CriarEstruturaParaCalculoCaixa()
        {
            Create.Table("caixa_calculo")
                .WithColumn("caixaIndividual_id").AsGuid().PrimaryKey("pk_caixa_calculo")
                .WithColumn("saldoAtual").AsDecimal(15, 2).NotNullable();

            Create.ForeignKey("fk_caixa_calculo_to_caixa")
                .FromTable("caixa_calculo").ForeignColumn("caixaIndividual_id")
                .ToTable("caixa_individual").PrimaryColumn("id");
        }

        private void CriarEstruturaParaRegistrosDoCalculo()
        {
            Create.Table("caixa_calculo_registro")
                .WithColumn("caixaCalculo_id").AsGuid().PrimaryKey("pk_caixa_calculo_registro")
                .WithColumn("caixaRegistro_id").AsGuid().PrimaryKey("pk_caixa_calculo_registro");

            Create.ForeignKey("fk_caixa_calculo_registro_to_calculo")
                .FromTable("caixa_calculo_registro").ForeignColumn("caixaCalculo_id")
                .ToTable("caixa_calculo").PrimaryColumn("caixaIndividual_id");

            Create.ForeignKey("fk_caixa_calculo_registro_to_registro")
                .FromTable("caixa_calculo_registro").ForeignColumn("caixaRegistro_id")
                .ToTable("caixa_registro").PrimaryColumn("id");
        }

        private void CriarEstruturaParaTotalizacaoDoCalculo()
        {
            Create.Table("caixa_calculo_total")
                .WithColumn("caixaCalculo_id").AsGuid().PrimaryKey("pk_caixa_calculo_total")
                .WithColumn("tipoPagamento").AsByte().PrimaryKey("pk_caixa_calculo_total")
                .WithColumn("totalDasEntradas").AsDecimal(15, 2).NotNullable()
                .WithColumn("totalDasSaidas").AsDecimal(15, 2).NotNullable()
                .WithColumn("totalSaldo").AsDecimal(15, 2).NotNullable();

            Create.ForeignKey("fk_caixa_calculo_total_to_calculo")
                .FromTable("caixa_calculo_total").ForeignColumn("caixaCalculo_id")
                .ToTable("caixa_calculo").PrimaryColumn("caixaIndividual_id");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}