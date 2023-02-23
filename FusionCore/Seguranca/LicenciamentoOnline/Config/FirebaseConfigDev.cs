using FireSharp.Config;
using FireSharp.Interfaces;

namespace FusionCore.Seguranca.LicenciamentoOnline.Config
{
    public class FirebaseConfigDev : IEndpointCfg
    {
        private readonly IFirebaseConfig _config;

        public FirebaseConfigDev()
        {
            // TODO 1612 - FIREBASE: Database firebase dev

            _config = new FirebaseConfig
            {
                BasePath = "https://<database>.firebaseio.com",
                AuthSecret = ""
            };
        }

        public IFirebaseConfig GetConfig()
        {
            return _config;
        }
    }
}