using System;
using System.Net;
using RestSharp;

namespace FusionCore.Helpers.ConsultaCnpj
{
    public sealed class ReceitaWs
    {
        private readonly RestClient _http;

        public ReceitaWs()
        {
            _http = new RestClient("https://www.receitaws.com.br");
        }

        public event EventHandler<Exception> RequestError;
        public event EventHandler<EmpresaReceitaWs> RequestSuccess;

        public void FazRequest(string cnpj)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = new RestRequest("v1/cnpj/{cnpj}", Method.GET);

                request.AddUrlSegment("cnpj", cnpj);
                request.AddHeader("Content-Type", "application/json");

                var response = _http.Execute<DataReceitaWs>(request);

                if (response.StatusCode == HttpStatusCode.GatewayTimeout)
                {
                    throw new InvalidOperationException(
                        "O tempo máximo para a resposta foi excedido. Talvez uma nova tentativa ocorra bem!");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new InvalidOperationException(
                        $"Falha ao completar a requisição: {response.StatusDescription} - {response.Content}");
                }

                if (response.Data.Status == "ERROR")
                {
                    throw new InvalidOperationException(response.Data.Message);
                }

                OnRequestSuccess(EmpresaReceitaWs.Cria(response.Data));
            }
            catch (Exception e)
            {
                OnRequestError(e);
            }
        }

        private void OnRequestError(Exception e)
        {
            RequestError?.Invoke(this, e);
        }

        private void OnRequestSuccess(EmpresaReceitaWs e)
        {
            RequestSuccess?.Invoke(this, e);
        }
    }
}