using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1602873605)]
    public class FA1602873605_AlterarFluxoContaCaixaRetirarFkAdicionarGuidOrigem : Migration
    {
        public override void Up()
        {
            Alter.Table("conta_caixa_fluxo")
                .AddColumn("origem_id").AsGuid().WithDefaultValue(Guid.NewGuid()).NotNullable();

            Execute.Sql(@"update 
	            conta_caixa_fluxo
	            set conta_caixa_fluxo.origem_id = conta_caixa_fluxo.caixaIndividual_id");

            Delete.ForeignKey("fk_conta_caixa_fluxo_to_caixa_individual").OnTable("conta_caixa_fluxo").InSchema("dbo");

            Delete.Column("caixaIndividual_id").FromTable("conta_caixa_fluxo").InSchema("dbo");

            Delete.DefaultConstraint().OnTable("conta_caixa_fluxo").InSchema("dbo").OnColumn("origem_id");
        }

        public override void Down()
        {
        }
    }
}