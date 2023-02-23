using FusionCore.CadastroEmpresa;
using FusionCore.Comum;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using NHibernate;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class EmpresaComboBoxDTO : Comparavel<int>, IEmpresa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string RazaoSocial => Nome;
        protected override int ChaveUnica => Id;

        public EmpresaDTO CarregaEmpresa(ISessaoManager sessaoManager)
        {
            using (var sessao = sessaoManager.CriaSessao())
            {
                return CarregaEmpresa(sessao);
            }
        }

        public EmpresaDTO CarregaEmpresa(ISession sessao)
        {
            return new RepositorioEmpresa(sessao).GetPeloId(Id);
        }

        public override string ToString()
        {
            return $"{Nome}";
        }
    }
}