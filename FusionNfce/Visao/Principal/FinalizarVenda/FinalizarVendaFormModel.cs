using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DFe.Classes;
using FusionCore.Excecoes;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionNfce.Autorizacao;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Pagamento;
using FusionCore.FusionNfce.Servico.Historicos;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Sessao;
using FusionLibrary.Helper.Conversores;
using FusionLibrary.Validacao.Regras;
using FusionLibrary.VisaoModel;
using FusionNfce.Core;
using FusionNfce.Servicos;
using FusionNfce.Visao.Autorizacao.Emissao;
using FusionNfce.Visao.Autorizacao.SatFiscal;
using FusionNfce.Visao.Principal.FinalizarVenda.CartoesPos;
using FusionNfce.Visao.Principal.FinalizarVenda.Flyout;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento.Contratos;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento.Model;
using FusionNfce.Visao.Principal.FinalizarVenda.Outros;
using FusionNfce.Visao.Principal.FinalizarVenda.Tef.Pos;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Parcelamento;
using NHibernate;
using NHibernate.Util;
using Tef.Dominio;
using Tef.Dominio.Enums;
using AutorizacaoModelResposta = FusionNfce.Visao.Autorizacao.Emissao.AutorizacaoModelResposta;

namespace FusionNfce.Visao.Principal.FinalizarVenda
{
    public class SucessoNfceEvent : EventArgs
    {
        public SucessoNfceEvent(Nfce nfce, IImprimeViaEventArgs imprimeViaEventArgs)
        {
            Nfce = BuscarNFCe(nfce);
            ImprimeViaEventArgs = imprimeViaEventArgs;
        }

        private Nfce BuscarNFCe(Nfce nfce)
        {
            using (var sessao = new SessaoManagerNfce().CriaSessao())
            {
                return new RepositorioNfce(sessao).GetPeloId(nfce.Id);
            }
        }

        public Nfce Nfce { get;}
        public IImprimeViaEventArgs ImprimeViaEventArgs { get; }
    }

    public sealed class FinalizarVendaFormModel : ViewModel
    {
        private ITef _tef;
        private decimal _subTotal;
        private decimal _saldo;
        private decimal _total;
        private string _mensagemTop;
        private bool _textBoxPagamentoVisivel;
        private bool _buttonLancarPagamentoVisivel;
        private bool _isEnableTextBoxPagamento;
        private string _valorDigitado = 0.ToString("N2");
        private ObservableCollection<Informacao> _informacoes;
        private ObservableCollection<IFormaPagamento> _formaPagamentos;
        private bool _podeFinalizar;
        private NfceDestinatario _destinatario;
        private bool _podeAdicionarCliente;
        private bool _habilitaTransmissaoBotao;
        private BitmapImage _logo;
        private FlyoutObservacaoModel _flyoutObservacaoModel;
        private IFormaPagamento _formaPagamentoAtual;
        private readonly RestricaoClienteObrigatorio _restricaoClienteObrigatorio;
        private bool _isEmissorNfce;
        private IImprimeViaEventArgs _imprimeViaEventArgs;
        private bool _isTefAtivo;
        private RespostaCrt _retornoTefCrt;

        public FlyoutObservacaoModel FlyoutObservacaoModel
        {
            get => _flyoutObservacaoModel;
            set
            {
                if (Equals(value, _flyoutObservacaoModel)) return;
                _flyoutObservacaoModel = value;
                PropriedadeAlterada();
            }
        }

        public BitmapImage Logo
        {
            get => _logo;
            set
            {
                if (Equals(value, _logo)) return;
                _logo = value;
                PropriedadeAlterada();
            }
        }

        public Nfce Nfce { get; }

        public FormaPagamento EnumFormaPagamento { get; private set; }

        public bool HabilitaTransmissaoBotao
        {
            get => _habilitaTransmissaoBotao;
            set
            {
                if (value == _habilitaTransmissaoBotao) return;
                _habilitaTransmissaoBotao = value;
                PropriedadeAlterada();
            }
        }

