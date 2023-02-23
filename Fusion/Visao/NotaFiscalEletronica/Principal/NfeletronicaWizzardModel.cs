using System;
using System.IO;
using System.Linq;
using System.Windows;
using Fusion.Sessao;
using Fusion.Visao.NotaFiscalEletronica.EmissaoSefaz;
using Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models;
using Fusion.Visao.NotaFiscalEletronica.Principal.ExceptionCustom;
using Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts.Models;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Autorizacao;
using FusionCore.FusionAdm.Fiscal.NF.Integridade;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.Controls;

// ReSharper disable MemberCanBePrivate.Global

namespace Fusion.Visao.NotaFiscalEletronica.Principal
{
    public sealed class NfeletronicaWizzardModel : ViewModel
    {
        private Nfeletronica _nfe;
        private AbaPerfilNfeDTO _abaPerfil;
        private Cliente _clienteDoPerfil;
        private Transportadora _transportadoraDoPerfil;
        private readonly ISessaoManager _sessaoManager;
        private bool _closeRequestInvoked;
        private EmissorFiscal _emissorDoPerfil;
        private Veiculo _veiculoDoPerfil;
        private readonly UsuarioDTO _usuario = SessaoSistema.ObterUsuarioLogado();

        public NfeletronicaWizzardModel(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;

            AbaItensModel = new AbaItensNfeModel(_sessaoManager);
            AbaPerfilModel = new AbaPerfilPickerModel();
            AbaDestinoModel = new AbaDestinatarioModel();
            AbaCabecalhoModel = new AbaCabecalhoModel(_sessaoManager);

            IsEnabled = true;
        }

        public AbaPerfilPickerModel AbaPerfilModel
        {
            get => GetValue<AbaPerfilPickerModel>();
            set => SetValue(value);
        }

        public AbaDestinatarioModel AbaDestinoModel
        {
            get => GetValue<AbaDestinatarioModel>();
            set => SetValue(value);
        }

        public AbaCabecalhoModel AbaCabecalhoModel
        {
            get => GetValue<AbaCabecalhoModel>();
            set => SetValue(value);
        }

        public AbaItensNfeModel AbaItensModel
        {
            get => GetValue<AbaItensNfeModel>();
            set => SetValue(value);
        }

        public bool Selecionado
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public FlyoutReferenciaNfeModel FlyoutReferenciaNfeModel
        {
            get => GetValue<FlyoutReferenciaNfeModel>();
            private set => SetValue(value);
        }

        public FlyoutReferenciaCfModel FlyoutReferenciaCfModel
        {
            get => GetValue<FlyoutReferenciaCfModel>();
            private set => SetValue(value);
        }

        public FlyoutAlteraEmissorModel FlyoutAlteraEmissorModel
        {
            get => GetValue<FlyoutAlteraEmissorModel>();
            set => SetValue(value);
        }

        public bool IsEnabled
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsNotaNova => _nfe == null || _nfe.Id == 0;

        public event EventHandler CloseRequest;

        public void Inicializar()
        {
            AbaPerfilModel.PerfilSelecionadoCalled += PerfilSelecionadoHandler;

            AbaCabecalhoModel.ProximoPassoCalled += AbaCabecalhoProximoPassoHandler;
            AbaCabecalhoModel.AlterarEmissorCalled += AlterarEmissorHandler;
            AbaCabecalhoModel.AlterarNumeroCalled += AlterarNumeroHandler;

            AbaDestinoModel.PassoAnteriorCalled += AbaDestinoPassoAnteriorHandler;
            AbaDestinoModel.ProximoPassoCalled += AbaDestinoProximoPassoHandler;

            AbaItensModel.EmiteNfeCalled += EmiteNfeHandler;
            AbaItensModel.ReferenciarNfeCalled += ReferenciasNfeHandler;
            AbaItensModel.ReferenciarCfCalled += ReferenciarCfHandler;
            AbaItensModel.PassoAnteriorCalled += AbaItensPassoAnteriorHandler;
        }

