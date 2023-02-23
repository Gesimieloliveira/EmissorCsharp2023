using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using Fusion.Sessao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Vendas.Autorizadores;
using FusionCore.Vendas.Autorizadores.Nfce;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Vendas.Repositorio;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas
{
    public class AutorizadorNfceTela : IAutorizadorTela
    {
        private readonly LayoutImpressao _layoutImpressao;
        private readonly string _nomeImpressora;
        private readonly bool _duasVias;
        private readonly bool _preVisualizar;
        private readonly bool _imprimirFinalizacao;
        private Action _acao;

        public AutorizadorNfceTela(LayoutImpressao layoutImpressao,
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

        public AutorizadorNfceTela() : this(LayoutImpressao.ImpressaoA4, string.Empty, false, false, false)
        {

        }

        public async void EnviaSefaz(FaturamentoVenda venda, Action acao)
        {
            _acao = acao;
            ProgressBarAgil4.ShowProgressBar();

            await new TaskFactory()
                .StartNew(async () =>
                {
                    try
                    {
                        var usuarioLogado = SessaoSistema.ObterUsuarioLogado();

                        venda = InicializaVendaCompleto(venda);

                        var temHistoricoEmAberto = VerificaSeTemHistoricoEmAberto.ComRejeicao(venda);

                        if (temHistoricoEmAberto)
                        {
                            new VerificaHistoricoPendente(venda).Verificar();
                        }


                        if (temHistoricoEmAberto == false)
                        {
                            IEnvioSefaz envioSefaz = new EnvioSefazNfce(venda, usuarioLogado);

                            envioSefaz.CriaCupomFiscal()
                                .AlocarNumeracaoFiscal()
                                .CriaHistorico()
                                .Autorizar();
                        }

                        await Application.Current.Dispatcher.BeginInvoke(new Action(ProgressBarAgil4.CloseProgressBar));

                        await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Imprimir(venda);
                        }));
                    }
                    catch (WebException webErro)
                    {
                        await Application.Current.Dispatcher.BeginInvoke(new Action(ProgressBarAgil4.CloseProgressBar));
                        await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            DialogBox.MostraErro(webErro.Message, webErro, BoxType.Warning);
                            Imprimir(venda);
                        }));
                    }
                    catch (InvalidOperationException ex)
                    {
                        await Application.Current.Dispatcher.BeginInvoke(new Action(ProgressBarAgil4.CloseProgressBar));
                        await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            DialogBox.MostraAviso(ex.Message);
                        }));
                    }
                    catch (Exception erro)
                    {
                        await Application.Current.Dispatcher.BeginInvoke(new Action(ProgressBarAgil4.CloseProgressBar));
                        await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            DialogBox.MostraErro("Não consegui emitir: " + erro.Message, erro);
                        }));
                    }
                    finally
                    {
                        await Application.Current.Dispatcher.BeginInvoke(new Action(ProgressBarAgil4.CloseProgressBar));
                    }
                });
        }

        private void Imprimir(FaturamentoVenda venda)
        {
            var imprimir =
                new ImprimirCupomFiscalNfce(_layoutImpressao,
                    _nomeImpressora,
                    _duasVias,
                    _preVisualizar,
                    _imprimirFinalizacao);
            imprimir.Imprime(venda);

            _acao?.Invoke();
        }

        private FaturamentoVenda InicializaVendaCompleto(FaturamentoVenda venda)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioFaturamento(sessao).GetPeloIdCompleto(venda.Id);
            }
        }
    }
}