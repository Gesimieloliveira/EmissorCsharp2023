using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Cfop
{
    public class CfopNfce
    {
        public string Id { get; set; }
        public string Descricao { get; set; }
        public bool ElegivelNfce { get; set; }
        public string DescricaoFormatada => $"{Id} - {Descricao}";

        private CfopNfce()
        {
            //nhibernate
        }

        public CfopNfce(string id, string descricao, bool elegivelNfce) : this()
        {
            Id = id;
            Descricao = descricao;
            ElegivelNfce = elegivelNfce;
        }

        public static CfopNfce From(CfopDTO cfop)
        {
            return new CfopNfce
            {
                Id = cfop.Id,
                Descricao = cfop.Descricao,
                ElegivelNfce = cfop.ElegivelNfce
            };
        }
    }
}