using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FusionCore.FusionAdm.Setup.Conexao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Setup;
using FusionLibrary.VisaoModel;

namespace Fusion.Conversor.Views.SGBD
{
    public sealed class GerenciarSGBDConexto : ViewModel
    {
        private readonly ConfiguradorConexao _configurador = new ConfiguradorConexao();

        public string NomeBancoNovo
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string NomeBancoBackupear
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public IEnumerable<string> BancosExistentes
        {
            get => GetValue<IEnumerable<string>>();
            set => SetValue(value);
        }

        public void CarregarBancosExistentes()
        {
            var dbUtility = CriaDatabaseUtility();

            BancosExistentes = dbUtility.ListaDatabasesExistentes();
        }

        private DatabaseUtility CriaDatabaseUtility()
        {
            var cfg = _configurador.LerArquivo();

            return new DatabaseUtility(cfg.ToCfg());
        }

        public void FazerBackup(string destino)
        {
            if (BancosExistentes.All(i => i != NomeBancoBackupear))
            {
                throw new InvalidOperationException("Banco de dados selecionado não existe na listagem atual");
            }

            var dbUtility = CriaDatabaseUtility();

            dbUtility.BackupDatabase(NomeBancoBackupear, destino);
        }

        public void CriarNovoBancoDados()
        {
            var arquivoBak = PathDatabaseBak();

            if (NomeBancoNovo == null || NomeBancoNovo.Length < 2)
            {
                throw new InvalidOperationException("Nome para o novo banco muito curto, preciso de no mínimo 2 caracteres");
            }

            if (!File.Exists(arquivoBak))
            {
                throw new InvalidOperationException($"Arquivo.bak não localizado para restauração em: {arquivoBak}");
            }

            var dbUtility = CriaDatabaseUtility();

            if (dbUtility.DatabaseExiste(NomeBancoNovo))
            {
                throw new InvalidOperationException("Que pena... Já existe um banco de dados com este nome.");
            }

            var restorecfg = new RestoreConfig(arquivoBak, NomeBancoNovo);

            dbUtility.RestoreDatabase(restorecfg, null);
        }

        private string PathDatabaseBak()
        {
            var path = Path.Combine(DiretorioAssembly.GetPastaAssembly(), @"Resources\FusionAdm.bak");

#if DEBUG
            var setupFile = path.Replace(
                @"Fusion.Conversor\bin\Debug\Resources\FusionAdm.bak", 
                @"_setup\files\FusionAdm.bak"
            );

            File.Copy(setupFile, path, true);
#endif

            return path;
        }
    }
}