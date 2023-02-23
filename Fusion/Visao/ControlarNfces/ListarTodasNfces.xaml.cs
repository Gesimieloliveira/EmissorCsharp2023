using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionCore.Cupom.Nfce;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Servicos;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.ControlarNfces
{
    public partial class ListarTodasNfces
    {
        private readonly ListarTodasNfcesDados _listarTodasNfcesDados;
        private readonly Action _atualizarListagens;

        public ListarTodasNfces(ListarTodasNfcesDados listarTodasNfcesDados, Action atualizarListagens)
        {
            InitializeComponent();
            _listarTodasNfcesDados = listarTodasNfcesDados;
            _atualizarListagens = atualizarListagens;
            DataContext = _listarTodasNfcesDados;
        }

        private void ManipularUmaNfceSelecionada(object sender, MouseButtonEventArgs e)
        {
            CliqueOpcaoNfce();
        }

        private void CliqueOpcoesUmaNfceSelecionada(object sender, RoutedEventArgs e)
        {
            CliqueOpcaoNfce();
        }

        private void CliqueOpcaoNfce()
        {
            var cupomSelecionada = _listarTodasNfcesDados.CupomSelecionado;

            var nfceId = cupomSelecionada.Id;

            new OpcaoNfce(new OpcaoNfceDados(nfceId, cupomSelecionada.IsFaturamento, cupomSelecionada.IsPodeAutorizar),
                () =>
                {
                    _listarTodasNfcesDados.PesquisarNfces();
                    _atualizarListagens.Invoke();
                }).ShowDialog();

            _listarTodasNfcesDados.PesquisarNfces();
            _atualizarListagens.Invoke();
        }

        private void CliqueCopiarChave(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string chave)
            {
                Clipboard.SetText(chave);
            }
        }

        private void CliqueAplicarFiltro(object sender, RoutedEventArgs e)
        {
            PesquisarNfces();
        }

        private void ListarTodasNfces_Carregado(object sender, RoutedEventArgs e)
        {
            PesquisarNfces();
        }

        private void PesquisarNfces()
        {
            _listarTodasNfcesDados.PesquisarNfces();
        }

        private void EnviarLoteNFCe_Click(object sender, RoutedEventArgs e)
        {
            var nfcesSelecionadas = _listarTodasNfcesDados.ObterNfcesSelecionadas();

            if (!nfcesSelecionadas.Any())
            {
                DialogBox.MostraAviso("Você deve selecionar um ou mais NFC-es vinda de Faturamentos");
                return;
            }

            if (nfcesSelecionadas.Any(x => x.CupomSituacao != CupomSituacao.Rejeicao &&
                                           x.CupomSituacao != CupomSituacao.RejeicaoOffline))
            {
                DialogBox.MostraAviso("Aceito somente NFC-e do tipo Rejeições");
                return;
            }

            if (nfcesSelecionadas.Any(x => x.IsFaturamento == false))
            {
                DialogBox.MostraAviso("Aceito somente NFC-e vinda de um Faturamento\n Filtrar no \"Emitidas no\" para apenas Faturamentos");
                return;
            }

            var agrupamento = nfcesSelecionadas.GroupBy(x => x.NomeEmpresa);

            if (agrupamento.Count() > 1)
            {
                DialogBox.MostraAviso("Somente posso emitir NFC-es de uma empresa por vez, nunca de duas ou mais empresas diferentes.");
                return;
            }

            foreach (var cupomFiscalSelecionado in nfcesSelecionadas)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                    if (repositorioCupomFiscal.ExisteHistoricoEmAberto(cupomFiscalSelecionado.VendaId))
                    {
                        DialogBox.MostraAviso($"NFC-e de Id={cupomFiscalSelecionado.Id} tem uma emissão pendente, finalizar a mesma para continuar \n ou remova a mesma da seleção de nfc-e para envio");
                        return;
                    }
                }
            }

            if (ContingenciaAtiva.EstaAtiva())
            {
                DialogBox.MostraAviso("Você está de contingência ativa, espere 40 minutos para tentar novamente");
                return;
            }

            var model = new ResolverNfceFaturamentosFormModel(nfcesSelecionadas);

            new ResolverNfceFaturamentosForm(model).ShowDialog();

            _listarTodasNfcesDados.PesquisarNfces();
        }

        private void AtualizarCheckSelecionadoHandler(object sender, RoutedEventArgs e)
        {
            if (_listarTodasNfcesDados.CupomSelecionado != null)
                _listarTodasNfcesDados.CupomSelecionado.IsSelecionado = true;
        }

        private void AtualizarNotCheckedSelecionadoHandler(object sender, RoutedEventArgs e)
        {
            if (_listarTodasNfcesDados.CupomSelecionado != null)
                _listarTodasNfcesDados.CupomSelecionado.IsSelecionado = false;
        }
    }
}
