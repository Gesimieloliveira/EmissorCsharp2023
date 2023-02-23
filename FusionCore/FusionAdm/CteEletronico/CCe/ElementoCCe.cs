namespace FusionCore.FusionAdm.CteEletronico.CCe
{
    public class ElementoCCe
    {
        public string Tag { get; set; }
        public string Descricao { get; set; }

        public ElementoCCe Pai { get; set; }

        public ElementoCCe(string tag, string descricao, ElementoCCe pai = null)
        {
            Tag = tag;
            Descricao = descricao;
            Pai = pai;
        }

        public static ElementoCCe Cria(string tag, string descricao, ElementoCCe pai = null)
        {
            return new ElementoCCe(tag, descricao, pai);
        }
    }
}