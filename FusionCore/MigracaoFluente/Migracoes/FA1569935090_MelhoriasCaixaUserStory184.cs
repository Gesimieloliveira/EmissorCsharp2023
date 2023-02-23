using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1569935090)]
    public class FA1569935090_MelhoriasCaixaUserStory184 : Migration
    {
        public override void Up()
        {
            AdaptarEstruturaCaixaIndividual();
            MigracaoDadosAberturaDeCaixa();
            MigracaoDadosRegistroCaixaIndividualFechado();
            MigracaoDadosRegistroCaixaIndividualAberto();
            MigracaoValoresTotaisCaixaIndividual();
            DroparTabelasLegadas();
        }

        public override void Down()
        {
        }

        private void AdaptarEstruturaCaixaIndividual()
        {
            Alter.Table("caixa_individual")
                .AddColumn("saldoCalculado").AsDecimal(15, 2).NotNullable().SetExistingRowsTo(0.00)
                .AddColumn("saldoInformado").AsDecimal(15, 2).NotNullable().SetExistingRowsTo(0.00);

            Create.Table("caixa_individual_fluxo")
                .WithColumn("id").AsGuid().PrimaryKey("pk_caixa_individual_fluxo")
                .WithColumn("ordem").AsInt64().Identity().NotNullable()
                .WithColumn("caixaIndividual_id").AsGuid().NotNullable()
                .WithColumn("usuario_id").AsInt32().NotNullable()
                .WithColumn("dataCriacao").AsDateTime().NotNullable()
                .WithColumn("dataOperacao").AsDateTime().NotNullable()
                .WithColumn("valorOperacao").AsDecimal(15, 2).NotNullable()
                .WithColumn("tipoOperacao").AsInt16().NotNullable()
                .WithColumn("tipoPagamento").AsInt16().NotNullable()
                .WithColumn("origemEvento").AsInt16().NotNullable()
                .WithColumn("ehUmEstorno").AsBoolean().NotNullable()
                .WithColumn("historico").AsAnsiString(255).NotNullable();

            Create.ForeignKey("fk_caixa_individual_fluxo_to_caixa_individual")
                .FromTable("caixa_individual_fluxo").ForeignColumn("caixaIndividual_id")
                .ToTable("caixa_individual").PrimaryColumn("id");
        }

        private void MigracaoDadosAberturaDeCaixa()
        {
            Execute.Sql(@"
                insert into caixa_individual_fluxo(id,caixaIndividual_id,usuario_id,dataCriacao,dataOperacao,valorOperacao,tipoOperacao,tipoPagamento,origemEvento,ehUmEstorno,historico)
                select NEWID(), ci.id, ci.usuario_id, ci.dataAbertura, ci.dataAbertura, ci.saldoInicial, 0, 0, 6, 0, 'abertura de caixa'	
                from caixa_individual ci"
            );
        }

        private void MigracaoDadosRegistroCaixaIndividualFechado()
        {
            Execute.Sql(@"
                insert into caixa_individual_fluxo(id,caixaIndividual_id,usuario_id,dataCriacao,dataOperacao,valorOperacao,tipoOperacao,tipoPagamento,origemEvento,ehUmEstorno,historico) 
                select newid(), ccr.caixaCalculo_id, cr.usuarioCriacao_id, cr.dataCriacao, cr.dataCriacao, cr.valorRegistro, cr.tipoOperacao, cr.tipoPagamento, cr.origemEvento, cr.ehUmEstorno, 'migracao registro caixa' 
                from caixa_calculo_registro ccr
                inner join caixa_individual ci on ci.id = ccr.caixaCalculo_id
                inner join caixa_registro cr on cr.id = ccr.caixaRegistro_id"
            );

            Execute.Sql("update caixa_individual_fluxo set valorOperacao = (valorOperacao * -1) where tipoOperacao = 1");
        }

        private void MigracaoDadosRegistroCaixaIndividualAberto()
        {
            Execute.Sql(@"
                insert into caixa_individual_fluxo(id,caixaIndividual_id,usuario_id,dataCriacao,dataOperacao,valorOperacao,tipoOperacao,tipoPagamento,origemEvento,ehUmEstorno,historico) 
                select newid(), ci.id, cr.usuarioCriacao_id, cr.dataCriacao, cr.dataCriacao, cr.valorRegistro, cr.tipoOperacao, cr.tipoPagamento, cr.origemEvento, cr.ehUmEstorno, 'migracao registro caixa' 
                from caixa_individual ci
                inner join caixa_registro cr on ci.usuario_id = cr.usuarioCriacao_id and cr.dataCriacao >= ci.dataAbertura
                where ci.estado = 0;
            ");

            Execute.Sql("update caixa_individual_fluxo set valorOperacao = (valorOperacao * -1) where tipoOperacao = 1");
        }

        private void MigracaoValoresTotaisCaixaIndividual()
        {
            Execute.Sql(@"
                update ci set ci.saldoCalculado = cc.saldoAtual, ci.saldoInformado = cc.saldoAtual
                from caixa_individual ci
                inner join caixa_calculo cc on ci.id = cc.caixaIndividual_id"
            );
        }

        private void DroparTabelasLegadas()
        {
            Delete.Table("caixa_calculo_total");
            Delete.Table("caixa_calculo_registro");
            Delete.Table("caixa_calculo");
            Delete.Table("caixa_registro");
        }
    }
}