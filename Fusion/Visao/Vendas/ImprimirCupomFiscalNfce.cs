using Fusion.FastReport.Facades;
using Fusion.FastReport.Facades.Infra;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Faturamentos;
using LayoutImpressao = FusionCore.FusionNfce.Preferencias.LayoutImpressao;

namespace Fusion.Visao.Vendas
{
    public class ImprimirCupomFiscalNfce : IImprimirCupomFiscal
    {
        private readonly FusionCore.Vendas.Faturamentos.LayoutImpressao _layoutImpressao;
        private readonly string _nomeImpressora;
        private readonly bool _duasVias;
        private readonly bool _preVisualizar;
        private readonly bool _imprimirFinalizacao;

        public ImprimirCupomFiscalNfce(FusionCore.Vendas.Faturamentos.LayoutImpressao layoutImpressao,
            string nomeImpressora,
            bool duasVias,
            bool preVisualizar,
            bool imprimirFinalizacao)
        {
            _layoutImpressao = layoutImpressao;
            _nomeImpressora = nomeImpressora;
            _duasVias = duasVias;
            _preVisualizar = preVisualizar;
            _imprimirFinalizacao = imprimirFinalizacao;
        }

        public void Imprime(FaturamentoVenda venda)
        {
            var danfeNfceConfiguracaoDto = new DanfeNfceConfiguracaoDto();
            danfeNfceConfiguracaoDto.ImprimirComImpressora(_nomeImpressora);
            danfeNfceConfiguracaoDto.ForcarSegundaVia(_duasVias);
            
            if (_preVisualizar)
                danfeNfceConfiguracaoDto.PreVisualizarSomente();

            if (_imprimirFinalizacao)
                danfeNfceConfiguracaoDto.ImprimirAposFinalizacao();

            danfeNfceConfiguracaoDto.UsarPlanoDeImpressao(_layoutImpressao == FusionCore.Vendas.Faturamentos.LayoutImpressao.ImpressaoA4 ? LayoutImpressao.ImpressaoA4 : LayoutImpressao.Impressao80M);
            danfeNfceConfiguracaoDto.SemNomeFantasiaCustomizado();
            danfeNfceConfiguracaoDto.InterromperImpressao(false);

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                new DanfeNfceFacade().Imprimir(
                    danfeNfceConfiguracaoDto,
                    new RepositorioCupomFiscal(sessao).ObterCupomFiscal(venda),
                    new ServicoObterXml(new RepositorioDanfeCupomFiscalNfce(sessao))
                );
            }
        }
    }
}