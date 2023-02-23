using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fusion.Parcelamento;
using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Parcelamento;
using MahApps.Metro.SimpleChildWindow;
using NHibernate.Util;

namespace Fusion.Visao.MdfeEletronico.IncluirPagamento
{
    public partial class IncluirPagamentoForm
    {
        private ParcelamentoDialog _childAtual;
        private readonly IncluirPagamentoFormModel _model;

        public IncluirPagamentoForm(IncluirPagamentoFormModel model)
        {
            _model = model;
            _model.Fechar += delegate { Close();};
            InitializeComponent();
            DataContext = model;
        }

        private async void GerarParcelas_OnClick(object sender, RoutedEventArgs e)
        {
            var factory = new ParcelamentoFactory(new SessaoManagerAdm());
            var dialog = factory.CriaDialog(_model.ValorTotalContrato);

            dialog.Contexto.ParceladoComSucesso += (send, args) =>
            {
                if (_model.EUmaEdicao)
                {
                    using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                    using (var transacao = sessao.BeginTransaction())
                    {
                        var repositorio = new RepositorioMdfe(sessao);

                        _model.ParcelasPagamento.ForEach(x =>
                        {
                            repositorio.Deletar(x.MdfeParcela);
                        });

                        _model.ParcelasPagamento.Clear();

                        var informacaoPagamento = _model.ObterInformacaoPagamentoEditavel();

                        informacaoPagamento.Parcelas.Clear();

                        var listaParcelas = args.Parcelas.Select(x => new MdfeParcela
                        {
                            InformacaoPagamento = informacaoPagamento,
                            Numero = x.Numero,
                            Valor = x.Valor,
                            DataDeVencimento = x.Vencimento
                        }).ToList();


                        listaParcelas.ForEach(repositorio.Salvar);

                        _model.ParcelasPagamento = new ObservableCollection<ParcelaDTO>(listaParcelas.Select(x => new ParcelaDTO
                        {
                            Valor = x.Valor,
                            VencimentoEm = x.DataDeVencimento,
                            Numero = x.Numero,
                            MdfeParcela = x
                        }).ToList());

                        transacao.Commit();
                    }

                    return;
                }

                _model.ParcelasPagamento = new ObservableCollection<ParcelaDTO>(args.Parcelas.Select(x => new ParcelaDTO
                {
                    Valor = x.Valor,
                    VencimentoEm = x.Vencimento,
                    Numero = x.Numero
                }).ToList());
            };

            _childAtual = dialog;

            await this.ShowChildWindowAsync(_childAtual, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void AdicionarComponente_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.AdicionarComponente();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void DeletarComponente(object sender, RoutedEventArgs e)
        {
            _model.DeletarComponente();
        }

        private void IncluirPagamento_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.IncluirPagamento();
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraAviso(exception.Message);
            }
        }

        private void IncluirPagamentoForm_OnContentRendered(object sender, EventArgs e)
        {
            _model.Inicializar();
        }
    }
}
