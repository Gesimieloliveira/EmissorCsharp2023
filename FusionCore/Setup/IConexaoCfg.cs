namespace FusionCore.Setup
{
    public interface IConexaoCfg
    {
        string Servidor { get; set; }
        string Instancia { get; set; }
        int Porta { get; set; }
        string BancoDados { get; set; }
        string Usuario { get; set; }
        string Senha { get; set; }
        string CriarStringDeConexao(short timeout = 30);
    }
}