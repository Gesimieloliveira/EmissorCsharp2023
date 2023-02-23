using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1577476353)]
    public class FN1577476353_AjusteTipoDocumentoParaCompatibilidadeComObjeto : Migration
    {
        public override void Up()
        {
            Alter.Table("tipo_documento")
                .AddColumn("alteradoEm").AsDateTime().NotNullable().SetExistingRowsTo(DateTime.Now)
                .AddColumn("formaPagamento").AsInt16().NotNullable().SetExistingRowsTo(9);
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}