using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.MdfeEletronico.Aba.Entidades
{
    public class GridMunicipioCarregamento
    {
        public MDFeMunicipioCarregamento MunicipioCarregamento { get; set; }
        public CidadeDTO Cidade { get; set; } 
    }
}