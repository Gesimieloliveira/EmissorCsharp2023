using FusionCore.Helpers.DocumentoXml;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class NfceHistoricoUltimaChave
    {
        public int Id { get; set; }
        public string Chave { get; set; }
        public bool Finalizado { get; set; }
        public bool FalharReceberLote { get; set; }
        public string XmlLote { get; set; }

        public string ObterRecibo()
        {
            if (XmlLote == null) return null;

            var xmlHelper = new XmlHelper(XmlLote);
            return xmlHelper.GetValueFromElement("nRec", "infRec").GetValueOrEmpty();
        }
    }
}