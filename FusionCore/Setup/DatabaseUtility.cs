using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace FusionCore.Setup
{
    public class DatabaseUtility
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConexaoCfg _cfg;

        public bool EhLocalHost
            => Regex.IsMatch(_cfg.Servidor, @"^(localhost|127\.0\.0\.1|\.).*", RegexOptions.IgnoreCase);

        public DatabaseUtility(IConexaoCfg cfg)
        {
            _cfg = cfg;
        }

        private ServerConnection CriaConexao()
        {
            var conString = CriraStringConexaoApenasComServidor();
            var con = new SqlConnection(conString);
            var server = new ServerConnection(con);

            return server;
        }

        private string CriraStringConexaoApenasComServidor()
        {
            var sb = new StringBuilder();

            sb.Append($"Data Source={_cfg.GetDataSource()};");
            sb.Append("Persist Security Info=True;");
            sb.Append($"User ID={_cfg.Usuario};");
            sb.Append($"Password={_cfg.Senha};");
            sb.Append($"Timeout=30;");
            sb.Append("Pooling=false;");
            sb.Append("Language=English;");

            return sb.ToString();
        }

        public TesteConexaoResult TesteConexao()
        {
            var conexao = CriaConexao();

            try
            {
                conexao.Connect();
                return TesteConexaoResult.Sucesso;
            }
            catch (Exception e)
            {
                _log.Warn("Teste Conexao", e);
                return new TesteConexaoResult($"{e.Message} - {e.InnerException?.Message ?? "Sem Detalhes"}");
            }
            finally
            {
                if (conexao?.IsOpen == true)
                {
                    conexao.Disconnect();
                }
            }
        }

        public IEnumerable<string> ListaDatabasesExistentes()
        {
            var conexao = CriaConexao();

            try
            {
                var serverSql = new Server(conexao);
                var databases = serverSql.Databases;

                var lista = new List<string>();

                foreach (Database db in databases)
                {
                    lista.Add(db.Name);
                }

                return lista;
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw;
            }
            finally
            {
                if (conexao.IsOpen)
                {
                    conexao.Disconnect();
                }
            }
        }

        public bool DatabaseExiste(string banco)
        {
            try
            {
                var databases = ListaDatabasesExistentes();

                return databases.Any(db => db.ToUpper() == banco.ToUpper());
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw;
            }
        }

        public void RestoreDatabase(RestoreConfig cfg, string restorePath)
        {
            var conexao = CriaConexao();

            try
            {
                var server = new Server(conexao);
                var databaseName = cfg.BancoDados;

                var res = new Restore
                {
                    Database = databaseName,
                    ReplaceDatabase = false,
                    NoRecovery = false
                };

                res.Devices.AddDevice(cfg.ArquivoBak, DeviceType.File);
                RealocarArquivosBak(res, server, cfg.BancoDados, restorePath);
                res.SqlRestore(server);
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw;
            }
            finally
            {
                conexao.Disconnect();
            }
        }

        private static void RealocarArquivosBak(
            Restore res,
            Server srv,
            string databaseName,
            string restorePath
        )
        {
            if (string.IsNullOrWhiteSpace(restorePath))
            {
                restorePath = srv.MasterDBPath;
            }

            if (!Directory.Exists(restorePath))
            {
                Directory.CreateDirectory(restorePath);
            }

            var mdf = res.ReadFileList(srv).Rows[0][0].ToString();
            var ldf = res.ReadFileList(srv).Rows[1][0].ToString();

            var dataFile = new RelocateFile(mdf, Path.Combine(restorePath, $"{databaseName}.mdf"));
            var ldfFile = new RelocateFile(ldf, Path.Combine(restorePath, $"{databaseName}.ldf"));

            res.RelocateFiles.Add(dataFile);
            res.RelocateFiles.Add(ldfFile);
        }

        public void BackupDatabase(string dbName, string destino)
        {
            var conexao = CriaConexao();

            try
            {
                var server = new Server(conexao);
                var versaoSql = ObterVersaoSql(server);
                var fileName = Path.Combine(destino, $"{dbName}_{DateTime.Now:yyyyMMdd_HHmmss}_{versaoSql}.bak");

                var backup = new Backup
                {
                    Incremental = false,
                    Action = BackupActionType.Database,
                    Database = dbName,
                    Checksum = true
                };

                backup.Devices.AddDevice(fileName, DeviceType.File);
                backup.SqlBackup(server);
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw;
            }
            finally
            {
                if (conexao.IsOpen)
                {
                    conexao.Disconnect();
                }
            }
        }

        public void BackupRemoto(string dbName)
        {
            var conexao = CriaConexao();
            

            try
            {
                var server = new Server(conexao);
                var versaoSql = ObterVersaoSql(server);
                var fileName = Path.Combine(server.BackupDirectory, $"{dbName}_{DateTime.Now:yyyyMMdd_HHmmss}_{versaoSql}.bak");

                var backup = new Backup
                {
                    Incremental = false,
                    Action = BackupActionType.Database,
                    Database = dbName,
                    Checksum = true
                };

                backup.Devices.AddDevice(fileName, DeviceType.File);
                backup.SqlBackup(server);
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw;
            }
            finally
            {
                if (conexao.IsOpen)
                {
                    conexao.Disconnect();
                }
            }
        }

        public string ObterVersaoSql(Server server)
        {

            var sqlLevel = server.Information.ProductLevel.ToString();
            var sqlVersionMajor = server.Information.VersionMajor.ToString();
            string versaoFinal;

            switch (sqlVersionMajor)
            {
                case "10":
                    versaoFinal = "v2008R2_" + sqlLevel;
                    break;

                case "11":
                    versaoFinal = "v2012_" + sqlLevel;
                    break;

                case "12":
                    versaoFinal = "v2014_" + sqlLevel;
                    break;

                case "13":
                    versaoFinal = "v2016_" + sqlLevel;
                    break;

                case "14":
                    versaoFinal = "v2017_" + sqlLevel;
                    break;

                case "15":
                    versaoFinal = "v2019_" + sqlLevel;
                    break;

                default:
                    versaoFinal = "";
                    break;
            }

            return versaoFinal;
        }
    }
}