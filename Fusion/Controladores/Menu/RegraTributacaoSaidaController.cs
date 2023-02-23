using Fusion.Sessao;
using Fusion.Visao.Tributacoes.Regras;
using FusionCore.Papeis.Enums;
using FusionCore.Tributacoes.Regras;
using MahApps.Metro.Controls;

namespace Fusion.Controladores.Menu
{
    public class RegraTributacaoSaidaController : Controlador
    {
        private const string TituloAba = "Regras de saída";
        private GerenciarRegrasListagemContexto _contextoAba;
        private GerenciarRegrasListagem _contentAba;
        private readonly bool _isGerenciarRegraSaida;

        public RegraTributacaoSaidaController(MetroTabControl tabControl) : base(tabControl)
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
            _isGerenciarRegraSaida = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_REGRA_SAIDA);
        }

        public void NovaRegra()
        {
            var contexto = new RegraTributacaoSaidaContexto(SessaoManager);
            var view = new RegraTributacaoSaidaView(contexto);

            contexto.SalvoSucesso += (sender, saida) => { AtualizaListagemAberta(); };

            ShowDialog(view);
        }

        private void AtualizaListagemAberta()
        {
            if (GetTab(TituloAba) != null)
            {
                _contextoAba.Inicializar();
            }
        }

        public void GerenciarRegras()
        {
            if (GetTab(TituloAba) is MetroTabItem tab)
            {
                tab.Focus();
                return;
            }

            _contextoAba = new GerenciarRegrasListagemContexto(SessaoManager);
            _contentAba = new GerenciarRegrasListagem(this, _contextoAba);

            AbrirJanelaEmAba(TituloAba, _contentAba, _contextoAba);
        }

        public void EditaRegra(RegraTributacaoSaidaSlim slim)
        {
            if (_isGerenciarRegraSaida == false) return;

            var contexto = new RegraTributacaoSaidaContexto(SessaoManager);
            var view = new RegraTributacaoSaidaView(contexto);

            contexto.Edicao(slim);
            contexto.SalvoSucesso += (sender, saida) => { AtualizaListagemAberta(); };

            ShowDialog(view);
        }
    }
}