using System;
using System.Reflection;
using log4net;

namespace FusionCore.FusionAdm.Fiscal.NF.Cancelar
{
    public class EventoCancelamento : EventArgs
    {
        public string Justificativa { get; }
        public IDocumentoCancelavel Documento { get; }
        public RetornoCancelamento Resposta { get; }
        public StatusCancelamento Status { get; private set; }
        public string NumeroProtocolo { get; private set; }
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EventoCancelamento(string justificativa, IDocumentoCancelavel documento, RetornoCancelamento resposta)
        {
            Justificativa = justificativa;
            Documento = documento;
            Resposta = resposta;
            _log.Info("Construtor EventoCancelamento");

            ComputaInformacoes();
        }

        private void ComputaInformacoes()
        {
            _log.Info("ComputaInfomracoes");
            _log.Info("Resposta.Retorno");
            var retonro = Resposta;

            Status = new StatusCancelamento(retonro.CStat, retonro.XMotivo);

            NumeroProtocolo = retonro.Protocolo;
            _log.Info("Numero Protocolo : " + NumeroProtocolo);
        }
    }
}