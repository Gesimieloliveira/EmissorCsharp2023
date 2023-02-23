using FusionCore.FusionNfce.Empresa;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtEmpresaAdm
    {
        public static EmpresaDTO ToAdm(this EmpresaNfce empresa)
        {
            return new EmpresaDTO
            {
                Id = empresa.Id
            };
        }
    }
}