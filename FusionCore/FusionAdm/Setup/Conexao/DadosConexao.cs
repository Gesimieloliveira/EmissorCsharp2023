using FusionCore.Setup;

namespace FusionCore.FusionAdm.Setup.Conexao
{
    public class DadosConexao
    {
        public DadosConexao()
        {
            Porta = 1433;
        }

        public string Endpoint => $"{Servidor}\\{Instancia}";

        public string Servidor { get; set; }
        public string Instancia { get; set; }
        public int Porta { get; set; }
        public string BancoDados { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }

        public IConexaoCfg ToCfg()
        {
            return new ConexaoCfg
            {
                Servidor = Servidor,
                Instancia = Instancia,
                Porta = Porta,
                BancoDados = BancoDados,
                Senha = Senha,
                Usuario = Usuario
            };
        }
    }
}