using System.Text;

namespace FusionCore.Setup
{
    public class ConexaoCfg : IConexaoCfg
    {
        public ConexaoCfg()
        {
            Porta = 1433;
        }

        public string Servidor { get; set; }
        public string Instancia { get; set; }
        public int Porta { get; set; }
        public string BancoDados { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }

        public string CriarStringDeConexao(short timeout = 30)
        {
            var sb = new StringBuilder();

            sb.Append($"Data Source={this.GetDataSource()};");
            sb.Append($"Initial Catalog={BancoDados};");
            sb.Append("Persist Security Info=True;");
            sb.Append($"User ID={Usuario};");
            sb.Append($"Password={Senha};");
            sb.Append($"Timeout={timeout};");
            sb.Append("Pooling=true;");
            sb.Append("Min Pool Size=10;");
            sb.Append("Max Pool Size=100;");
            sb.Append("Language=English;");

            return sb.ToString();
        }
    }
}