        public bool PodeAdicionarCliente
        {
            get => _podeAdicionarCliente;
            set
            {
                if (value == _podeAdicionarCliente) return;
                _podeAdicionarCliente = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscaCliente => GetSimpleCommand(BuscarClienteCommand);

        public NfceDestinatario Destinatario
        {
            get => _destinatario;
            set
            {
                if (Equals(value, _destinatario)) return;
                _destinatario = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<IFormaPagamento> FormaPagamentos
        {
            get => _formaPagamentos;
            set
            {
                if (Equals(value, _formaPagamentos)) return;
                _formaPagamentos = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<Informacao> Informacoes
        {
            get => _informacoes;
            set
            {
                if (Equals(value, _informacoes)) return;
                _informacoes = value;
                PropriedadeAlterada();
            }
        }

        public bool IsEnableTextBoxPagamento
        {
            get => _isEnableTextBoxPagamento;
            set
            {
                if (value == _isEnableTextBoxPagamento) return;
                _isEnableTextBoxPagamento = value;
                PropriedadeAlterada();
            }
        }

        public bool ButtonLancarPagamentoVisivel
        {
            get => _buttonLancarPagamentoVisivel;
            set
            {
                if (value == _buttonLancarPagamentoVisivel) return;
                _buttonLancarPagamentoVisivel = value;
                PropriedadeAlterada();
            }
        }

        public bool TextBoxPagamentoVisivel
        {
            get => _textBoxPagamentoVisivel;
            set
            {
                if (value == _textBoxPagamentoVisivel) return;
                _textBoxPagamentoVisivel = value;
                PropriedadeAlterada();
            }
        }

        public string MenssagemTop
        {
            get => _mensagemTop;
            set
            {
                if (value == _mensagemTop) return;
                _mensagemTop = value;
                PropriedadeAlterada();
            }
        }

        public decimal Total
        {
            get => _total;
            set
            {
                if (value == _total) return;
                _total = value;
                PropriedadeAlterada();
            }
        }

        public decimal Saldo
        {
            get => _saldo;
            set
            {
                if (value == _saldo) return;
                _saldo = value;
                PropriedadeAlterada();
            }
        }

        public decimal SubTotal
        {
            get => _subTotal;
            set
            {
                if (value == _subTotal) return;
                _subTotal = value;
                PropriedadeAlterada();
            }
        }

        public string ValorDigitado
        {
            get => _valorDigitado;
            set
            {
                if (value == _valorDigitado) return;
                _valorDigitado = value;
                PropriedadeAlterada();
            }
        }

        public bool PodeFinalizar
        {
            get => _podeFinalizar;
            set
            {
                if (value == _podeFinalizar) return;
                _podeFinalizar = value;
                PropriedadeAlterada();
            }
        }

        public bool PossuiFinanceiro
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ICommand CommandLimparPagamentos => GetSimpleCommand(LimparPagamentosAction);

        private void LimparPagamentosAction(object obj)
        {
            if (ExisteHistoricoAbertoServico.Existe(Nfce))
            {
                DialogBox.MostraAviso("Não é possível limpar os pagamentos o cupom possui uma emissão pendente.");
                return;
            }

            if (!DialogBox.MostraDialogoDeConfirmacao("Deseja realmente deletar os pagamentos atuais?"))
            {
                return;
            }

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfce = new RepositorioNfce(sessao);
                var formasPagamentos = Nfce.ObterOsPagamentos();

                new GeraRegistroCaixa(Nfce, sessao, SessaoSistemaNfce.Usuario).EstornarCaixa();

                formasPagamentos?.ForEach(p =>
                {
                    repositorioNfce.Deletar(p);
                });

                Nfce.EstornarDesconto();
                Nfce.EstornarAcrescimo();

                if (Nfce.ExisteCobranca())
                {
                    Nfce.Cobranca.CobrancaDuplicatas.ForEach(repositorioNfce.Deletar);
                    repositorioNfce.Deletar(Nfce.Cobranca);
                }

                Nfce.RemoveCobranca();
                Nfce.RemoveFormasPagamento();

                repositorioNfce.Salvar(Nfce);

                transacao.Commit();
            }

            FormaPagamentos.Clear();
            TotaisInicial();
            InicializaBotoesIniciais();

            if (HabilitaTransmissaoBotao) OnAdicionaEventosDeBotoes();

            HabilitaTransmissaoBotao = false;
        }

        public FinalizarVendaFormModel(Nfce nfce)
        {
            Nfce = nfce;
            IsEmissorNfce = SessaoSistemaNfce.IsEmissorNFce();
            InicializaModel();
            _restricaoClienteObrigatorio = new RestricaoClienteObrigatorio();
            InicializaTef();
        }

        public bool IsEmissorNfce
        {
            get => _isEmissorNfce;
            set
            {
                if (value == _isEmissorNfce) return;
                _isEmissorNfce = value;
                PropriedadeAlterada();
            }
        }

        public bool IsTefAtivo
        {
            get => _isTefAtivo;
            set
            {
                _isTefAtivo = value && IsEmissorNfce;
                PropriedadeAlterada();
            }
        }

        public event EventHandler FocusDigitarPagamento;
        public event EventHandler RetiraEventosDeBotoes;
        public event EventHandler AdicionaEventosDeBotoes;
        public event EventHandler FinalizouPagamento;
        public event EventHandler<SucessoNfceEvent> SucessoNfce;
        public event EventHandler FocusBotaoTransmitir;

        private void LimparCampoCliente()
        {
            var temCrediario = false;

            FormaPagamentos.ForEach(f =>
            {
                if (f.GetType() == typeof (Crediario))
                {
                    temCrediario = true;
                }
            });

            if (temCrediario)
            {
                DialogBox.MostraInformacao("Não pode excluir o cliente quando se tem crediário");
                return;
            }


            if (Nfce.Destinatario == null)
            {
                Destinatario = new NfceDestinatario();
                return;
            }

            DeletarDesintatario();
        }

        private void DeletarDesintatario()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNfce(sessao);

                repositorio.Deletar(Nfce.Destinatario);

                transacao.Commit();
            }

            Nfce.Destinatario = null;
            Destinatario = new NfceDestinatario();
        }

        private void BuscarClienteCommand(object obj)
        {
            BuscarCliente();
        }

        public void BuscarCliente()
        {
            if (ExisteHistoricoAbertoServico.Existe(Nfce))
            {
                DialogBox.MostraAviso("Não é possível alterar Cliente o cupom possui uma emissão pendente.");
                return;
            }

            var model = new AddClienteVendaFormModel(Nfce.TotalNfce, Nfce.Destinatario ?? Destinatario);

            model.SalvarHandler += SalvarCliente;
            model.LimparDestinatario += LimparDestinatario;

            new AddClienteVendaForm(model).ShowDialog();
        }

        private void LimparDestinatario(object sender, EventArgs e)
        {
            LimparCampoCliente();
        }

        private void SalvarCliente(object sender, NfceDestinatario e)
        {
            Destinatario.Cliente = e.Cliente;
            Destinatario.Nome = e.Nome;
            Destinatario.DocumentoUnico = e.DocumentoUnico;
            Destinatario.Email = e.Email;
            Destinatario.Logradouro = e.Logradouro;
            Destinatario.Numero = e.Numero;
            Destinatario.Bairro = e.Bairro;
            Destinatario.Cep = e.Cep;
            Destinatario.Complemento = e.Complemento;
            Destinatario.InscricaoEstadual = e.InscricaoEstadual;
            Destinatario.Cidade = e.Cidade;

            AdicionaClienteSeOuver();
        }

        private void InicializaModel()
        {
            FlyoutObservacaoModel = new FlyoutObservacaoModel();
            FlyoutObservacaoModel.AdicionaObservacao += AdicionaObservacao;
            CarregarLogo();
            TotaisInicial();
            InicializaBotoesIniciais();
            PossuiFinanceiro = SessaoSistemaNfce.AcessoConcedido.PossuiFusionGestor;

            Informacoes = new ObservableCollection<Informacao>();
            FormaPagamentos = new ObservableCollection<IFormaPagamento>();

            Destinatario = Nfce.Destinatario ?? new NfceDestinatario();
        }

        private void InicializaBotoesIniciais()
        {
            EnumFormaPagamento = FormaPagamento.Nenhum;
            TextBoxPagamentoVisivel = true;
            ButtonLancarPagamentoVisivel = false;
            IsEnableTextBoxPagamento = false;
            PodeFinalizar = true;
            PodeAdicionarCliente = true;
        }

        private void TotaisInicial()
        {
            Total = Nfce.TotalProdutosServicos;
            SubTotal = Nfce.TotalNfce;
            Saldo = SubTotal - Nfce.ObterOsPagamentos().Where(p => p.IsAjuste == false).Sum(p => p.ValorPagamento);
        }

        private void AdicionaObservacao(object sender, RetornoAdicionaObservacao e)
        {
            Nfce.Observacao = e.Observacao;
        }

        private void CarregarLogo()
        {
            Logo = ConverteImage.ByteEmImagem(SessaoSistemaNfce.ConfiguracaoFrenteCaixa?.Logo);
        }

        public void EfetuaTransmissao()
        {
            var codigoTef = int.Parse(FormaPagamentoNfce.CartaoTef);
            var naoEncontrouFormaTef = FormaPagamentos.All(x => x.Id != codigoTef);

            if (naoEncontrouFormaTef)
            {
                EnviaParaSefazTrataException();
                return;
            }

            TentaEnviarPagamentosCartaoTef();
        }

        private void TentaEnviarPagamentosCartaoTef()
        {
            EnviaPagamentoTef();
        }

        private async void EnviaPagamentoTef()
        {
            var codigoTef = int.Parse(FormaPagamentoNfce.CartaoTef);

            var naoEncontrouFormaTef = FormaPagamentos.All(x => x.Id != codigoTef);
            if (naoEncontrouFormaTef) return;

            var pagamento = FormaPagamentos.SingleOrDefault(x => x.Id == codigoTef);

            await Task.Run(() =>
            {
                _retornoTefCrt = _tef.Crt(pagamento.Valor, Nfce.Id.ToString(), true);
            });

            var isTefAutorizadoPagamento = _retornoTefCrt.TefStatus == AcTefStatus.Sucesso;
            if (isTefAutorizadoPagamento == false) return;

            AutorizaDfeEventArgs e = new AutorizaDfeEventArgs(null);

            try
            {
                await EnviaParaSefaz(e);
            }
            catch (InvalidOperationException ex)
            {
                e.ErroDfe();
                DialogBox.MostraInformacao(ex.Message);
                FalhaNaTransmissao();
            }
            catch (ImpressaoException ex)
            {
                e.AutorizadoDfe();
                DialogBox.MostraAviso(ex.Message);
                OnSucessoNfce(Nfce);
            }
            catch
            {
                e.ErroDfe();
                FalhaNaTransmissao();
                throw;
            }

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var tefLinhaLista = _retornoTefCrt.Resposta;

                var formaPagamentoNfce = Nfce.ObterFormaPagamentoNfces().FirstOrDefault(p => p.Id == pagamento.IdRepositorio);

                var repositorioNfce = new RepositorioNfce(sessao);

                formaPagamentoNfce.IsTef = true;
                formaPagamentoNfce.Adquirente = tefLinhaLista.BuscaLinha(AcTefIdentificadorCampos.RedeAdquirente)?.Valor ?? string.Empty;
                formaPagamentoNfce.TipoTransacao = tefLinhaLista.BuscaLinha(AcTefIdentificadorCampos.TipoTransacao)?.Valor ?? string.Empty;
                formaPagamentoNfce.Nsu = tefLinhaLista.BuscaLinha(AcTefIdentificadorCampos.Nsu)?.Valor ?? string.Empty;
                formaPagamentoNfce.NumeroAprovacao = tefLinhaLista.BuscaLinha(AcTefIdentificadorCampos.CodigoAutorizacao)?.Valor ?? string.Empty;
                formaPagamentoNfce.CodigoControle = tefLinhaLista.BuscaLinha(AcTefIdentificadorCampos.CodigoControle)?.Valor ?? string.Empty;
                formaPagamentoNfce.OperadoraTef = SessaoSistemaNfce.ConfigTef.Operadora;
                formaPagamentoNfce.Bandeira = tefLinhaLista.BuscaLinha(40)?.Valor ?? string.Empty;

                var data = tefLinhaLista.BuscaLinha(AcTefIdentificadorCampos.DataComprovante).Valor;
                var horaCompleta = tefLinhaLista.BuscaLinha(AcTefIdentificadorCampos.HoraComprovante).Valor;

                if (data.IsNullOrEmpty() || horaCompleta.IsNullOrEmpty())
                {
                    formaPagamentoNfce.DataEHoraTransacao = DateTime.Now;

                    repositorioNfce.SalvarFormaPagamento(formaPagamentoNfce);
                    transacao.Commit();
                    return;
                }

                var dia = data.Substring(0, 2);
                var mes = data.Substring(2, 2);
                var ano = data.Substring(4, 4);

                var hora = horaCompleta.Substring(0, 2);
                var minuto = horaCompleta.Substring(2, 2);
                var segundos = horaCompleta.Substring(4, 2);

                formaPagamentoNfce.DataEHoraTransacao = new DateTime(int.Parse(ano), int.Parse(mes), int.Parse(dia),
                    int.Parse(hora), int.Parse(minuto), int.Parse(segundos));

                repositorioNfce.SalvarFormaPagamento(formaPagamentoNfce);
                transacao.Commit();
            }
        }

        private void HabilitaTransmissao()
        {
            HabilitaTransmissaoBotao = true;
            TextBoxPagamentoVisivel = false;
            ButtonLancarPagamentoVisivel = false;
        }

        private void DesabilitaCampos()
        {
            Informacoes.Clear();
            Informacoes.Add(new Informacao("Recuperação de nota com sucesso"));
            PodeAdicionarCliente = false;
            PodeFinalizar = false;
            OnRetiraEventosDeBotoes();
        }

        private void IniciaValorDigitado()
        {
            ValorDigitado = Saldo.ToString("N2");
        }

        public void UsarDinheiro()
        {
            if (ValidacaoAntesPagar()) return;
            MenssagemTop = "Meio de pagamento: DINHEIRO" + VendaModel.Homologacao();
            IsEnableTextBoxPagamento = true;
            EnumFormaPagamento = FormaPagamento.Dinheiro;
            IniciaValorDigitado();
            OnFocusDigitarPagamento();
        }

        public void UsarOutros()
        {
            if (ValidacaoAntesPagar()) return;
            MenssagemTop = "Meio de pagamento: OUTROS" + VendaModel.Homologacao();
            IsEnableTextBoxPagamento = true;
            EnumFormaPagamento = FormaPagamento.Outros;
            IniciaValorDigitado();
            OnFocusDigitarPagamento();
        }

        public void UsarCartaoPos()
        {
            if (IsEmissorNfce) return;
            if (ValidacaoAntesPagar()) return;
            MenssagemTop = "Meio de pagamento: CARTÃO POS" + VendaModel.Homologacao();
            IsEnableTextBoxPagamento = true;
            EnumFormaPagamento = FormaPagamento.CartaoPos;
            IniciaValorDigitado();
            OnFocusDigitarPagamento();
        }

        public void UsarCartaoTef()
        {
            if (!IsTefAtivo) return;
            if (IsEmissorNfce == false) return;
            if (ValidacaoAntesPagar()) return;
            if (AceitaApenasUmCartaoTef()) return;
            MenssagemTop = "Meio de pagamento: CARTÃO TEF" + VendaModel.Homologacao();
            IsEnableTextBoxPagamento = true;
            EnumFormaPagamento = FormaPagamento.CartaoTef;
            IniciaValorDigitado();
            OnFocusDigitarPagamento();
        }

        private bool AceitaApenasUmCartaoTef()
        {
            var existeCartaoTefJaLancado = FormaPagamentos.Any(x => x.Id == int.Parse(FormaPagamentoNfce.CartaoTef));

            if (existeCartaoTefJaLancado == false) return false;

            DialogBox.MostraAviso("Existe Cartão Tef já lançado, somente pode um por venda");
            return true;

        }

        public void UsarCartaoCredito()
        {
            if (IsEmissorNfce == false) return;
            if (ValidacaoAntesPagar()) return;
            MenssagemTop = "Meio de pagamento: CARTÃO CRÉDITO" + VendaModel.Homologacao();
            IsEnableTextBoxPagamento = true;
            EnumFormaPagamento = FormaPagamento.CartaoCredito;
            IniciaValorDigitado();
            OnFocusDigitarPagamento();
        }

        public void UsarCartaoDebito()
        {
            if (IsEmissorNfce == false) return;
            if (ValidacaoAntesPagar()) return;
            MenssagemTop = "Meio de pagamento: CARTÃO DÉBITO" + VendaModel.Homologacao();
            IsEnableTextBoxPagamento = true;
            EnumFormaPagamento = FormaPagamento.CartaoDebito;
            IniciaValorDigitado();
            OnFocusDigitarPagamento();
        }

        public void UsarPix()
        {
            if (ValidacaoAntesPagar()) return;
            MenssagemTop = "Meio de pagamento: PIX" + VendaModel.Homologacao();
            IsEnableTextBoxPagamento = true;
            EnumFormaPagamento = FormaPagamento.Pix;
            IniciaValorDigitado();
            OnFocusDigitarPagamento();
        }

        private bool ExisteAcrescimo()
        {
            var isExisteAcrescimo = Nfce
                .ObterFormaPagamentoNfces()
                .Any(f => f.IsAjuste == true && f.AjusteTipo == AjusteTipo.Acrescimo);

            if (!isExisteAcrescimo) return false;

            DialogBox.MostraInformacao("Quando existe acréscimo não pode adicionar desconto");

            return true;
        }

        private bool ExisteDesconto()
        {
            var isExisteDesconto = Nfce
                .ObterFormaPagamentoNfces()
                .Any(f => f.IsAjuste == true && f.AjusteTipo == AjusteTipo.Desconto);

            var isDescontoAlteraItem = Nfce.TotalDesconto != 0;

            if (isExisteDesconto)
            {
                DialogBox.MostraInformacao("Quando existe desconto não pode adicionar acréscimo");

                return true;
            }

            if (isDescontoAlteraItem)
            {
                DialogBox.MostraInformacao("Quando existe desconto não pode adicionar acréscimo");

                return true;
            }


            return false;
        }

        private bool ValidacaoAntesPagar()
        {
            if (!IsEnableTextBoxPagamento) return false;

            MenssagemTop = "Esperando ação" + VendaModel.Homologacao();
            ButtonLancarPagamentoVisivel = false;
            TextBoxPagamentoVisivel = true;
            IsEnableTextBoxPagamento = false;
            EnumFormaPagamento = FormaPagamento.Nenhum;
            return true;
        }

        public void LancaPagamento(decimal? acrescimoOuDesconto = null)
        {
            try
            {
                if (acrescimoOuDesconto != null)
                {
                    ValorDigitado = acrescimoOuDesconto.Value.ToString("N2");
                }
                var valor = decimal.Parse(ValorDigitado);

                ValidaValorMenorQueZero(valor);

                ValidaAjusteDeSaldo(valor);


                Informacoes.Clear();
                Informacoes.Add(new Informacao("Valor: " + valor.ToString("N2")));
            }
            catch (OverflowException)
            {
                throw new InvalidOperationException("Valor está muito grande");
            }
            catch (FormatException)
            {
                throw new InvalidOperationException(
                    "Porfavor digitar valores somente com uma virgula ex: 10,50 ou 1009,20 ou 10000,53");
            }

            CalculaSeTemTroco();

            IsEnableTextBoxPagamento = false;
            TextBoxPagamentoVisivel = false;
            ButtonLancarPagamentoVisivel = true;
        }

        private void ValidaAjusteDeSaldo(decimal valor)
        {
            if (EnumFormaPagamento != FormaPagamento.AjusteSaldo && EnumFormaPagamento != FormaPagamento.Desconto && EnumFormaPagamento != FormaPagamento.Acrescimo) return;

            if (EnumFormaPagamento != FormaPagamento.Desconto) return;

            if (valor >= Saldo)
            {
                throw new InvalidOperationException("O Desconto não pode ser maior ou igual que o Saldo");
            }
        }

        private static void ValidaValorMenorQueZero(decimal valor)
        {
            if (valor <= 0)
            {
                throw new InvalidOperationException(
                    "Não e permitido um valor menor ou igual a zero, tente outro valor obrigado.");
            }
        }

        private void CalculaSeTemTroco()
        {
            if (EnumFormaPagamento == FormaPagamento.AjusteSaldo || EnumFormaPagamento == FormaPagamento.Desconto || EnumFormaPagamento == FormaPagamento.Acrescimo) return;

            var valorDigitado = decimal.Parse(ValorDigitado);

            if (VerificaSeTemTroco()) return;

            var troco = (valorDigitado+FormaPagamentos.Where(f => f.Id != 10).Sum(f => f.Valor)) - SubTotal;

            if (EnumFormaPagamento == FormaPagamento.Crediario)
            {
                throw new InvalidOperationException("Não e permitido troco nesta forma de pagamento => " +
                                                    EnumFormaPagamento);
            }

            Nfce.Troco = troco;
            Informacoes.Add(new Informacao("Troco: " + troco.ToString("N2")));
        }

        private bool VerificaSeTemTroco()
        {
            return ((decimal.Parse(ValorDigitado) + FormaPagamentos.Where(f => f.Id != 10).Sum(f => f.Valor)) <= SubTotal);
        }

        public void EfetuarPagamento(
            CartaoPosFormModel cartaoPosFormModel = null, DescricaoOutrosFormModel descricaoOutrosModel = null,
            EfetuaPagamentoPosFormModel efetuaPagamentoPosFormModel = null)
        {
            Informacoes.Clear();
            Informacoes.Add(new Informacao("Operação efetuada com sucesso"));

            var valor = decimal.Parse(ValorDigitado);

            _formaPagamentoAtual = EnumFormaPagamento.GetFormaPagamento(valor);
            FormaPagamentos.Add(_formaPagamentoAtual); 

            var pagamento = SalvarPagamento(cartaoPosFormModel, descricaoOutrosModel, efetuaPagamentoPosFormModel);

            ButtonLancarPagamentoVisivel = false;
            TextBoxPagamentoVisivel = true;
            IsEnableTextBoxPagamento = false;
            ValorDigitado = 0.ToString("N2");

            if (AjusteDeSaldo(valor, pagamento))
            {
                return;
            }

            AtualizaTotais(valor);

            Finalizar();

            MenssagemTop = "Operação efetuada com sucesso" + VendaModel.Homologacao();
        }

        private FormaPagamentoNfce SalvarPagamento(
            CartaoPosFormModel cartaoPosFormModel = null, DescricaoOutrosFormModel descricaoOutrosModel = null, EfetuaPagamentoPosFormModel efetuaPagamentoPosFormModel = null)
        {
            var pagamento = new FormaPagamentoNfce
            {
                Nfce = Nfce,
                Nome = _formaPagamentoAtual.Descricao,
                ValorPagamento = _formaPagamentoAtual.Valor,
                IdFormaPagamento = _formaPagamentoAtual.Id.ToString("D2"),
                IsMfe = SessaoSistemaNfce.IsMFe()
            };

            if (cartaoPosFormModel != null && SessaoSistemaNfce.Preferencia.SolicitaDadosCartaoPos)
            {
                pagamento.CnpjCredenciadora = cartaoPosFormModel.CnpjCredenciadora.TrimOrEmpty();
                pagamento.Bandeira = cartaoPosFormModel.CartaoBandeira.ToString();
                pagamento.NumeroAprovacao = cartaoPosFormModel.NumeroAutorizacao.TrimOrEmpty();
            }

            if (descricaoOutrosModel != null)
            {
                pagamento.DescricaoOutros = descricaoOutrosModel.DescricaoOutros;
            }

            if (efetuaPagamentoPosFormModel != null)
            {
                pagamento.Credenciadora = efetuaPagamentoPosFormModel.Credenciadora;
                pagamento.TipoCartaoPos = efetuaPagamentoPosFormModel.TipoPagamentoCartaoPos;
            }

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioFormaPagamentoNfce = new RepositorioNfce(sessao);
                repositorioFormaPagamentoNfce.SalvarFormaPagamento(pagamento);
                Nfce.AddFormaPagamento(pagamento);

                transacao.Commit();
            }

            _formaPagamentoAtual.IdRepositorio = pagamento.Id;

            return pagamento;
        }

        private bool AjusteDeSaldo(decimal valor, FormaPagamentoNfce pagamento = null)
        {
            if (EnumFormaPagamento != FormaPagamento.AjusteSaldo && EnumFormaPagamento != FormaPagamento.Desconto && EnumFormaPagamento != FormaPagamento.Acrescimo) return false;

            if (EnumFormaPagamento == FormaPagamento.Desconto)
            {
                if (pagamento != null)
                {
                    pagamento.IsAjuste = true;
                    pagamento.AjusteTipo = AjusteTipo.Desconto;
                }

                var totalDistribuicao = FormaPagamentos.Where(f => f.Id == 10).Sum(f => f.Valor);

                Nfce.ZerarDescontoItensRateado();
                Nfce.DistribuiDesconto(totalDistribuicao);
            }

            if (EnumFormaPagamento == FormaPagamento.Acrescimo)
            {
                if (pagamento != null)
                {
                    pagamento.IsAjuste = true;
                    pagamento.AjusteTipo = AjusteTipo.Acrescimo;
                }

                var totalDistribuicao = FormaPagamentos.Where(f => f.Id == 10).Sum(f => f.Valor);

                Nfce.ZerarAcrescimoItensRateado();
                Nfce.DistribuiAcrescimo(totalDistribuicao);
            }

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {

                if (pagamento != null)
                    SalvaPagamento(pagamento, sessao);

            
                SalvarNfce(sessao);
                transacao.Commit();
            }


            if (EnumFormaPagamento == FormaPagamento.Desconto)
            {
                Saldo -= valor;
                SubTotal -= valor;
            }

            if (EnumFormaPagamento == FormaPagamento.Acrescimo)
            {
                Saldo += valor;
                SubTotal += valor;
            }

            

            return true;
        }

        private static void SalvaPagamento(FormaPagamentoNfce pagamento, ISession sessao)
        {
            var repositorioFormaPagamentoNfce = new RepositorioNfce(sessao);
            repositorioFormaPagamentoNfce.SalvarFormaPagamento(pagamento);
        }

        private void Finalizar()
        {
            if (!VerificaSePodeFinalizar()) return;
            VerificaSePodeTransmitir();

            var codigoTef = int.Parse(FormaPagamentoNfce.CartaoTef);
            var naoEncontrouFormaTef = FormaPagamentos.All(x => x.Id != codigoTef);

            if (naoEncontrouFormaTef)
            {
                EnviaParaSefazTrataException();
                return;
            }

            TentaEnviarPagamentosCartaoTef();
        }

        private void SalvarNfce(ISession sessao)
        {
            var repositorioNfce = new RepositorioNfce(sessao);

            var temCrediario = FormaPagamentos.Where(fp => fp.Id == 3).ToList();

            if (temCrediario.IsNotNullOrEmpty())
            {
                Nfce.FormaPagamento = FusionCore.FusionAdm.Fiscal.Flags.FormaPagamento.Aprazo;
            }

            Nfce.Contingencia = SessaoSistemaNfce.Contingencia;

            repositorioNfce.SalvarESincronizar(Nfce);
        }

        private void AdicionaClienteSeOuver()
        {
            if (ValidaDestinatario()) return;

            AdicionaDestinatarioNaNfce();

            PersisteDestinatario();
        }

        private void AdicionaDestinatarioNaNfce()
        {
            Destinatario.Nfce = Nfce;
            Nfce.Destinatario = Destinatario;
        }

        private void PersisteDestinatario()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfce = new RepositorioNfce(sessao);
                repositorioNfce.SalvarESincronizar(Nfce);

                transacao.Commit();
            }
        }

