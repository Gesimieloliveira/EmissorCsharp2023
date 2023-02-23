using FusionLibrary.VisaoModel;
using FusionCore.Preferencias;
using FusionCore.Preferencias.Repositorios;
using FusionCore.Helpers.Maquina;

namespace FusionNfce.Visao.Backup
{
    class BackupFormModel : ViewModel
    {

        public BackupFormModel()
        {

        }

        public PreferenciaSistema Preferencia { get; set; }

        public string Diretorio
        {
            get => GetValue<string>();
            set => SetValue(value);
        }       

        internal void Inicializar()
        {
            using (var sessao = FusionCore.FusionNfce.Sessao.Sistema.SessaoSistemaNfce.SessaoManager.CriaSessao()){
                var repositorio = new RepositorioPreferenciaSistema(sessao);

                Preferencia = repositorio.Buscar(IdMaquinaProvider.Computa(), "backup.local");

                if (Preferencia == null)
                {
                    Preferencia = new PreferenciaSistema("backup.local", "C:\\SistemaFusion\\Backup");              
                    repositorio.Inserir(Preferencia);

                }

                Diretorio = Preferencia.Valor;               
            }
            
        }


    }
}
