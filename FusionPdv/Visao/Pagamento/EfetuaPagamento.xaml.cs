using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ACBrFramework;
using FusionCore.FusionPdv.Sessao;
using FusionPdv.Modelos.FormaPagamento;
using FusionPdv.Modelos.Pagamento;
using FusionPdv.Servicos.Tef;
using FusionPdv.Visao.Validacao;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace FusionPdv.Visao.Pagamento
{
    public partial class EfetuaPagamento
    {
        private readonly EfetuaPagamentoModel _efetuaPagamentoModel;
        public bool PagamentoConcluido { get; private set; }

        public EfetuaPagamento(EfetuaPagamentoModel model)
        {
            InitializeComponent();
            try
            {
                _efetuaPagamentoModel = model;
                _efetuaPagamentoModel.FecharTela += FecharTela;
                _efetuaPagamentoModel.DesativaTudo += DesativaTudo;
                _efetuaPagamentoModel.AtivaTudo += AtivaTudo;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }


            DataContext = _efetuaPagamentoModel;
        }

        private void FecharTela(object sender, EventArgs e)
        {
            AtivaTudo(sender, e);
            Application.Current.Dispatcher.Invoke(Close);
        }

        private void TbPagamento_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_efetuaPagamentoModel.UsandoTef) return;
            if (e.Key != Key.Enter) return;

            try
            {
                _efetuaPagamentoModel.AtualizaMensagem();
                TbPagamento.Visibility = Visibility.Collapsed;
                BtLancarPagamento.Visibility = Visibility.Visible;
                BtLancarPagamento.Focus();
            }
            catch (ValorMenorQueZeroException)
            {
                BtLancarPagamento.Visibility = Visibility.Collapsed;
                TbPagamento.Visibility = Visibility.Visible;
                _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                _efetuaPagamentoModel.MensagemValorInvalido();
                TbPagamento.SelectAll();
                TbPagamento.Focus();
            }
            catch (ExceptionValorInvalido)
            {
                BtLancarPagamento.Visibility = Visibility.Collapsed;
                TbPagamento.Visibility = Visibility.Visible;
                _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                _efetuaPagamentoModel.MensagemValorInvalido();
                TbPagamento.SelectAll();
                TbPagamento.Focus();
            }
        }

        private void BtLancarPagamento_OnClick(object sender, RoutedEventArgs e)
        {
            if (TravaTodasOpcoesAteAcabar) return;

            TravaTodasOpcoesAteAcabar = true;

            _efetuaPagamentoModel.TerminouDeLancarPagamento = false;
            var finalizado = false;
            _efetuaPagamentoModel.InicializaFormaPagamentoLocal();

            try
            {
                _efetuaPagamentoModel.ValidarPagamento();

                var valor = _efetuaPagamentoModel.PagamentoDeveSerMaiorQueZero();

                if (_efetuaPagamentoModel.AjusteDeSaldoEMenorQueTotalPago(valor))
                {
                    VoltaCamposParaValoresPadroes();
                    TravaTodasOpcoesAteAcabar = false;
                    return;
                }

                _efetuaPagamentoModel.ExecutaValidacoesDeIntegridade(valor);

                _efetuaPagamentoModel.AdicionaPagamenoASerImpressosNaEcf();

                _efetuaPagamentoModel.AtualizaInformacao();

                _efetuaPagamentoModel.AtualizarTotais();

                if (_efetuaPagamentoModel.EUmAjusteDeSaldo())
                {
                    VoltaCamposParaValoresPadroes();
                    TravaTodasOpcoesAteAcabar = false;
                    return;
                }

                if (_efetuaPagamentoModel.NaoPodeFinalizar())
                {
                    VoltaCamposParaValoresPadroes();
                    TravaTodasOpcoesAteAcabar = false;
                    return;
                }
            }
            catch (ExceptionValorInvalido)
            {
                TravaTodasOpcoesAteAcabar = false;
                BtLancarPagamento.Visibility = Visibility.Collapsed;
                TbPagamento.Visibility = Visibility.Visible;
                _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                _efetuaPagamentoModel.MensagemValorInvalido();
                TbPagamento.SelectAll();
                TbPagamento.Focus();

                return;
            }
            catch (PagamentoNegativoException ex)
            {
                TravaTodasOpcoesAteAcabar = false;
                DialogBox.MostraAviso(ex.Message);
                BtLancarPagamento.Visibility = Visibility.Collapsed;
                TbPagamento.Visibility = Visibility.Visible;
                TbPagamento.IsEnabled = false;
                BtLancarPagamento.Focus();
                _efetuaPagamentoModel.Pagamento = 0.ToString("N2");

                return;
            }
            catch (ValorMenorQueZeroException ex)
            {
                TravaTodasOpcoesAteAcabar = false;
                DialogBox.MostraAviso(ex.Message);
                VisibilidadeComponentesPadrao();
                _efetuaPagamentoModel.MenssagemTop = "Escolher um meio de pagamento.";
                _efetuaPagamentoModel.Informacoes.Clear();
                TbPagamento.IsEnabled = false;
                _efetuaPagamentoModel.Pagamento = 0.ToString("N2");

                return;
            }
            catch (ExceptionCartao ex)
            {
                TravaTodasOpcoesAteAcabar = false;
                DialogBox.MostraInformacao(ex.Message);
                BtLancarPagamento.Visibility = Visibility.Collapsed;
                TbPagamento.Visibility = Visibility.Visible;
                _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                TbPagamento.SelectAll();
                TbPagamento.Focus();

                return;
            }
            catch (InvalidOperationException ex)
            {
                TravaTodasOpcoesAteAcabar = false;
                Dispatcher.Invoke(() =>
                {
                    DialogBox.MostraAviso(ex.Message);
                });

                return;
            }
            catch (Exception ex)
            {
                TravaTodasOpcoesAteAcabar = false;
                Dispatcher.Invoke(() =>
                {
                    DialogBox.MostraErro(ex.Message, ex);
                });
                return;
            }

            new Thread(() =>
            {
                while (true)
                {
                    if (_efetuaPagamentoModel.TerminouDeLancarPagamento)
                    {
                        TravaTodasOpcoesAteAcabar = false;
                        return;
                    }

                    if (finalizado)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    try
                    {
                        if (_efetuaPagamentoModel.UsandoTef) return;

                        finalizado = true;
                        _efetuaPagamentoModel.AdicionaValor();

                        Dispatcher.Invoke(() =>
                        {
                            BtLancarPagamento.Visibility = Visibility.Collapsed;
                            TbPagamento.Visibility = Visibility.Visible;
                            TbPagamento.IsEnabled = false;
                            BtLancarPagamento.Focus();

                            if (!_efetuaPagamentoModel.PagamentoFoiConcluido()) return;
                            _efetuaPagamentoModel.MensagemPressioneEsc();
                            BtDinheiro.IsEnabled = false;
                            BtDesconto.IsEnabled = false;
                            BtCartao.IsEnabled = false;
                            BtCartaoPos.IsEnabled = false;
                        });
                    }
                    catch (ACBrException ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DialogBox.MostraAviso(ex.Message);
                            BtLancarPagamento.Visibility = Visibility.Collapsed;
                            TbPagamento.Visibility = Visibility.Visible;
                            TbPagamento.IsEnabled = false;
                            _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                            TbPagamento.SelectAll();
                            TbPagamento.Focus();
                            TravaTodasOpcoesAteAcabar = false;
                        });
                    }
                    catch (ExceptionValorInvalido)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            BtLancarPagamento.Visibility = Visibility.Collapsed;
                            TbPagamento.Visibility = Visibility.Visible;
                            _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                            _efetuaPagamentoModel.MensagemValorInvalido();
                            TbPagamento.SelectAll();
                            TbPagamento.Focus();
                            TravaTodasOpcoesAteAcabar = false;
                        });
                    }
                    catch (PagamentoNegativoException ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DialogBox.MostraAviso(ex.Message);
                            BtLancarPagamento.Visibility = Visibility.Collapsed;
                            TbPagamento.Visibility = Visibility.Visible;
                            TbPagamento.IsEnabled = false;
                            BtLancarPagamento.Focus();
                            _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                            TravaTodasOpcoesAteAcabar = false;
                        });
                    }
                    catch (ValorMenorQueZeroException ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DialogBox.MostraAviso(ex.Message);
                            VisibilidadeComponentesPadrao();
                            _efetuaPagamentoModel.MenssagemTop = "Escolher um meio de pagamento.";
                            _efetuaPagamentoModel.Informacoes.Clear();
                            TbPagamento.IsEnabled = false;
                            _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                            TravaTodasOpcoesAteAcabar = false;
                        });
                    }
                    catch (ExceptionCartao ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DialogBox.MostraInformacao(ex.Message);
                            BtLancarPagamento.Visibility = Visibility.Collapsed;
                            TbPagamento.Visibility = Visibility.Visible;
                            _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                            TbPagamento.SelectAll();
                            TbPagamento.Focus();
                            TravaTodasOpcoesAteAcabar = false;
                        });
                    }
                    catch (InvalidOperationException ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DialogBox.MostraAviso(ex.Message);
                            TravaTodasOpcoesAteAcabar = false;
                        });
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DialogBox.MostraErro(ex.Message, ex);
                            TravaTodasOpcoesAteAcabar = false;
                        });
                    }
                }
            }).Start();
        }

        public bool TravaTodasOpcoesAteAcabar { get; set; }

        private void VoltaCamposParaValoresPadroes()
        {
            BtLancarPagamento.Visibility = Visibility.Collapsed;
            TbPagamento.Visibility = Visibility.Visible;
            TbPagamento.IsEnabled = false;
            TbPagamento.Text = 0.ToString("N2");
            BtLancarPagamento.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _efetuaPagamentoModel.MenssagemTop = "Escolher um meio de pagamento.";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_efetuaPagamentoModel.UsandoTef) return;

            if (TravaTodasOpcoesAteAcabar) return;

            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.F1:
                    PagamentoPorDinheiro();
                    break;
                case Key.F2:
                    AdicionaNovoValor();
                    break;
                case Key.F3:
                    PagamentoPorCartaoTef();
                    break;
                case Key.F4:
                    PagamentoPorCartaoPos();
                    break;

            }
        }

        private void PagamentoPorCartaoPos()
        {
            if (TravaTodasOpcoesAteAcabar) return;
            var formaPagamento = new PagamentoCartaoPos();
            if (ValidaSeFormaPagamentoNaoEstaMapeada(formaPagamento)) return;
            if (ValidaFormaPagamento(formaPagamento))
            {
                DialogBox.MostraInformacao("Esta forma de pagamento não foi mapeada no sistema. Porfavor mapear a mesma.");
                return;
            }
            if (_efetuaPagamentoModel.UsandoTef) return;
            if (BotaoFinalizar()) return;
            if (_efetuaPagamentoModel.PagamentoFoiConcluido()) return;
            VisibilidadeComponentesPadrao();
            if (ValidaDisponibilidadePapel()) return;

            try
            {
                _efetuaPagamentoModel.MenssagemTop = "Meio de pagamento: CARTÃO POS";
                _efetuaPagamentoModel.Informacoes.Clear();
                _efetuaPagamentoModel.MensagemAdicionarUmValor();
                FazPagamento(formaPagamento);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void BtDinheiro_OnClick(object sender, RoutedEventArgs e)
        {
            PagamentoPorDinheiro();
        }

        private void PagamentoPorDinheiro()
        {
            if (TravaTodasOpcoesAteAcabar) return;
            var formaPagamento = new PagamentoDinheiro();
            if (ValidaSeFormaPagamentoNaoEstaMapeada(formaPagamento)) return;
            if (_efetuaPagamentoModel.UsandoTef) return;
            if (BotaoFinalizar()) return;
            if (_efetuaPagamentoModel.PagamentoFoiConcluido()) return;
            VisibilidadeComponentesPadrao();
            if (ValidaDisponibilidadePapel()) return;

            try
            {
                _efetuaPagamentoModel.MenssagemTop = "Meio de pagamento: DINHEIRO";
                _efetuaPagamentoModel.Informacoes.Clear();
                _efetuaPagamentoModel.MensagemAdicionarUmValor();
                FazPagamento(formaPagamento);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private bool ValidaSeFormaPagamentoNaoEstaMapeada(IPagamento formaPagamento)
        {
            var listaEcf = SessaoSistema.FormasPagamentoEcf;
            var retorno = true;

            listaEcf.ForEach(p =>
            {
                if (p.Indice.Equals(formaPagamento.FormaPagamento.CodigoEcf))
                {
                    retorno = false;
                }
            });

            if (retorno)
            {
                DialogBox.MostraInformacao("Está forma de pagamento não está mapeada corretamente, porfavor mapear a mesma.");
            }

            return retorno;
        }

        private bool BotaoFinalizar()
        {
            if (!_efetuaPagamentoModel.BotaoFinalizar()) return false;
            BtFinalizar.Focus();
            return true;
        }

        private void VisibilidadeComponentesPadrao()
        {
            BtLancarPagamento.Visibility = Visibility.Collapsed;
            TbPagamento.Visibility = Visibility.Visible;
        }

        private static bool ValidaDisponibilidadePapel()
        {
            try
            {
                if (new VerificarPapelEcf(true).Executar()) return true;
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
                return true;
            }
            return false;
        }

        private void FazPagamento(IPagamento formaPagamento)
        {
            _efetuaPagamentoModel.FormaPagamento = formaPagamento;
            _efetuaPagamentoModel.AtualizarTotais();
            _efetuaPagamentoModel.Pagamento = _efetuaPagamentoModel.Saldo.ToString("F");
            TbPagamento.IsEnabled = true;
            TbPagamento.SelectAll();
            TbPagamento.Focus();
        }

        private void EfetuaPagamento_OnClosed(object sender, EventArgs e)
        {
            _efetuaPagamentoModel.Informacoes.Clear();
            _efetuaPagamentoModel.MensagemAdicionarUmValor();
            PagamentoConcluido = _efetuaPagamentoModel.PagamentoFoiConcluido();
            _efetuaPagamentoModel.RemoveEventosTef();
        }

        private void BtDesconto_Click(object sender, RoutedEventArgs e)
        {
            AdicionaNovoValor();
        }

        private void AdicionaNovoValor()
        {
            if (TravaTodasOpcoesAteAcabar) return;
            if (_efetuaPagamentoModel.UsandoTef) return;
            if (BotaoFinalizar()) return;
            if (_efetuaPagamentoModel.PagamentoFoiConcluido()) return;
            VisibilidadeComponentesPadrao();
            if (ValidaDisponibilidadePapel()) return;

            try
            {
                _efetuaPagamentoModel.MenssagemTop = "Ajuste de Valor";
                _efetuaPagamentoModel.Informacoes.Clear();
                _efetuaPagamentoModel.MensagemNovoValor();
                FazPagamento(new PagamentoAjuste());
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void PagamentoPorCartaoTef()
        {
            if (TravaTodasOpcoesAteAcabar) return;
            var formaPagamento = new PagamentoCartaoTef();

            if (ValidaSeFormaPagamentoNaoEstaMapeada(formaPagamento)) return;

            if (ValidaFormaPagamento(formaPagamento))
            {
                DialogBox.MostraInformacao("Esta forma de pagamento não foi mapeada no sistema. Porfavor mapear a mesma.");
                return;
            }
            if (TemTefDial()) return;
            if (_efetuaPagamentoModel.UsandoTef) return;
            if (BotaoFinalizar()) return;
            if (_efetuaPagamentoModel.PagamentoFoiConcluido()) return;
            VisibilidadeComponentesPadrao();
            if (ValidaDisponibilidadePapel()) return;

            try
            {
                _efetuaPagamentoModel.MenssagemTop = "Meio de pagamento: CARTÃO TEF";
                _efetuaPagamentoModel.Informacoes.Clear();
                _efetuaPagamentoModel.MensagemAdicionarUmValor();
                FazPagamento(formaPagamento);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private bool ValidaFormaPagamento(IPagamento formaPagamento)
        {
            return string.IsNullOrEmpty(formaPagamento.FormaPagamento.CodigoEcf);
        }

        private static bool TemTefDial()
        {
            try
            {
                VerificaSeTemTef();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
                return true;
            }
            return false;
        }

        private static void VerificaSeTemTef()
        {
            var arquivoTef = new ManipulaTef();
            arquivoTef.CriarArquivoSeNaoExistir();

            var dadosTef = arquivoTef.LerArquivo();

            if(!dadosTef.Ativo) throw new InvalidOperationException("Porfavor configurar o tef para uso do mesmo");

            if(!dadosTef.ExisteGpExe()) throw new InvalidOperationException("Seu GP não está configurado corretamente, porfavor verificar o mesmo");

        }

        private void BtCartaoTef_Click(object sender, RoutedEventArgs e)
        {
            PagamentoPorCartaoTef();
        }

        private void BtLancarFinalizar_OnClick(object sender, RoutedEventArgs e)
        {
            if (_efetuaPagamentoModel.NaoPodeFinalizar()) return;
            _efetuaPagamentoModel.TerminouDeLancarPagamento = false;
            var finalizado = false;

            new Thread(() =>
            {
                while (true)
                {
                    if (_efetuaPagamentoModel.TerminouDeLancarPagamento)
                    {
                        return;
                    }

                    if (finalizado)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    try
                    {
                        if (_efetuaPagamentoModel.UsandoTef) return;

                        finalizado = true;
                        _efetuaPagamentoModel.Finaliza();

                        Dispatcher.Invoke(() =>
                        {
                            _efetuaPagamentoModel.BotaoFinalizarVisivel = false;
                            BtLancarPagamento.Visibility = Visibility.Collapsed;
                            TbPagamento.Visibility = Visibility.Visible;
                            TbPagamento.IsEnabled = false;
                            BtLancarPagamento.Focus();

                            if (!_efetuaPagamentoModel.PagamentoFoiConcluido()) return;
                            _efetuaPagamentoModel.MensagemPressioneEsc();
                            BtDinheiro.IsEnabled = false;
                            BtDesconto.IsEnabled = false;
                            BtCartao.IsEnabled = false;
                        });
                    }
                    catch (ACBrException ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DialogBox.MostraAviso(ex.Message);
                            BtLancarPagamento.Visibility = Visibility.Collapsed;
                            TbPagamento.Visibility = Visibility.Visible;
                            TbPagamento.IsEnabled = false;
                            _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                            TbPagamento.SelectAll();
                            TbPagamento.Focus();
                        });
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DialogBox.MostraErro(ex.Message, ex);
                        });
                    }
                }
            }).Start();
        }

        private void EfetuaPagamento_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = _efetuaPagamentoModel.UsandoTef;
        }

        private void BtCartaoPos_Click(object sender, RoutedEventArgs e)
        {
            PagamentoPorCartaoPos();
        }

        private void DesativaTudo(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _efetuaPagamentoModel.BotaoFinalizarVisivel = false;
                BtLancarPagamento.Visibility = Visibility.Collapsed;
                TbPagamento.IsEnabled = false;
                _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                BtDinheiro.IsEnabled = false;
                BtDesconto.IsEnabled = false;
                BtCartao.IsEnabled = false;
                BtCartaoPos.IsEnabled = false;
                KeyDown -= Window_KeyDown;
                Closing -= EfetuaPagamento_OnClosing;
                Closing += NaoPermiteFechar;
            });
        }

        private static void NaoPermiteFechar(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void AtivaTudo(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _efetuaPagamentoModel.BotaoFinalizarVisivel = false;
                BtLancarPagamento.Visibility = Visibility.Collapsed;
                TbPagamento.IsEnabled = false;
                _efetuaPagamentoModel.Pagamento = 0.ToString("N2");
                BtDinheiro.IsEnabled = true;
                BtDesconto.IsEnabled = true;
                BtCartao.IsEnabled = true;
                BtCartaoPos.IsEnabled = true;
                KeyDown += Window_KeyDown;
                Closing -= NaoPermiteFechar;
                Closing += EfetuaPagamento_OnClosing;
            });
        }


        

        private void PagamentoCrediario()
        {
            if (!_efetuaPagamentoModel.PossuiFinanceiro)
            {
                return;
            }

            if (TravaTodasOpcoesAteAcabar)
            {
                return;
            }

            var formaPagamento = new PagamentoCrediario();

            if (ValidaSeFormaPagamentoNaoEstaMapeada(formaPagamento))
            {
                return;
            }

            if (_efetuaPagamentoModel.UsandoTef)
            {
                return;
            }

            if (BotaoFinalizar())
            {
                return;
            }

            if (_efetuaPagamentoModel.PagamentoFoiConcluido())
            {
                return;
            }

            VisibilidadeComponentesPadrao();

            if (ValidaDisponibilidadePapel())
            {
                return;
            }

            try
            {
                _efetuaPagamentoModel.MenssagemTop = "Meio de pagamento: CREDIÁRIO";
                _efetuaPagamentoModel.Informacoes.Clear();
                _efetuaPagamentoModel.MensagemAdicionarUmValor();
                FazPagamento(formaPagamento);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private static void TentaRestaurarConexaoAdm()
        {
            new Thread(() =>
            {
                try
                {
                    GerenciaSessao.GerenciaSessaoInicializar();
                }
                catch
                {
                    // ignored
                }
            }).Start();
        }
    }
}