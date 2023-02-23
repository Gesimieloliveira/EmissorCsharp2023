using FusionCore.ControleCaixa.Servicos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.ControleCaixa.Facades
{
    public static class ControleCaixaGestorFacade
    {
        public static void ThrowExcetpionSeNaoExistirCaixaAberto(UsuarioDTO usuario)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var servico = new ServicoControleCaixaAtivo(sessao, ELocalEventoCaixa.Gestao);

                servico.ThrowExceptionSePrecisarDeCaixaAberto(usuario);
            }
        }
    }
}