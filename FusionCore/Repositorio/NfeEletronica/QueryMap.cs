using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.Repositorio.NfeEletronica
{
    public static class QueryMap
    {
        public static Nfeletronica TbNfe { get; } = null;
        public static DestinatarioNfe TbDestinatario { get; } = null;
        public static EmpresaDTO TbEmpresa { get; } = null;
        public static EmissaoFinalizadaNfe TbFinalizacao { get; } = null;
        public static ItemNfe TbItem { get; } = null;
    }
}