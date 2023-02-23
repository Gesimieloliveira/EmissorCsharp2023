using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class EmpresaPickerModelDto
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoEstadual { get; set; }
        public string NomeCidade { get; set; }
        public string SiglaUf { get; set; }

        public string GetCidadeUf()
        {
            return $"{NomeCidade} - {SiglaUf}";
        }

        public EmpresaDTO GetEmpresa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioEmpresa(sessao).GetPeloId(Id);
            }
        }
    }
}