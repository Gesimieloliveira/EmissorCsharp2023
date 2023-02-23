using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Empresa;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Setup.Empresa
{
    public class EmpresaSetupHelper
    {
        public bool EmpresaCadastrada { get; set; }

        public void ConsultarSituacaoEmpresa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<EmpresaDTO>(sessao);

                var empresas = repositorio.Busca(new TodasEmpresas());

                EmpresaCadastrada = empresas.Count > 0;
            }
        }
    }
}