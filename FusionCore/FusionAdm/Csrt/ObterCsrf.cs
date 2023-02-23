using System;
using System.Collections.Generic;
using FireSharp;
using FusionCore.Seguranca.LicenciamentoOnline;
using FusionCore.Seguranca.LicenciamentoOnline.Config;
using Newtonsoft.Json;

namespace FusionCore.FusionAdm.Csrt
{
    public class ObterCsrf
    {
        private readonly IEndpointCfg _cfg;

        public ObterCsrf()
        {
            _cfg = FirebaseFactory.CriaCfg();
        }

        public List<CsrtDTO> LerResponsaveisTecnicos()
        {
            try
            {
                using (var firebaseClient = new FirebaseClient(_cfg.GetConfig()))
                {
                    const string path = "responsaveisTecnicos";

                    var resposta = firebaseClient.Get(path);

                    var listaCsrf = new List<CsrtDTO>();

                    if (resposta == null || resposta.Body == "null" || resposta.Body == null)
                        return listaCsrf;

                    foreach (var csrf in JsonConvert.DeserializeObject<Dictionary<string, CsrtInfo>>(resposta.Body))
                    {
                        listaCsrf.Add(new CsrtDTO
                        {
                            Guid = new Guid(csrf.Key),
                            CsrtId = csrf.Value.CsrtId,
                            Csrt = csrf.Value.Csrt,
                            SiglaUf = csrf.Value.SiglaUf,
                            IsMDFe = csrf.Value.IsMDFe,
                            IsCTe = csrf.Value.IsCTe,
                            IsNFe = csrf.Value.IsNFe,
                            IsCTeOs = csrf.Value.IsCTeOs,
                            IsNFCe = csrf.Value.IsNFCe
                        });
                    }

                    return listaCsrf;
                }
            }
            catch (Exception e)
            {
                throw CsrtException.Lancar(e.Message, e);
            }
        }
    }
}