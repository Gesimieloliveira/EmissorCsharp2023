using FusionCore.FusionAdm.MdfeEletronico;

namespace Fusion.Visao.MdfeEletronico.Aba.Entidades
{
    public class GridContratante
    {
        public string DocumentoUnico { get; set; }
        public string Nome { get; set; }
        public MDFeContratante Contratante { get; set; }
    }
}