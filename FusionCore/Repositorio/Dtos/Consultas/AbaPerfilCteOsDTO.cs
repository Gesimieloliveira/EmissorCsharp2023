using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using TipoServico = FusionCore.FusionAdm.CteEletronicoOs.Flags.TipoServico;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class AbaPerfilCteOsDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string RazaoSocial { get; set; }
        public string CnpjEmpresa { get; set; }
        public string PerfilCfopCodigo { get; set; }
        public TipoCte TipoCte { get; set; }
        public TipoServico TipoServico { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
    }
}