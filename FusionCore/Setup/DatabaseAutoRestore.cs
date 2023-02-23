using System;
using System.IO;
using System.Reflection;
using log4net;

namespace FusionCore.Setup
{
    public class DatabaseAutoRestore
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DatabaseUtility _databaseUtility;
        private readonly AutoRestorePath _paths;

        public DatabaseAutoRestore(DatabaseUtility databaseUtility, AutoRestorePath paths)
        {
            _databaseUtility = databaseUtility;
            _paths = paths;
        }

        public void RestaurarAdm(string dbName)
        {
            var bakRestore = Path.Combine(_paths.CaminhoArquivosBak, Bak.FusionAdm);

            Restaurar(dbName, bakRestore);
        }

        private void Restaurar(string dbName, string bakRestore)
        {
            if (!_databaseUtility.EhLocalHost)
            {
                throw new InvalidOperationException($"Falha restaurar {dbName}: Não é possível restaurar um banco apartir de um Terminal!");
            }

            try
            {
                var restoreCfg = new RestoreConfig(bakRestore, dbName);

                _databaseUtility.RestoreDatabase(restoreCfg, _paths.CaminhoBancoDados);

                File.Delete(bakRestore);
            }
            catch (Exception e)
            {
                _log.Error("Falha retaurar bak", e);

                throw new InvalidOperationException(
                    $"Falha restaurar {dbName} em {bakRestore}");
            }
        }

        public void RestaurarRelatorio(string dbName)
        {
            var bakRestore = Path.Combine(_paths.CaminhoArquivosBak, Bak.FusionRelatorio);

            if (File.Exists(bakRestore))
            {
                Restaurar(dbName, bakRestore);
            }
        }

        public void RestaurarPdv(string dbName)
        {
            var bakRestore = Path.Combine(_paths.CaminhoArquivosBak, Bak.FusionPdv);

            Restaurar(dbName, bakRestore);
        }

        public void RestaurarNfce(string dbName)
        {
            var bakRestore = Path.Combine(_paths.CaminhoArquivosBak, Bak.FusionNfce);

            Restaurar(dbName, bakRestore);
        }
    }
}