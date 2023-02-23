using FusionCore.FusionAdm.TabelasDePrecos.Dtos;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Venda;
using FusionCore.FusionNfce.Vendedores;
using FusionCore.Repositorio.FusionNfce;
using FusionNfce.Visao.Principal.Contratos;
using NHibernate;

namespace FusionNfce.Visao.Principal.Implementacoes
{
    public class ComandoAbrirVenda : IComandoVenda
    {
        private readonly Nfce _nfce;

        public ComandoAbrirVenda()
        {
            _nfce = new Nfce
            {
                UsuarioCriacao = SessaoSistemaNfce.Usuario,
                RegimeTributario = SessaoSistemaNfce.Empresa().RegimeTributario,
                TipoEmissao = SessaoSistemaNfce.TipoEmissao
            };
        }

        public void ExecutaAcao(VendaModel model, ItemEspera item)
        {
            if (model.StatusVenda == StatusVenda.EmUso) return;

            model.StatusVenda = StatusVenda.EmUso;

            var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            MontaNfce(model.ObterVendedor(), model.TabelaPrecoSelecionada);

            using (sessao)
            using (transacao)
            {
                SalvaNfce(sessao);
                transacao.Commit();
            }

            model.Nfce = _nfce;

            model.StatusVenda = StatusVenda.Venda;

            new ComandoVenderItem().ExecutaAcao(model, item);
        }

        private void MontaNfce(VendedorNfce vendedor, TabelaPrecoDto modelTabelaDto)
        {
            new ConstruirNfce(_nfce, vendedor).ComTabelaPreco(modelTabelaDto).Constroi();
        }

        private void SalvaNfce(ISession sessao)
        {
            var repositorioNfce = new RepositorioNfce(sessao);

            repositorioNfce.SalvarESincronizar(_nfce);
        }
    }
}