using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.AssemblyUtils;
using FusionCore.Helpers.AssemblyUtils.Leitura;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using Newtonsoft.Json;

namespace FusionCore.Helpers.EmpresaDesenvolvedora
{
    public class ResponsavelJson
    {
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoEstadual { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string NomeAplicacaoPdv { get; set; }
        public string NomeAplicacaoNfce { get; set; }

        public EstadoJson Estado { get; set; }
    }

    public class EstadoJson
    {
        public byte CodigoIbge { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
    }

    public static class ResponsavelLegal
    {
        static ResponsavelLegal()
        {
            var localAplicacao = ManipulaPasta.LocalSistema();

            var manipulaArquivo = new ManipulaArquivo($"{localAplicacao}\\Assets\\ResponsavelLegal\\responsaveltecnico.json");

            var manipula = manipulaArquivo.AbreArquivo().LerDados();

            manipulaArquivo.FecharArquivo();

            var responsavelJson = JsonConvert.DeserializeObject<ResponsavelJson>(manipula);

            NomeAplicacaoPdv = responsavelJson.NomeAplicacaoPdv;
            RazaoSocial = responsavelJson.RazaoSocial;
            Cnpj = responsavelJson.Cnpj;
            InscricaoEstadual = responsavelJson.InscricaoEstadual;
            Email = responsavelJson.Email;
            Telefone = responsavelJson.Telefone;
            NomeAplicacaoNfce = responsavelJson.NomeAplicacaoNfce;
            EstadoUf = new EstadoDTO
            {
                CodigoIbge = responsavelJson.Estado.CodigoIbge,
                Nome = responsavelJson.Estado.Nome,
                Sigla = responsavelJson.Estado.Sigla
            };
        }


        public static string NomeAplicacaoPdv { get; }
        public static string RazaoSocial { get; }
        public static string Cnpj { get; }
        public static string InscricaoEstadual { get; }

        public static EstadoDTO EstadoUf { get; }

        public static string VersaoAplicacaoPdv => AssemblyHelper.LerDoAssemblyPrincipal(new Versao3Digito());
        public static string InscricaoMunicipal => "";
        public static string VersaoSistema => AssemblyHelper.LerDoAssemblyPrincipal(new Versao3Digito());
        public static string Email { get; }
        public static string Telefone { get; }
        public static string NomeAplicacaoNfce { get; }
    }
}