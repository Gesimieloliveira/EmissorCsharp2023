using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class PerfilCteListBoxDTO
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public string RazaoSocialEmpresa { get; set; }
        public string CnpjEmpresa { get; set; }
        public string PerfilCfopCodigo { get; set; }
        public TipoCte TipoCte { get; set; }
        public TipoServico TipoServico { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
    }
}