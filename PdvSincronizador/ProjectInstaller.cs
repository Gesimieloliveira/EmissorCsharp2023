using System.ComponentModel;
using System.Configuration.Install;

namespace FusionSincronizador
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
