using FusionCore.ControleCaixa.Servicos;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Usuario;

namespace FusionCore.ControleCaixa.Facades
{
    public static class ControleCaixaNfceFacade
    {
        public static void ThrowExcetpionSeNaoExistirCaixaAberto(UsuarioNfce usuario)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var servico = new ServicoControleCaixaAtivo(sessao, ELocalEventoCaixa.Terminal);

                servico.ThrowExceptionSePrecisarDeCaixaAberto(usuario);
            }
        }
    }
}