using Fusion.Sessao;
using FusionCore.FusionAdm.Setup.Conexao;
using FusionCore.Helpers.Maquina;
using FusionCore.Preferencias;
using FusionCore.Preferencias.Repositorios;
using FusionCore.Setup;
using FusionWPF.Base.Utils.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Fusion.Visao.Backup
{
    public partial class BackupForm
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;

        public BackupForm()
        {
            DataContext = new BackupFormModel();
            InitializeComponent();
        }

        private BackupFormModel Model => DataContext as BackupFormModel;

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Model.Inicializar();
        }

        private void SearchTextBox(object sender, RoutedEventArgs e)
        {
            using (var fdb = new FolderBrowserDialog())
            {
                var resul = fdb.ShowDialog();

                if (resul != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                var destino = fdb.SelectedPath.ToString();
                Model.Diretorio = destino;

            }

        }

        private void FazerBackup(object sender, RoutedEventArgs e)
        {
            try
            {
                var conexao = ObterDadosDaConexaoAtual();
                var dbUtility = new DatabaseUtility(conexao.ToCfg());
                var destino = Path.Combine(Model.Diretorio);
                dbUtility.BackupDatabase(conexao.BancoDados, destino);

                try
                {
                    using (var sessao = SessaoSistema.Instancia.SessaoManager.CriaSessao())
                    {
                        var repositorio = new RepositorioPreferenciaSistema(sessao);

                        var preferencia = repositorio.Buscar(IdMaquinaProvider.Computa(), "backup.local");

                        if (preferencia == null)
                        {
                            var novaPreferencia = new PreferenciaSistema("backup.local", "C:\\SistemaFusion\\Backup");
                            repositorio.Inserir(novaPreferencia);
                            return;
                        }

                        preferencia.Valor = destino;
                        repositorio.Alterar(preferencia);
                    }
                }
                catch
                {
                    //ignore
                }

                Close();
                DialogBox.MostraInformacao($"Backup {conexao.BancoDados} em {destino}");

            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso("Falha ao fazer backup. Certifique que o sistema tem acesso ao diretório escolhido!");
            }
        }


        private static DadosConexao ObterDadosDaConexaoAtual()
        {
            var configurarConexao = new ConfiguradorConexao();

            if (!configurarConexao.ArquivoExiste())
                throw new InvalidOperationException("Arquivo de conexão não existe, fechar e abrir sistema");

            var conexao = configurarConexao.LerArquivo();
            return conexao;
        }
    }
}
