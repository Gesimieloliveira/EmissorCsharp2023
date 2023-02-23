using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Cancelar;
using FusionCore.Repositorio.Base;

// ReSharper disable ConvertToAutoProperty
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Local

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class CancelamentoNfe : Entidade
    {
        private int _id;

        private CancelamentoNfe()
        {
            //nhbiernte
        }

        public CancelamentoNfe(
            Nfeletronica nfe,
            TipoAmbiente ambiente,
            int statusReposta,
            string textoResposta,
            StatusCancelamento statusCancelamento,
            string justificativa,
            string xmlEnvio,
            string xmlRetorno,
            DateTime ocorreuEm
        )
        {
            if (nfe.Cancelamento != null)
            {
                _id = Nfe.Id;
            }

            Nfe = nfe;
            Ambiente = ambiente;
            StatusResposta = statusReposta;
            TextoResposta = textoResposta;
            Status = statusCancelamento;
            Justificativa = justificativa;
            XmlEnvio = xmlEnvio;
            XmlRetorno = xmlRetorno;
            OcorreuEm = ocorreuEm;
        }

        protected override int ReferenciaUnica => _id;
        public Nfeletronica Nfe { get; private set; }
        public TipoAmbiente Ambiente { get; private set; }
        public int StatusResposta { get; private set; }
        public string TextoResposta { get; private set; }
        public StatusCancelamento Status { get; private set; }
        public string Justificativa { get; private set; }
        public string XmlEnvio { get; private set; }
        public string XmlRetorno { get; private set; }
        public DateTime OcorreuEm { get; private set; }
    }
}