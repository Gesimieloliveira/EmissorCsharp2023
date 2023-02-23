using Fusion.Sessao;
using FusionCore.Seguranca.Licenciamento.Dominio;
using FusionCore.Sessao;
using FusionWPF.Parcelamento;

namespace Fusion.Parcelamento
{
    public class ParcelamentoFactory : IParcelamentoFactory
    {
        private readonly ISessaoManager _sessaoManager;

        public ParcelamentoFactory(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public ParcelamentoDialog CriaDialog(decimal valor)
        {
            return new ParcelamentoDialog(this, valor);
        }

        public IRepositorioParcelamento CriarRepositorio()
        {
            return new RepositorioParcelamento(_sessaoManager.CriaSessao());
        }

        public AcessoConcedido GetAcessoConcedido()
        {
            return SessaoSistema.Instancia.AcessoConcedido;
        }
    }
}