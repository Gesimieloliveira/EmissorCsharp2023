using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1573481440)]
    public class FN1573481440_DeletarConfigCertDoTerminal : Migration
    {
        public override void Up()
        {

            Execute.Sql(@"INSERT INTO [dbo].[certificado_digital]
           ([empresa_id]
           ,[tipo]
           ,[serialRepositorio]
           ,[caminhoArquivo]
           ,[senha])
             select e.id, 
	         ct.tipoCertificadoDigital, 
	         ct.serialNumberCertificado,
	         ct.arquivoCertificado, 
	         ct.senhaCertificado
		    from configuracao_terminal as ct
		    inner join emissor_fiscal ef on ef.id = ct.emissorFiscal_id
		    inner join empresa e on e.id = ef.empresa_id");

            Delete.Column("arquivoCertificado").FromTable("configuracao_terminal");
            Delete.Column("senhaCertificado").FromTable("configuracao_terminal");
            Delete.Column("serialNumberCertificado").FromTable("configuracao_terminal");
            Delete.Column("tipoCertificadoDigital").FromTable("configuracao_terminal");
        }

        public override void Down()
        {
        }
    }
}