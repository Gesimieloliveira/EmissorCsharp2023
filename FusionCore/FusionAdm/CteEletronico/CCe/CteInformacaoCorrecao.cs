namespace FusionCore.FusionAdm.CteEletronico.CCe
{
    public class CteInformacaoCorrecao
    {
        public int Id { get; set; }
        public CteCartaCorrecao CteCartaCorrecao { get; set; }
        public string Grupo { get; set; }
        public string Campo { get; set; }
        public string NovoValor { get; set; }
        public string NumeroItem { get; set; }
    }
}