        private bool ValidaDestinatario()
        {
            if (Destinatario.DocumentoUnico.IsNullOrEmpty()) return true;

            if (!new DocumentoUnicoRegra().AplicaRegra(Destinatario.DocumentoUnico))
            {
                DialogBox.MostraInformacao("Ops, Cpf/Cnpj esta inválido, verificar o mesmo");
                return true;
            }

            if (Destinatario.Nome.IsNotNullOrEmpty())
                if (Destinatario.Nome.Length < 2)
                {
                    DialogBox.MostraInformacao("Nome do cliente deve ser maior que 2 digitos");
                    return true;
                }

            return false;
        }

        private void VerificaPagamentos(ISession sessao)
        {
            if (Nfce.ObterOsPagamentos() != null && Nfce.ObterOsPagamentos().Sum(p => p.ValorPagamento) == SubTotal)
            {
                if (FormaPagamentos.IsNotNullOrEmpty())
                {
                    FormaPagamentos.Remove(_formaPagamentoAtual);
                }
                return;
            }

            var repositorioFormaPagamentoNfce = new RepositorioNfce(sessao);

            DistribuiTrocoSeTiver(repositorioFormaPagamentoNfce);
        }

        private void DistribuiTrocoSeTiver(RepositorioNfce repositorioFormaPagamentoNfce)
        {
            if (SessaoSistemaNfce.IsEmissorSat() || SessaoSistemaNfce.IsMFe()) return;

            if (EnumFormaPagamento == FormaPagamento.AjusteSaldo) return;

            var soma = FormaPagamentos.Where(f => f.Id != 10).Sum(f => f.Valor);

            var temTroco = soma - SubTotal;
            if (temTroco <= 0) return;

            var ultimaFormaDePagamentoAdicionada = FormaPagamentos.Last();

            ultimaFormaDePagamentoAdicionada.DescontaTroco(temTroco);

            var formaPagamentoNfce = Nfce.ObterOsPagamentos().SingleOrDefault(p => p.Id == ultimaFormaDePagamentoAdicionada.IdRepositorio);

            formaPagamentoNfce.ValorPagamento = ultimaFormaDePagamentoAdicionada.Valor;

            repositorioFormaPagamentoNfce.SalvarFormaPagamento(formaPagamentoNfce);
        }