        private void PerfilSelecionadoHandler(object sender, PerfilSelecionadoEventArgs e)
        {
            try
            {
                _abaPerfil = e.Perfil;

                using (var sessao = _sessaoManager.CriaSessao())
                {
                    var repositorioEmissor = new RepositorioEmissorFiscal(sessao);
                    var repositorioPessoa = new RepositorioPessoa(sessao);
                    var repositorioVeiculo = new RepositorioVeiculo(sessao);

                    _emissorDoPerfil = repositorioEmissor.GetPeloId(_abaPerfil.EmissorFiscalId);
                    _clienteDoPerfil = repositorioPessoa.GetClientePeloId(_abaPerfil.DestinatarioId);
                    _transportadoraDoPerfil = repositorioPessoa.GetTransportadoraPeloId(_abaPerfil.TransportadoraId);
                    _veiculoDoPerfil = repositorioVeiculo.GetPeloId(_abaPerfil.VeiculoId);
                }

                if (IsNotaNova && _nfe == null)
                {
                    _nfe = new Nfeletronica(new EmitenteNfe(_emissorDoPerfil), _usuario)
                    {
                        PerfilId = _abaPerfil.Id
                    };
                }

                AbreAbaCabecalho();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        private void AlterarEmissorHandler(object sender, EventArgs e)
        {
            FlyoutAlteraEmissorModel = new FlyoutAlteraEmissorModel(_sessaoManager, _nfe) { IsOpen = true };
            FlyoutAlteraEmissorModel.EmissorAlterado += EmissorAlteradoHandler;
            FlyoutAlteraEmissorModel.Inicializar();
        }

        private void EmissorAlteradoHandler(object sender, Nfeletronica e)
        {
            AbaCabecalhoModel.NumeroDocumento = e.NumeroDocumento;
            AbaCabecalhoModel.SerieDocumento = e.SerieEmissao;
            AbaCabecalhoModel.PreecherCom(e.Emitente.CarregarDadosEmissor(_sessaoManager));

            DialogBox.MostraInformacao("Tudo certo o emissor foi alterado");
        }

        private void AlterarNumeroHandler(object sender, EventArgs e)
        {
            var reservador = new ReservadorNumeroNfe(_sessaoManager);
            reservador.ReservarNumero(_nfe, _nfe.Emitente.CarregarDadosEmissor(_sessaoManager));

            AbaCabecalhoModel.PreecherCom(_nfe);
            DialogBox.MostraInformacao($"Pronto! Reservei o número {_nfe.NumeroDocumento}");
        }

        public void InicializarParaEdicao(NfeletronicaGrid nfe)
        {
            try
            {
                bool emissaoPendente;

                using (var sessao = _sessaoManager.CriaSessao())
                {
                    var repositorioPessoa = new RepositorioPessoa(sessao);
                    var repositorioNfe = new RepositorioNfe(sessao);

                    _nfe = repositorioNfe.GetPeloId(nfe.Id);
                    var cliente = repositorioPessoa.GetClientePeloId(_nfe.Destinatario.GetPessoaId());

                    if (_nfe.Destinatario.SolicitaPedido != cliente.SolicitaPedidoNfe)
                    {
                        _nfe.Destinatario.SolicitaPedido = cliente.SolicitaPedidoNfe;
                        repositorioNfe.SalvarAlteracoes(_nfe);
                    }

                    emissaoPendente = repositorioNfe.PossuiEmissaoPendente(_nfe);
                }

                AbreAbaItens();

                if (!emissaoPendente)
                {
                    return;
                }

                Application.Current.BeginInvoke(() => { AbreJanelaEmissaoSefaz(_nfe); });
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void ReferenciarCfHandler(object sender, EventArgs e)
        {
            FlyoutReferenciaCfModel = new FlyoutReferenciaCfModel(_nfe, _sessaoManager) { IsOpen = true };
        }

        private void AbreAbaCabecalho()
        {
            if (IsNotaNova)
            {
                AbaCabecalhoModel.PreecherCom(_abaPerfil);
                AbaCabecalhoModel.PreecherCom(_emissorDoPerfil);
            }
            else
            {
                AbaCabecalhoModel.PreecherCom(_nfe);
            }

            AbaCabecalhoModel.IsNovo = IsNotaNova;
            AbaCabecalhoModel.Selecionado = true;
        }

        private void AbreAbaDestinatario()
        {
            AbaDestinoModel.UsarNfe(_nfe);

            if (IsNotaNova && _clienteDoPerfil != null)
            {
                AbaDestinoModel.Com(_clienteDoPerfil);
            }

            if (IsNotaNova && _transportadoraDoPerfil != null)
            {
                AbaDestinoModel.Com(_transportadoraDoPerfil);
            }

            if (IsNotaNova && _veiculoDoPerfil != null)
            {
                AbaDestinoModel.Com(_veiculoDoPerfil);
            }

            if (!IsNotaNova)
            {
                AbaDestinoModel.Com(_nfe);
            }

            AbaDestinoModel.Selecionado = true;
        }

        private void AbreAbaItens()
        {
            AbaItensModel.Preparar(_nfe);
            AbaItensModel.Selecionado = true;
        }

        private void AbaCabecalhoProximoPassoHandler(object sender, EventArgs e)
        {
            SetarDadosDoCabecalhoNaNota();
            AbreAbaDestinatario();
        }

        private void SetarDadosDoCabecalhoNaNota()
        {
            _nfe.TipoOperacao = AbaCabecalhoModel.TipoOperacao;
            _nfe.NaturezaOperacao = AbaCabecalhoModel.NaturezaOperacao;
            _nfe.FinalidadeEmissao = AbaCabecalhoModel.FinalidadeEmissao;
            _nfe.ModalidadeFrete = AbaCabecalhoModel.ModalidadeFrete;
            _nfe.EmitidaEm = AbaCabecalhoModel.EmitidaEm;
            _nfe.SaidaEm = AbaCabecalhoModel.SaidaEm;
            _nfe.InformacaoAdicional = AbaCabecalhoModel.InformacaoAdicional;
            _nfe.IncluirInformacaoIbpt = AbaCabecalhoModel.IncluirInformacaoIbpt;

            if (_nfe.Id > 0)
            {
                SalvarNota();
            }
        }

        private void AbaDestinoProximoPassoHandler(object sender, AbaDestintarioEventArgs e)
        {
            try
            {
                SetarDadosDoDestinatarioNaNota(e.Destinatario);
                SetarDadosDaTransportadoraNaNota(e.Transportadora);
                SetarDadosDosVolumesNaNota(e.VolumesModel);
                SetarDadosExportacaoNaNfe(e.Exportacao);
                SalvarNota();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
                return;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message);
                return;
            }

            AbreAbaItens();
        }

        private void SetarDadosDoDestinatarioNaNota(DestinatarioModel destinatario)
        {
            _nfe.LocalEntrega = null;

            if (_nfe.Destinatario == null)
            {
                _nfe.Destinatario = new DestinatarioNfe(_nfe, destinatario.GetEnderecoFiscal());
            }

            if (destinatario.LocalEntregaSelecionada != null)
            {
                _nfe.LocalEntrega = new LocalEntrega
                {
                    Endereco = destinatario.LocalEntregaSelecionada,
                    Nfe = _nfe
                };
            }

            _nfe.Destinatario.Nome = destinatario.Nome;
            _nfe.Destinatario.Endereco = destinatario.GetEnderecoFiscal();
            _nfe.Destinatario.DocumentoUnico = destinatario.DocumentoUnico;
            _nfe.Destinatario.InscricaoEstadual = destinatario.InscricaoEstadual;
            _nfe.Destinatario.IndicadorIE = destinatario.IndicadorIE;
            _nfe.Destinatario.ReferenciaUmaPessoaId(destinatario.GetPessoaId());
            _nfe.Destinatario.SolicitaPedido = destinatario.SolicitaPedido;
            _nfe.Destinatario.IndicadorOperacaoFinal = destinatario.IndicadorOperacaoFinal;
            _nfe.Destinatario.IndicadorPresenca = destinatario.IndicadorPresencaComprador;
            _nfe.Destinatario.IndicadorDestinoOperacao = destinatario.IndicadorDestinoOperacao;
        }

        private void SetarDadosDaTransportadoraNaNota(TransportadoraModel transportadora)
        {
            transportadora.Validar();

            if (transportadora.TemTransportadora == false && transportadora.TemVeiculo == false)
            {
                _nfe.Transportadora = null;
                return;
            }

            if (_nfe.Transportadora == null)
            {
                _nfe.Transportadora = new TransportadoraNfe(_nfe, transportadora.DocumentoUnico, transportadora.Nome);
            }

            _nfe.Transportadora.DocumentoUnico = transportadora.DocumentoUnico ?? string.Empty;
            _nfe.Transportadora.EnderecoCompleto = transportadora.Endereco ?? string.Empty;
            _nfe.Transportadora.Nome = transportadora.Nome ?? string.Empty;
            _nfe.Transportadora.SiglaUf = transportadora.SiglaEstado ?? string.Empty;
            _nfe.Transportadora.NomeMunicipio = transportadora.Cidade?.Nome ?? string.Empty;
            _nfe.Transportadora.Veiculo = transportadora.GetVeiculoFiscal();
            _nfe.Transportadora.InscricaoEstadual = transportadora.InscricaoEstadual ?? string.Empty;
            _nfe.Transportadora.ReferenciaUmaPessoaId(transportadora.GetPessoaId());
        }

        private void SetarDadosDosVolumesNaNota(VolumesModel volume)
        {
            if (volume.NaoTemVolume())
            {
                _nfe.Volumes = null;
                return;
            }

            _nfe.Volumes = volume.Volumes;
        }

        private void SetarDadosExportacaoNaNfe(ExportacaoModel e)
        {
            if (!_nfe.Destinatario.ResideExterior())
            {
                _nfe.Exportacao = null;
                return;
            }

            if (e.NenhumCampoExportacaoInformado())
            {
                _nfe.Destinatario.Endereco.Localizacao.SetLocalidade(e.ObterPaisDestino());
                _nfe.Exportacao = null;
                return;
            }

            if (_nfe.Exportacao == null)
            {
                _nfe.Exportacao = new ExportacaoNfe(_nfe, e.UfSaidaPais, e.LocalEmbarque, e.LocalDespacho);
            }

            _nfe.Exportacao.LocalEmbarque = e.LocalEmbarque;
            _nfe.Exportacao.LocalDespacho = e.LocalDespacho;
            _nfe.Exportacao.UfSaidaPais = e.UfSaidaPais;
            _nfe.Destinatario.Endereco.Localizacao.SetLocalidade(e.ObterPaisDestino());
        }

        private void SalvarNota()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                new RepositorioNfe(sessao).SalvarAlteracoes(_nfe);

                transacao.Commit();
            }
        }

        private void AbaDestinoPassoAnteriorHandler(object sender, AbaDestintarioEventArgs e)
        {
            AbreAbaCabecalho();
        }

        private void AbaItensPassoAnteriorHandler(object sender, EventArgs e)
        {
            AbreAbaDestinatario();
        }

        private void EmiteNfeHandler(object sender, EventArgs e)
        {
            try
            {
                if (_nfe.Itens.Any() == false)
                {
                    throw new InvalidOperationException("Não posso emitir uma NF-e sem itens.");
                }

                if (_nfe.Itens.Any(x => x.Produto.Ativo == false))
                {
                    throw new InvalidOperationException("Não posso emitir uma NF-e com produtos inativos.\nProdutos com linhas em vermelho.");
                }

                if (_nfe.TotalFinal <= 0 && _nfe.FinalidadeEmissao == FinalidadeEmissao.Normal)
                {
                    throw new InvalidOperationException($"Não posso emitir uma NF-e com este valor: {_nfe.TotalFinal}");
                }

                if (!_nfe.PossuiPagamento())
                {
                    _nfe.PagarEmDinheiro(_usuario);
                    SalvarNota();
                }

                if (!_nfe.IsPago())
                {
                    throw new PagamentoNfeException($"Pagamento diverge do valor total da nota, preciso que ajuste-o");
                }

                AbreJanelaEmissaoSefaz(_nfe);

                if (_closeRequestInvoked)
                {
                    return;
                }

                using (var sessao = _sessaoManager.CriaSessao())
                {
                    var repositorio = new RepositorioNfe(sessao);
                    repositorio.Refresh(_nfe);

                    AbaItensModel.Nfe = _nfe;
                    AbaItensModel.AtualizaDadosView();
                }
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnCloseRequest()
        {
            _closeRequestInvoked = true;
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        private void ReferenciasNfeHandler(object sender, EventArgs e)
        {
            FlyoutReferenciaNfeModel = new FlyoutReferenciaNfeModel(_nfe, _sessaoManager) { IsOpen = true };
        }

        private void AbreJanelaEmissaoSefaz(Nfeletronica nfe)
        {
            var viewModel = new EmissaoSefazViewModel(nfe, _sessaoManager);
            viewModel.EmissaoAutorizada += EmissaoAutorizadaHandler;
            viewModel.EmissaoPendente += EmissaoPendenteHandler;

            new EmissaoSefazView(viewModel).ShowDialog();
        }

        private void EmissaoAutorizadaHandler(object sender, EmissaoFinalizadaNfe e)
        {
            if (e.IsDenegado)
            {
                DialogBox.MostraInformacao("(NF-E DENEGADA): NF-E foi autorizada, porém a SEFAZ denegou a mesma.");
            }

            new NfeletronicaOpcoes(e.Nfe).ShowDialog();
            OnCloseRequest();
        }

        private void EmissaoPendenteHandler(object sender, EmissaoNfe e)
        {
            DialogBox.MostraInformacao(
                "Existe uma emissão pedente, a nota ficará aguardando nova tentaiva de envio.");
            OnCloseRequest();
        }
    }
}