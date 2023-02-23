using FusionCore.Extencoes;

namespace Fusion.FastReport.Facades
{
    public class ServicoObterXml : IServicoObterXml
    {
        private readonly IObterXml _obterXml;

        public ServicoObterXml(IObterXml obterXml)
        {
            _obterXml = obterXml;
        }

        public string ObterXml(int nfceId)
        {
            var xmlAutorizacao = _obterXml.ObterXmlAutorizado(nfceId);

            return xmlAutorizacao.IsNotNullOrEmpty() ? 
                xmlAutorizacao : _obterXml.UltimoXmlAssinado(nfceId);
        }
    }
}