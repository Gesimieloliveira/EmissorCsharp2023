using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeEmitente
    {
        public int MDFeId { get; set; }
        public MDFeEletronico MDFeEletronico { get; set; }
        public EmpresaDTO Empresa { get; set; }
    }
}