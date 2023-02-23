using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeMunicipioCarregamento
    {
        public int Id { get; set; }
        public MDFeEletronico MDFeEletronico { get; set; }
        public CidadeDTO Cidade { get; set; } 
    }
}