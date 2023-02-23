using FusionCore.Setup;

namespace FusionCore.Relatorios
{
    public class CriaIConexaoCfg
    {
        public static IConexaoCfg CriaIConexaoCfgRelatorio(IConexaoCfg cfg)
        {
            return new ConexaoCfg
            {
                Servidor = cfg.Servidor,
                Instancia = cfg.Instancia,
                Porta = cfg.Porta,
                BancoDados = "FusionRelatorio",
                Senha = cfg.Senha,
                Usuario = cfg.Usuario
            };
        }
    }
}