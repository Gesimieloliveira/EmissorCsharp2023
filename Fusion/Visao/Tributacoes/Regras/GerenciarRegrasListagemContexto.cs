using System.Collections.Generic;
using Fusion.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Regras;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Tributacoes.Regras
{
    public class GerenciarRegrasListagemContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private bool _isGerenciarRegraSaida;

        public GerenciarRegrasListagemContexto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public IEnumerable<RegraTributacaoSaidaSlim> Regras
        {
            get => GetValue<IEnumerable<RegraTributacaoSaidaSlim>>();
            set => SetValue(value);
        }

        public RegraTributacaoSaidaSlim RegraSelecionada
        {
            get => GetValue<RegraTributacaoSaidaSlim>();
            set => SetValue(value);
        }

        public bool IsGerenciarRegraSaida
        {
            get => _isGerenciarRegraSaida;
            set
            {
                if (value == _isGerenciarRegraSaida) return;
                _isGerenciarRegraSaida = value;
                PropriedadeAlterada();
            }
        }

        public void Inicializar()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                Regras = new RepositorioRegraTributacao(sessao).ListaRegras();
            }

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
            IsGerenciarRegraSaida = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_REGRA_SAIDA);
        }
    }
}