        private async void EnviaParaSefazTrataException()
        {
            try
            {
                await EnviaParaSefaz();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
                FalhaNaTransmissao();
            }
            catch (ImpressaoException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                OnSucessoNfce(Nfce);
            }
            catch 
            {
                FalhaNaTransmissao();
                throw;
            }
        }

        private async Task EnviaParaSefaz(AutorizaDfeEventArgs autorizaDfeEventArgs = null)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                SalvarNfce(sessao);
                VerificaPagamentos(sessao);
                SalvarEmitente(sessao);

                transacao.Commit();
            }

            if (SessaoSistemaNfce.IsEmissorSat())
            {
                var autorizaSatView = new AutorizacaoSatView(Nfce);
                var resposta = await autorizaSatView.AutorizaAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (resposta.Sucesso)
                    {
                        autorizaSatView.Close();
                    }
                });

                if (!resposta.Sucesso)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (Saldo != 0)
                        {
                            return;
                        }

                        DesabilitaCampos();
                        HabilitaTransmissao();
                        OnFocusBotaoTransmitir();
                    });

                    return;
                }

                OnSucessoNfce(Nfce);
                return;
            }


            if (SessaoSistemaNfce.IsEmissorNFce())
            {
                var autorizaView = new AutorizacaoNfceView(Nfce);
                var resposta = await autorizaView.AutorizaAsync();

                await FinalizaTransacaoTef(autorizaDfeEventArgs, resposta);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (resposta.Sucesso)
                    {
                        autorizaView.Close();
                    }
                });


                if (!resposta.Sucesso)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (Saldo != 0) return;

                        DesabilitaCampos();
                        HabilitaTransmissao();
                        OnFocusBotaoTransmitir();
                    });
                    return;
                }

                OnSucessoNfce(Nfce);
            }
        }

        private async Task FinalizaTransacaoTef(AutorizaDfeEventArgs autorizaDfeEventArgs, AutorizacaoModelResposta resposta)
        {
            if (Nfce.IsTemFormaPagamentoTef() == false) return;

            if (resposta.Sucesso)
            {
                autorizaDfeEventArgs?.AutorizadoDfe();
                ConfirmaTef(autorizaDfeEventArgs);
            }

            if (resposta.Sucesso == false)
            {
                autorizaDfeEventArgs?.RejeicaoDfe();
                ConfirmaTef(autorizaDfeEventArgs);
            }
        }

        private void FalhaNaTransmissao()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Saldo != 0)
                {
                    return;
                }

                DesabilitaCampos();
                HabilitaTransmissao();
                OnFocusBotaoTransmitir();
            });
        }

        private void SalvarEmitente(ISession sessao)
        {
            if (Nfce.Emitente != null) return;

            var repositorioNfce = new RepositorioNfce(sessao);

            var emitente = new NfceEmitente
            {
                Nfce = Nfce,
                Empresa = SessaoSistemaNfce.Configuracao.EmissorFiscal.Empresa
            };

            Nfce.Emitente = emitente;

            repositorioNfce.SalvarEmitente(Nfce.Emitente);
        }

        private bool VerificaSePodeFinalizar()
        {
            return Saldo <= 0;
        }

        private void AtualizaTotais(decimal valor)
        {
            Saldo -= valor;
            if (Saldo < 0)
                Saldo = 0;
        }

        private void OnFocusDigitarPagamento()
        {
            FocusDigitarPagamento?.Invoke(this, EventArgs.Empty);
        }

        private void OnRetiraEventosDeBotoes()
        {
            RetiraEventosDeBotoes?.Invoke(this, EventArgs.Empty);
        }

        private void OnFinalizouPagamento()
        {
            FinalizouPagamento?.Invoke(this, EventArgs.Empty);
        }

        private void OnSucessoNfce(Nfce nfce)
        {
            SucessoNfce?.Invoke(this, new SucessoNfceEvent(nfce, _imprimeViaEventArgs));
        }

        private void ConfirmaTef(AutorizaDfeEventArgs autorizaDfeEventArgs)
        {
            var isTemFormaPagamentoTef = Nfce.IsTemFormaPagamentoTef();

            if (SessaoSistemaNfce.ConfigTef.IsAtivo && isTemFormaPagamentoTef)
                _retornoTefCrt = _tef.ConfirmarCrt(autorizaDfeEventArgs);
        }

        public void SucessoLimparCampos()
        {
            ButtonLancarPagamentoVisivel = false;
            TextBoxPagamentoVisivel = true;
            IsEnableTextBoxPagamento = false;
            ValorDigitado = 0.ToString("N2");
            PodeFinalizar = false;
            PodeAdicionarCliente = false;
            OnRetiraEventosDeBotoes();
            OnFinalizouPagamento();
        }

        public void Recuperacao()
        {
            var pagamentos = Nfce.ObterOsPagamentos();

            pagamentos?.ForEach(fp =>
            {
                IFormaPagamento formaPagamento = new Dinheiro(fp.ValorPagamento)
                {
                    IdRepositorio = fp.Id,
                    FormaPagamentoNfce = fp
                };

                EnumFormaPagamento = FormaPagamento.Dinheiro;

                if (fp.IdFormaPagamento.Equals(FormaPagamentoNfce.CartaoPos))
                {
                    formaPagamento = new CartaoPos(fp.ValorPagamento)
                    {
                        IdRepositorio = fp.Id,
                        FormaPagamentoNfce = fp
                    };
                    EnumFormaPagamento = FormaPagamento.CartaoPos;
                }

                if (fp.IdFormaPagamento.Equals(FormaPagamentoNfce.CartaoDebito))
                {
                    formaPagamento = new CartaoDebito(fp.ValorPagamento)
                    {
                        IdRepositorio = fp.Id,
                        FormaPagamentoNfce = fp
                    };
                    EnumFormaPagamento = FormaPagamento.CartaoDebito;
                }

                if (fp.IdFormaPagamento.Equals(FormaPagamentoNfce.Pix))
                {
                    formaPagamento = new Pix(fp.ValorPagamento)
                    {
                        IdRepositorio = fp.Id,
                        FormaPagamentoNfce = fp
                    };
                    EnumFormaPagamento = FormaPagamento.Pix;
                }

                if (fp.IdFormaPagamento.Equals(FormaPagamentoNfce.CartaoTef))
                {
                    formaPagamento = new CartaoTef(fp.ValorPagamento)
                    {
                        IdRepositorio = fp.Id,
                        FormaPagamentoNfce = fp
                    };
                    EnumFormaPagamento = FormaPagamento.CartaoTef;
                }

                if (fp.IdFormaPagamento.Equals(FormaPagamentoNfce.Outros))
                {
                    formaPagamento = new FormasPagamento.Outros(fp.ValorPagamento)
                    {
                        IdRepositorio = fp.Id,
                        FormaPagamentoNfce = fp
                    };
                    EnumFormaPagamento = FormaPagamento.Outros;
                }

                if (fp.IdFormaPagamento.Equals(FormaPagamentoNfce.CartaoCredito))
                {
                    formaPagamento = new CartaoCredito(fp.ValorPagamento)
                    {
                        IdRepositorio = fp.Id,
                        FormaPagamentoNfce = fp
                    };
                    EnumFormaPagamento = FormaPagamento.CartaoCredito;
                }

                if (fp.IdFormaPagamento.Equals("03"))
                {
                    formaPagamento = new Crediario(fp.ValorPagamento)
                    {
                        IdRepositorio = fp.Id,
                        FormaPagamentoNfce = fp
                    };
                    EnumFormaPagamento = FormaPagamento.Crediario;
                }

                if (fp.IdFormaPagamento.Equals("10"))
                {
                    var descricao = fp.AjusteTipo == AjusteTipo.Desconto ? "Desconto" : "Acréscimo";

                    formaPagamento = new AjusteSaldo(fp.ValorPagamento, descricao)
                    {
                        IdRepositorio = fp.Id,
                        FormaPagamentoNfce = fp
                    };

                    if (fp.AjusteTipo == AjusteTipo.Desconto)
                    {
                        EnumFormaPagamento = FormaPagamento.Desconto;
                    }

                    if (fp.AjusteTipo == AjusteTipo.Acrescimo)
                    {
                        EnumFormaPagamento = FormaPagamento.Acrescimo;
                    }
                }

                FormaPagamentos.Add(formaPagamento);
            });

            EnumFormaPagamento = FormaPagamento.Nenhum;

            VerificaSePodeTransmitir();
        }

        private void VerificaSePodeTransmitir()
        {
            if (!VerificaSePodeFinalizar()) return;

            DesabilitaCampos();
            HabilitaTransmissao();
            OnFocusBotaoTransmitir();
        }

        private void OnFocusBotaoTransmitir()
        {
            FocusBotaoTransmitir?.Invoke(this, EventArgs.Empty);
        }

        public void UsarCrediario()
        {
            VerificaSeJaTemCrediarioLancado();
            if (ValidacaoAntesPagar()) return;

            MenssagemTop = "Meio de pagamento: CREDIÁRIO" + VendaModel.Homologacao();
            IsEnableTextBoxPagamento = true;
            EnumFormaPagamento = FormaPagamento.Crediario;
            IniciaValorDigitado();
            OnFocusDigitarPagamento();
        }

        private void VerificaSeJaTemCrediarioLancado()
        {
            if (_formaPagamentos.Any(fp => fp.Id == Crediario.IdCrediario))
            {
                throw new InvalidOperationException("Permitido apenas um crediário");
            }
        }

        public void AdicionarObservacao()
        {
            if (ExisteHistoricoAbertoServico.Existe(Nfce))
            {
                DialogBox.MostraAviso("Não é possível editar observação o cupom possui uma emissão pendente.");
                return;
            }

            FlyoutObservacaoModel.IsOpen = true;
            FlyoutObservacaoModel.Observacao = Nfce.Observacao;
        }

        public AddClienteVendaFormModel CriarModelClienteObrigatorio()
        {
            var model = new AddClienteVendaFormModel(Nfce.TotalNfce, Nfce.Destinatario);
            model.SalvarHandler += SalvarCliente;
            model.LimparDestinatario += LimparDestinatario;
            model.ExibirMensagemCliente = true;

            return model;
        }

        public bool NecessarioInformarCliente()
        {
            return _restricaoClienteObrigatorio.NecessarioCliente(Nfce);
        }

        public void AdicionaCobrancaNaNFCe(ParcelamentoArgs parcelamento) 
        {
            var valorTotal = parcelamento.Parcelas.Sum(i => i.Valor);

            var cobranca = new NfceCobranca
            {
                Descricao = "",
                ValorDesconto = 0,
                ValorLiquido = valorTotal,
                ValorOriginal = valorTotal,
                Nfce = Nfce,
                NumeroFatura = $"NFCE #ID: {Nfce.Id}",
                CentroLucroId = null,
                ValorEntrada = 0.00M,
                TipoDocumento = (TipoDocumento) parcelamento.TipoDocumento,
                DiaParcelaFixa = DateTime.Now.Day,
                TipoParcela = TipoParcela.Intervalo
            };

            Nfce.Cobranca = cobranca;

            foreach (var p in parcelamento.Parcelas)
            {
                var duplicata = new NfceCobrancaDuplicata
                {
                    Cobranca = cobranca,
                    Descricao = cobranca.TipoDocumento.Descricao,
                    Valor = p.Valor,
                    NumeroDuplicata = p.Numero,
                    EmitidoEm = DateTime.Now,
                    VenceEm = p.Vencimento
                };

                Nfce.Cobranca.Add(duplicata);
            }

            SalvarCobranca();
        }

        private void SalvarCobranca()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfce = new RepositorioNfce(sessao);

                repositorioNfce.Salvar(Nfce.Cobranca);
                Nfce.Cobranca.CobrancaDuplicatas.ForEach(repositorioNfce.Salvar);

                transacao.Commit();
            }
        }

        private void InicializaTef()
        {
            IsTefAtivo = SessaoSistemaNfce.ConfigTef.IsAtivo;
            var requisicao = FabricaTef.ObterRequisicao();
            requisicao.AguardandoResposta += TefAguardandoRequisicao;
            requisicao.ExibeMensagem += TefExibeMensagemAction;
            requisicao.ImprimirVia += TefImprimirViaAction;

            _tef = FabricaTef.ObterTefDial(requisicao);
            _tef.Inicializa();
        }

        private void TefImprimirViaAction(object sender, IImprimeViaEventArgs e)
        {
            _imprimeViaEventArgs = e;
        }

        private void TefExibeMensagemAction(object sender, ExibeMensagemEventArgs e)
        {
            DialogBox.MostraInformacao(e.Mensagem);
        }

        private async void TefAguardandoRequisicao(object sender, AguardaRespostaEventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync(() => { MenssagemTop = $"{e.ArquivoSts} {e.Segundos}"; });
        }

        private void OnAdicionaEventosDeBotoes()
        {
            AdicionaEventosDeBotoes?.Invoke(this, EventArgs.Empty);
        }

        public bool Acrescimo()
        {
            if (ExisteDesconto()) return false;
            if (ValidacaoAntesPagar()) return false;

            EnumFormaPagamento = FormaPagamento.Acrescimo;
            IniciaValorDigitado();
            return true;
        }

        public bool Desconto()
        {
            if (ExisteAcrescimo()) return false;
            if (ValidacaoAntesPagar()) return false;

            EnumFormaPagamento = FormaPagamento.Desconto;
            IniciaValorDigitado();
            return true;
        }

        public bool NãoPossuiCliente()
        {
            return Destinatario?.Cliente?.Id == null;
        }
    }
}