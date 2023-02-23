using System;
using System.Collections.Generic;
using FireSharp;
using FusionCore.FusionAdm.Csrt;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Seguranca.LicenciamentoOnline;
using FusionCore.Seguranca.LicenciamentoOnline.Config;
using Newtonsoft.Json;

namespace FusionCore.FusionAdm.ConfiguracoesDfe
{
    // TODO 1612 - FIREBASE: Ler configurações dfe para configurar local

    public class RepositorioFirebaseConfiguracaoDfe
    {
        private readonly IEndpointCfg _cfg;

        public RepositorioFirebaseConfiguracaoDfe()
        {
            _cfg = FirebaseFactory.CriaCfg();
        }

        public List<ConfiguracaoDfe> Ler()
        {
            try
            {
                using (var firebaseCliente = new FirebaseClient(_cfg.GetConfig()))
                {
                    const string path = "configuracaoDfe";
                    var listaConfiguracaoDfe = new List<ConfiguracaoDfe>();


                    var resposta = firebaseCliente.Get(path);

                    if (resposta == null || resposta.Body == "null" || resposta.Body == null)
                        return listaConfiguracaoDfe;

                    foreach (var configuracaoDfeFirebaseDTO in 
                        JsonConvert.DeserializeObject<Dictionary<string, ConfiguracaoDfeFirebaseDTO>>(resposta.Body))
                    {

                        EstadoDTO estadoUf;
                        using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                        {
                            estadoUf = new RepositorioEstado(sessao).GetPelaSigla(configuracaoDfeFirebaseDTO.Value
                                .SiglaUf);
                        }

                        listaConfiguracaoDfe.Add(new ConfiguracaoDfe(configuracaoDfeFirebaseDTO, TipoAmbiente.Homologacao, estadoUf));
                        listaConfiguracaoDfe.Add(new ConfiguracaoDfe(configuracaoDfeFirebaseDTO, TipoAmbiente.Producao, estadoUf));
                    }

                    return listaConfiguracaoDfe;
                }
            }
            catch (Exception e)
            {
                throw CsrtException.Lancar(e.Message, e);
            }
        }
    }
}