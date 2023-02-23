namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class ItemDistribuicaoDFe
    {
        public ItemDistribuicaoDFe()
        {
            XmlDescompactado = string.Empty;
            Processado = false;
        }

        public int Id { get; set; }
        public DistribuicaoDFe Distribuicao { get; set; }
        public string XmlDescompactado { get; set; }
        public long Nsu { get; set; }
        public string NomeSchema { get; set; }
        public TipoDfe TipoDfe { get; set; }
        public int? TipoEvento { get; set; }
        public bool Processado { get; private set; }

        public void MarcarComoProcessado()
        {
            Processado = true;
        }
    }
}