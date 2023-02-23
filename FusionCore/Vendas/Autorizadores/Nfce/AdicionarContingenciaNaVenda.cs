using FusionCore.FusionAdm.Sessao;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Infraestrutura;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Vendas.Repositorio;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class AdicionarContingenciaNaVenda
    {
        private FaturamentoVenda _venda;

        public AdicionarContingenciaNaVenda(FaturamentoVenda venda)
        {
            _venda = venda;
        }

        public void Adicionar()
        {
            FaturamentoVenda venda;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var todasContingencias = new TodasContingencias(sessao);

                if (todasContingencias.ExisteContingenciaEmAberto() == false) return;

                var todosCupomFiscais = new RepositorioCupomFiscal(sessao);
                var cupomFiscal = todosCupomFiscais.ObterCupomFiscal(_venda);
                var contingenciaAberta = todasContingencias.BuscarContingenciaAberta();

                cupomFiscal.AtivarContingencia(contingenciaAberta.Id);

                venda = cupomFiscal.Venda;
                venda.SituacaoFiscalAutorizadoSemInternet();

                new RepositorioFaturamento(sessao).Salvar(venda);
                todosCupomFiscais.SalvarOuAlterar(cupomFiscal);

                transacao.Commit();
            }

            _venda = venda;
        }
    }
}