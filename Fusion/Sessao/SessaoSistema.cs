using Fusion.DI.Providers;
using Fusion.Parcelamento;
using FusionCore.Core.Estoque;
using FusionCore.DI;
using FusionCore.Preferencias;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Seguranca.Licenciamento.Dominio;
using FusionCore.Sessao;
using FusionWPF.Parcelamento;

namespace Fusion.Sessao
{
    public class SessaoSistema
    {
        private static SessaoSistema _instancia;
        private UsuarioDTO _usuarioLogado;

        private SessaoSistema()
        {
            //singleton
            SessaoManager = new SessaoManagerAdm();
            ParcelamentoFactory = new ParcelamentoFactory(SessaoManager);
            CaixaProvider = new ControleCaixaProvider();
            Preferencias = new PreferenciaSistemaService(SessaoManager);
        }

        public static SessaoSistema Instancia => _instancia ?? (_instancia = new SessaoSistema());

        public UsuarioDTO UsuarioLogado
        {
            get => _usuarioLogado;
            set
            {
                _usuarioLogado = value;
                SessaoEstoque.UsuarioEvento = value;
            }
        }

        public ISessaoManager SessaoManager { get; }
        public AcessoConcedido AcessoConcedido { get; set; }
        public bool IsAdmin => UsuarioLogado?.IsAdmin == true;
        public IControleCaixaProvider CaixaProvider { get; }
        public IParcelamentoFactory ParcelamentoFactory { get; }
        public PreferenciaSistemaService Preferencias { get; }

        public static UsuarioDTO ObterUsuarioLogado()
        {
            return Instancia.UsuarioLogado;
        }
    }
}