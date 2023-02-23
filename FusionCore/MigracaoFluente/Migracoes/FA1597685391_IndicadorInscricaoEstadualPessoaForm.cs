using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1597685391)]
    public class FA1597685391_IndicadorInscricaoEstadualPessoaForm : Migration
    {
        public override void Up()
        {
            Alter.Table("pessoa").AddColumn("indicadorIe").AsByte().Nullable();

            Execute.Sql(@"update pessoa 
                set indicadorIe = case when pessoa.cnpj != '' and pessoa.inscricaoEstadual != '' 
                then 1 else 9 end");
        }

        public override void Down()
        {
        }
    }
}