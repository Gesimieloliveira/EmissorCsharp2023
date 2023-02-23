using System;
using Fusion.FastReport.Relatorios.Sistema.FaturamentoMei;
using Fusion.FastReport.Relatorios.Sistema.Financeiro;
using Fusion.Visao.ControlarNfces;
using FusionCore.Sessao;
using FusionCore.Vendas.Faturamentos;

namespace Fusion.Facades
{
    public class ImpressorFaturamento
    {
        private readonly ISessaoManager _sessaoManager;

        public ImpressorFaturamento(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public void Imprime(IFaturuamentoImprimivel imprimivel, FaturamentoPreferencia faturamentoPreferencia)
        {
            if (faturamentoPreferencia == null)
            {
                throw new PreferenciaException("Configure a preferencias do faturamento");
            }

            if (imprimivel.EstadoAtual != Estado.Finalizado)
            {
                throw new InvalidOperationException("Faturamento precisa estar finalizado para ser impresso");
            }

            using (var report = CriarRelatorio(imprimivel, faturamentoPreferencia))
            {
                report.Imprimir(faturamentoPreferencia.Impressora);
            }
        }

        private RImpressaoFaturamento CriarRelatorio(IFaturuamentoImprimivel imprimivel, FaturamentoPreferencia faturamentoPreferencia)
        {
            RImpressaoFaturamento relatorio;

            if (faturamentoPreferencia.LayoutImpressao == LayoutImpressao.Impressao80M)
            {
                relatorio = new RImpressaoFaturamento80(_sessaoManager);
            }
            else
            {
                relatorio = new RImpressaoFaturamentoA4(_sessaoManager);
            }

            relatorio.ComId(imprimivel.Id);
            relatorio.DuplicarImpressao(faturamentoPreferencia.ImprimeDuasVias);

            return relatorio;
        }

        public void Viazualiza(IFaturuamentoImprimivel imprimivel, FaturamentoPreferencia faturamentoPreferencia)
        {
            if (faturamentoPreferencia == null)
            {
                throw new PreferenciaException("Configure a preferencias do faturamento");
            }

            if (imprimivel.EstadoAtual != Estado.Finalizado)
            {
                throw new InvalidOperationException("Faturamento precisa estar finalizado para ser impresso");
            }

            using (var report = CriarRelatorio(imprimivel, faturamentoPreferencia))
            {
                report.Visualizar();
            }
        }

        public void VisualizaPromissoriaCarne(FaturamentoVenda faturamentoVenda, FaturamentoPreferencia preferencia)
        {
            if (preferencia == null)
            {
                throw new PreferenciaException("Configure a preferencias do faturamento");
            }

            if (faturamentoVenda.ContemMalote() == false) return;

            switch (preferencia.LayoutImpressaoPromissoria)
            {
                case LayoutImpressaoPormissoria.NaoImprimir:
                    return;
                case LayoutImpressaoPormissoria.ImpressaoPromissoria:
                    var promissoria = new RPromissoria(new SessaoManagerAdm());
                    promissoria.ComMaloteId(faturamentoVenda.Malote.Id);
                    promissoria.Visualizar();
                    break;
                case LayoutImpressaoPormissoria.ImpressaoPromissoriaCarne:
                    var promissoriaCarne = new RPromissoriaCarne(new SessaoManagerAdm());
                    promissoriaCarne.ComMaloteId(faturamentoVenda.Malote.Id);
                    promissoriaCarne.Visualizar();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}