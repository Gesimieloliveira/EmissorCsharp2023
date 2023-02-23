namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsInformacaoCorrecao
    {
        public int Id { get; set; }
        public CteOsCartaCorrecao CteOsCartaCorrecao { get; set; }
        public string Grupo { get; set; }
        public string Campo { get; set; }
        public string NovoValor { get; set; }
        public string NumeroItem { get; set; }
    }
}