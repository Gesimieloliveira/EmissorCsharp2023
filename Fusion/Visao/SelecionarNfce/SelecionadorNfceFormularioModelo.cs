using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Fusion.Sessao;
using Fusion.Visao.ConverterVendaParaNfe;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.ConverterVendaParaNfe;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.SelecionarNfce;
using FusionCore.Sessao;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.SelecionarNfce
{
    public class SelecionadorNfceFormularioModelo : ViewModel
    {
        private ObservableCollection<NfceDto> _nfcesDtos = new ObservableCollection<NfceDto>();
        private ObservableCollection<NfceSelecionadaDto> _nfcesSelecionadas = new ObservableCollection<NfceSelecionadaDto>();
        private NfceDto _nfceDtoSelecionado;
        private NfceSelecionadaDto _nfceSelecionada;
        private decimal _valorTotalDocumentos;
        private FiltroConversorNfce _filtroConversorNfce = new FiltroConversorNfce();
        private ClienteDto _clienteDtoNfce;
        private Cliente _cliente;

        public NfceDto NfceDtoSelecionado
        {
            get => _nfceDtoSelecionado;
            set
            {
                _nfceDtoSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public NfceSelecionadaDto NfceSelecionada
        {
            get => _nfceSelecionada;
            set
            {
                _nfceSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorTotalDocumentos
        {
            get => _valorTotalDocumentos;
            set
            {
                _valorTotalDocumentos = value;
                PropriedadeAlterada();
            }
        }

        public FiltroConversorNfce FiltroConversorNfce
        {
            get => _filtroConversorNfce;
            set
            {
                _filtroConversorNfce = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<NfceDto> NfcesDtos
        {
            get => _nfcesDtos;
            set
            {
                _nfcesDtos = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<NfceSelecionadaDto> NfcesSelecionadas
        {
            get => _nfcesSelecionadas;
            set
            {
                _nfcesSelecionadas = value;
                PropriedadeAlterada();
            }
        }

        public ICommand ComandoAplicarFiltroABusca => GetSimpleCommand(AplicarFiltroABuscaAcao);

        public ICommand ComandoInicializacaoConvercao => GetSimpleCommand(InicializacaoConvercaoAcao);

        public void Inicializar()
        {
            AplicarFiltros();
        }

        private void InicializacaoConvercaoAcao(object obj)
        {
            try
            {
                var cliente = ObterClienteParaNfe();

                var modelo = new ConverterParaNfeFormModel(new ConverterNfcesParaNfe(
                    NfcesSelecionadas,
                    new SessaoManagerAdm(),
                    SessaoSistema.ObterUsuarioLogado(),
                    cliente));

                new ConverterParaNfeForm(modelo).ShowDialog();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
                return;
            }
            OnFechar();
        }

        public void SelecionarNfceParaConversao()
        {
            if (NfcesSelecionadas.Any(x => x.Id == NfceDtoSelecionado.Id
                && x.Serie == NfceDtoSelecionado.Serie && x.NumeroFiscal == NfceDtoSelecionado.NumeroFiscal))
                throw new InvalidOperationException("Está nfc-e já foi selecionada");


            if (NfcesSelecionadas.Any(nfceSelecionadaDto => nfceSelecionadaDto.Emitente.Id != NfceDtoSelecionado.IdEmitente))
            {
                throw new InvalidOperationException("Não é permitido selecionar nfc-e de diferentes empresas emissoras");
            }

            if (NfcesSelecionadas.Any(nfceSelecionadaDto =>
                nfceSelecionadaDto.Emitente.RegimeTributario != NfceDtoSelecionado.RegimeTributario))
            {
                throw new InvalidOperationException("Não é permitido selecionar nfc-e de diferentes regimes tributários");
            }

            NfcesSelecionadas.Add(NfceSelecionadaDto.Com(NfceDtoSelecionado));
            AtualizaTotalNfces();
        }
        private void AplicarFiltroABuscaAcao(object obj)
        {
            try
            {
                FiltroConversorNfce.FiltroNomeDoClienteContenha =
                    FiltroConversorNfce.FiltroNomeDoClienteContenha.TrimOrEmpty();

                FiltroConversorNfce.DataInicialNaoPodeSerMaiorQueFinal();

                AplicarFiltros();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }
        private void AplicarFiltros()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var listaDeNfces = new RepositorioNfceAdm(sessao).BuscaNfceParaConversao(FiltroConversorNfce);
                var cuponsNfces = new RepositorioCupomFiscal(sessao).BuscaNfceParaConversao(FiltroConversorNfce);

                cuponsNfces.ForEach(listaDeNfces.Add);

                NfcesDtos = new ObservableCollection<NfceDto>(listaDeNfces);
            }
        }
        public void RemoverNfceSelecionadaDaConversao()
        {
            NfcesSelecionadas.Remove(NfceSelecionada);
            AtualizaTotalNfces();
        }
        private void AtualizaTotalNfces()
        {
            ValorTotalDocumentos = NfcesSelecionadas.Sum(x => x.TotalFiscal);
        }
        private ClienteDto ObterClienteParaNfe()
        {
            _clienteDtoNfce = null;
            _cliente = null;

            var listaDeClientesDisponiveis = NfcesSelecionadas.Where(x => x.Cliente != null)
                .Select(x => x.Cliente).ToList();

            if (listaDeClientesDisponiveis.Count != 0)
            {
                var modelo = new SelecionarUmClienteParaNfeFormularioModel();
                modelo.CarregarClientes(listaDeClientesDisponiveis);
                modelo.ClienteSelecionadoComSucesso += delegate (object sender, ClienteDto cliente)
                {
                    _clienteDtoNfce = cliente;

                    if (cliente == null)
                    {
                        SelecionarClienteManual();
                    }
                };
                new SelecionarUmClienteParaNfeFormulario(modelo).ShowDialog();
            }

            if (_clienteDtoNfce == null)
            {
                SelecionarClienteManual();
            }

            if (_clienteDtoNfce == null && _cliente == null)
            {
                throw new InvalidOperationException("Não foi selecionado nem um cliente.");
            }

            return _clienteDtoNfce ?? new ClienteDto(_cliente.Nome, _cliente.Id);
        }
        private void SelecionarClienteManual()
        {
            var modelo = new PessoaPickerModel(new ClienteEngine());
            modelo.PickItemEvent += delegate (object sender, GridPickerEventArgs args) { _cliente = args.GetItem<Cliente>(); };

            modelo.GetPickerView().ShowDialog();
        }
    }
}