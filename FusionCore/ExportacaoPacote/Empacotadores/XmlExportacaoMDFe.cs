using FusionCore.FusionAdm.MdfeEletronico.Flags;

namespace FusionCore.ExportacaoPacote.Empacotadores
{
    public class XmlExportacaoMDFe : IEnvelope
    {
        public string Xml { get; set; }
        public string Chave { get; set; }
        public MDFeStatus Status { get; set; }
        public string Nome => $"{Chave}.xml";
        public string Conteudo
        {
            get => Xml;
            set => Xml = value;
        }

        public string Grupo
        {
            get
            {
                switch (Status)
                {
                    case MDFeStatus.Cancelada:
                        return "Canceladas";
                    case MDFeStatus.Autorizado:
                        return "Autorizados";
                }

                return "Encerradas";
            }
        }
    }
}