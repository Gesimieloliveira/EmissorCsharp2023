using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1596583048)]
    public class FA1596583048_UpdateAverbacaoMDFE : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"INSERT INTO [dbo].[mdfe_seguro_averbacao]
           select seg.id, seg.numeroAverbacao from mdfe_seguro_carga as seg");
            
            Delete.
                Column("numeroAverbacao").FromTable("mdfe_seguro_carga").InSchema("dbo");
        }

        public override void Down()
        {
        }
    }
}