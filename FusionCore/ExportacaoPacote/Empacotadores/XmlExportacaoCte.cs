using System.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable ClassNeverInstantiated.Global

namespace FusionCore.ExportacaoPacote.Empacotadores
{
    public class XmlExportacaoCte : IEnvelope
    {
        private static int[] _codigosCancelamento;

        public XmlExportacaoCte()
        {
            if (_codigosCancelamento == null)
            {
                _codigosCancelamento = new[] { 134, 135, 136 };
            }
        }

        public string Chave { get; private set; }
        public string Grupo => _codigosCancelamento.Any(i => i == StatusCancelamento) ? "Cancelados" : "Autorizados";
        public string Nome => $"{Chave}.xml";
        public string Conteudo { get; set; }
        public short StatusCancelamento { get; private set; }
    }
}