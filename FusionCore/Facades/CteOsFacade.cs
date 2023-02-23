using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;

namespace FusionCore.Facades
{
    public class CteOsFacade
    {
        private static ISessaoManager SessaoManager { get; } = new SessaoManagerAdm();

        public static void Salvar(CteOsPercurso percurso)
        {
            using (var repositorio = CriaRepositorio())
            {
                repositorio.Salvar(percurso);
                repositorio.Commit();
            }
        }

        private static RepositorioCteOs CriaRepositorio()
        {
            var sessao = SessaoManager.CriaSessao();

            var repositorio = new RepositorioCteOs(sessao);
            repositorio.BeginTransaction();

            return repositorio;
        }

        public static void Deletar(CteOsPercurso percurso)
        {
            using (var repositorio = CriaRepositorio())
            {
                repositorio.Deletar(percurso);
                repositorio.Commit();
            }
        }

        public static void Deletar(CteOsComponenteValorPrestacao componente)
        {
            using (var repositorio = CriaRepositorio())
            {
                repositorio.Deletar(componente);
                repositorio.Commit();
            }
        }

        public static void Deletar(CteOsDocumentoReferenciado documentoReferenciado)
        {
            using (var repositorio = CriaRepositorio())
            {
                repositorio.Deletar(documentoReferenciado);
                repositorio.Commit();
            }
        }

        public static void Salvar(CteOsComponenteValorPrestacao componente)
        {
            using (var repositorio = CriaRepositorio())
            {
                repositorio.Salvar(componente);
                repositorio.Commit();
            }
        }

        public static void Salvar(CteOsDocumentoReferenciado documentoReferenciado)
        {
            using (var repositorio = CriaRepositorio())
            {
                repositorio.Salvar(documentoReferenciado);
                repositorio.Commit();
            }
        }
    }
}