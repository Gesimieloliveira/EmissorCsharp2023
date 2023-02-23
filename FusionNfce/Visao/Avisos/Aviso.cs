using System.Windows.Input;
using FontAwesome.WPF;

namespace FusionNfce.Visao.Avisos
{
    public class Aviso
    {
        public string Mensagem { get; set; }
        public ICommand Action { get; set; }
        public FontAwesomeIcon Icone { get; set; } = FontAwesomeIcon.Check;
        public bool BotaoAcaoAtivo => Action != null;
    }
}