using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1602611278)]
    public class FA1602611278_IndicadorIEAceitarNotNull : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update pessoa 
                set pessoa.indicadorIe = 1
                where 
                pessoa.cnpj != ''
                and (pessoa.inscricaoEstadual != '' and pessoa.inscricaoEstadual != 'ISENTO');

                update pessoa
                set pessoa.indicadorIe = 1
                where
                pessoa.cpf != '' and (pessoa.inscricaoEstadual != '' and pessoa.inscricaoEstadual != 'ISENTO');

                update pessoa
                set pessoa.indicadorIe = 2
                where
                pessoa.inscricaoEstadual = 'ISENTO';


                update pessoa
                set pessoa.indicadorIe = 9
                where
                pessoa.cpf != ''
                and pessoa.inscricaoEstadual = '';

                update pessoa 
                set pessoa.indicadorIe = 9
                where 
                (pessoa.cnpj = '' and pessoa.cpf = '' and pessoa.inscricaoEstadual != '') or (pessoa.cpf = '' and pessoa.cnpj = '') 
                ");

            Delete.DefaultConstraint().OnTable("pessoa")
                .InSchema("dbo").OnColumn("indicadorIe");
        }

        public override void Down()
        {
        }
    }
}