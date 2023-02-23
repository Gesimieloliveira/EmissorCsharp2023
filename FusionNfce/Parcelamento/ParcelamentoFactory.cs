using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Seguranca.Licenciamento.Dominio;
using FusionWPF.Parcelamento;

namespace FusionNfce.Parcelamento
{
    public class ParcelamentoFactory : IParcelamentoFactory
    {
        public ParcelamentoDialog CriaDialog(decimal valor)
        {
            return new ParcelamentoDialog(this, valor);
        }

        public IRepositorioParcelamento CriarRepositorio()
        {
            var sessao = SessaoSistemaNfce.SessaoManager.CriaSessao();

            return new RepositorioParcelamento(sessao);
        }

        public AcessoConcedido GetAcessoConcedido()
        {
            return SessaoSistemaNfce.AcessoConcedido;
        }
    }
}