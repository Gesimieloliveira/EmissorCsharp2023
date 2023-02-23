using System;
using FluentMigrator;
using FusionCore.NfceSincronizador.Flags;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1551459836)]
    public class FA1551459836_AdicionarTiposDeDocumentoParaSyncNfce : Migration
    {
        public override void Up()
        {
            var codigoEntidade = (int) EntidadeSincronizavel.TipoDocumento;

            Delete.FromTable("sync_pendente").Row(new {entidade = codigoEntidade });

            Execute.Sql(
                "insert into sync_pendente(referencia, terminalOffline_id, entidade) " +
                $"select td.id, te.id, {codigoEntidade} from tipo_documento td cross join terminal_offline te;"
            );
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}