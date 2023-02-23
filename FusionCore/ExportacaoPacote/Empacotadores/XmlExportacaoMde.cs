using System;

namespace FusionCore.ExportacaoPacote.Empacotadores
{
    public class XmlExportacaoMde : IEnvelope
    {
        public string Grupo => DefinirGrupo();
        public string Nome => $"{Chave}.xml";

        public string Conteudo
        {
            get => Xml;
            set => Xml = value;
        }
        public string Chave { get; set; }
        public DateTime EmitidaEm { get; set; }
        public string Xml { get; set; }

        private string DefinirGrupo()
        {
            return $"Distribuicao/NFe/{EmitidaEm.Year:D4}-{EmitidaEm.Month:D2}";
        }
    }
}