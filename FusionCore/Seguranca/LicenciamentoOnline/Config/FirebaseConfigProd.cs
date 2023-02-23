using FireSharp.Config;
using FireSharp.Interfaces;

namespace FusionCore.Seguranca.LicenciamentoOnline.Config
{
    public class FirebaseConfigProd : IEndpointCfg
    {
        private readonly FirebaseConfig _config;

        public FirebaseConfigProd()
        {
            // TODO 1612 - FIREBASE: Database firebase producao

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