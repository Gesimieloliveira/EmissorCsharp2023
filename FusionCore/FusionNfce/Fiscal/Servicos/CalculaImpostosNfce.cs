using System.Linq;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Repositorio.FusionNfce;

namespace FusionCore.FusionNfce.Fiscal.Servicos
{
    public class CalculaImpostosNfce
    {
        private readonly Nfce _nfce;

        public CalculaImpostosNfce(Nfce nfce)
        {
            _nfce = nfce;
        }

        public void Calcular()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfce = new RepositorioNfce(sessao);

                _nfce.CalculaImpostos(repositorioNfce);
                _nfce.TotalBaseCalculo = _nfce.ObterOsItens().Sum(x => x.ImpostoIcms.BcIcms);
                _nfce.TotalIcms = _nfce.ObterOsItens().Sum(x => x.ImpostoIcms.ValorIcms);
                _nfce.TotalBaseCalculoCofins = _nfce.ObterOsItens().Sum(x => x.ImpostoCofins.BaseCalculo);
                _nfce.TotalCofins = _nfce.ObterOsItens().Sum(x => x.ImpostoCofins.Valor);
                _nfce.TotalBaseCalculoPis = _nfce.ObterOsItens().Sum(x => x.ImpostoPis.BaseCalculo);
                _nfce.TotalPis = _nfce.ObterOsItens().Sum(x => x.ImpostoPis.Valor);

                repositorioNfce.SalvarESincronizar(_nfce);

                transacao.Commit();
            }
        }
    }
}