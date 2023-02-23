using FireSharp.Interfaces;

namespace FusionCore.Seguranca.LicenciamentoOnline.Config
{
    public interface IEndpointCfg
    {
        IFirebaseConfig GetConfig();
    }
}