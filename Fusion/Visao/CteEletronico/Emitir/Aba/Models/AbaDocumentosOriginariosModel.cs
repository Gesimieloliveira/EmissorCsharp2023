using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fusion.Visao.CteEletronico.Emitir.EntidadesModels;
using Fusion.Visao.CteEletronico.Emitir.EntidadesModels.DocAnt;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace Fusion.Visao.CteEletronico.Emitir.Aba.Models
{
    public class RetornaDocumentos
    {
        public AbaDocumentosOriginariosModel AbaDocumentosOriginariosModel { get; set; }

        public RetornaDocumentos(AbaDocumentosOriginariosModel abaDocumentosOriginariosModel)
        {
            AbaDocumentosOriginariosModel = abaDocumentosOriginariosModel;
        }
    }

    public sealed class AbaDocumentosOriginariosModel : ViewModel
    {
        private readonly CteFacade _cteFacade;
        private bool _selecionado;
        private ObservableCollection<GridDocumentoNfeModel> _listaDocumentoNfe;
        private GridDocumentoNfeModel _itemSelecionadoDocumentoNfe;
        private ObservableCollection<GridDocumentoNotaFiscalImpressaModel> _listaDocumentoImpressos;
        private GridDocumentoNotaFiscalImpressaModel _itemSelecionadoDocumentoImpresso;
        private ObservableCollection<GridOutroDocumentoModel> _listaDocumentoOutroDocumento;
        private GridOutroDocumentoModel _itemSelecionadoDocumentoOutroDocumento;
        private bool _habilitado;
        private decimal _totalCarga;
        private bool _calcularTotalCargaAutomatico;
        private bool _ativarTotalCargaAutomatico;
        private ObservableCollection<GridDocumentoAnterior> _listaDocumentoAnterior;
        private GridDocumentoAnterior _itemSelecionadoDocumentoAnterior;
        private bool _isSubcontratacao;
        private ObservableCollection<GridComponenteValorPrestacao> _listaComponenteValorPrestacao;
        private GridComponenteValorPrestacao _itemSelecionadoComponenteValorPrestacao;
        private decimal _valorTotalComponentes;
        private bool _isNaoEComplementar;


        public decimal ValorTotalComponentes
        {
            get => _valorTotalComponentes;
            set
            {
                if (value == _valorTotalComponentes) return;
                _valorTotalComponentes = value;
                PropriedadeAlterada();
            }
        }

        public bool AtivarTotalCargaAutomatico
        {
            get => _ativarTotalCargaAutomatico;
            set
            {
                if (value == _ativarTotalCargaAutomatico) return;
                _ativarTotalCargaAutomatico = value;
                PropriedadeAlterada();
            }
        }

        public bool CalcularTotalCargaAutomatico
        {
            get => _calcularTotalCargaAutomatico;
            set
            {
                if (value == _calcularTotalCargaAutomatico) return;
                _calcularTotalCargaAutomatico = value;

                AtivarTotalCargaAutomatico = true;

                if (value)
                    AtivarTotalCargaAutomatico = false;

                PropriedadeAlterada();
            }
        }

        public bool Habilitado
        {
            get => _habilitado;
            set
            {
                if (value == _habilitado) return;
                _habilitado = value;
                PropriedadeAlterada();
            }
        }

        public GridDocumentoNfeModel ItemSelecionadoDocumentoNfe
        {
            get => _itemSelecionadoDocumentoNfe;
            set
            {
                if (Equals(value, _itemSelecionadoDocumentoNfe)) return;
                _itemSelecionadoDocumentoNfe = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridDocumentoNfeModel> ListaDocumentoNfe
        {
            get => _listaDocumentoNfe;
            set
            {
                if (Equals(value, _listaDocumentoNfe)) return;
                _listaDocumentoNfe = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridDocumentoNotaFiscalImpressaModel> ListaDocumentoImpressos
        {
            get => _listaDocumentoImpressos;
            set
            {
                if (Equals(value, _listaDocumentoImpressos)) return;
                _listaDocumentoImpressos = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridOutroDocumentoModel> ListaDocumentoOutroDocumento
        {
            get => _listaDocumentoOutroDocumento;
            set
            {
                if (Equals(value, _listaDocumentoOutroDocumento)) return;
                _listaDocumentoOutroDocumento = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridDocumentoAnterior> ListaDocumentoAnterior
        {
            get => _listaDocumentoAnterior;
            set
            {
                if (Equals(value, _listaDocumentoAnterior)) return;
                _listaDocumentoAnterior = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridComponenteValorPrestacao> ListaComponenteValorPrestacao
        {
            get => _listaComponenteValorPrestacao;
            set
            {
                if (Equals(value, _listaComponenteValorPrestacao)) return;
                _listaComponenteValorPrestacao = value;
                PropriedadeAlterada();
            }
        }

        public bool Selecionado
        {
            get => _selecionado;
            set
            {
                if (value == _selecionado) return;
                _selecionado = value;
                PropriedadeAlterada();
            }
        }

        public GridDocumentoNotaFiscalImpressaModel ItemSelecionadoDocumentoImpresso
        {
            get => _itemSelecionadoDocumentoImpresso;
            set
            {
                if (Equals(value, _itemSelecionadoDocumentoImpresso)) return;
                _itemSelecionadoDocumentoImpresso = value;
                PropriedadeAlterada();
            }
        }

        public decimal TotalCarga
        {
            get => _totalCarga;
            set
            {
                if (value == _totalCarga) return;
                _totalCarga = value;
                PropriedadeAlterada();
            }
        }

        public GridOutroDocumentoModel ItemSelecionadoDocumentoOutroDocumento
        {
            get => _itemSelecionadoDocumentoOutroDocumento;
            set
            {
                if (Equals(value, _itemSelecionadoDocumentoOutroDocumento)) return;
                _itemSelecionadoDocumentoOutroDocumento = value;
                PropriedadeAlterada();
            }
        }

        public GridDocumentoAnterior ItemSelecionadoDocumentoAnterior
        {
            get => _itemSelecionadoDocumentoAnterior;
            set
            {
                if (Equals(value, _itemSelecionadoDocumentoAnterior)) return;
                _itemSelecionadoDocumentoAnterior = value;
                PropriedadeAlterada();
            }
        }

        public GridComponenteValorPrestacao ItemSelecionadoComponenteValorPrestacao
        {
            get => _itemSelecionadoComponenteValorPrestacao;
            set
            {
                if (Equals(value, _itemSelecionadoComponenteValorPrestacao)) return;
                _itemSelecionadoComponenteValorPrestacao = value;
                PropriedadeAlterada();
            }
        }

        public bool IsSubcontratacao
        {
            get => _isSubcontratacao;
            set
            {
                if (value == _isSubcontratacao) return;
                _isSubcontratacao = value;
                PropriedadeAlterada();
            }
        }

        public AbaCabecalhoCteModel Cabecalho
        {
            get => GetValue<AbaCabecalhoCteModel>();
            set => SetValue(value);
        }

        public bool IsNaoEComplementar
        {
            get => _isNaoEComplementar;
            set
            {
                _isNaoEComplementar = value;
                PropriedadeAlterada();
            }
        }

        public AbaDocumentosOriginariosModel(CteFacade cteFacade, AbaCabecalhoCteModel cabecalho)
        {
            _cteFacade = cteFacade;
            Cabecalho = cabecalho;

            Inicializa();
        }

        public event EventHandler<RetornaDocumentos> ProximoPasso;
        public event EventHandler PassoAnterior;
        public event EventHandler AdicionaNfeCall;
        public event EventHandler AdicionaNfImpressaCall;
        public event EventHandler AdicionaNfOutroDocumentoCall;
        public event EventHandler AdicionaDocumentoAnteriorCall;
        public event EventHandler AdicionaComponenteValorPrestacaoCall;

        public void Anterior()
        {
            OnPassoAnterior();
        }

        public void OnProximoPasso()
        {
            ProximoPasso?.Invoke(this, new RetornaDocumentos(this));
        }

        private void OnPassoAnterior()
        {
            PassoAnterior?.Invoke(this, EventArgs.Empty);
        }

        public void OnAdicionaNfeCall()
        {
            AdicionaNfeCall?.Invoke(this, EventArgs.Empty);
        }

        public void OnAdicionaNfImpressaCall()
        {
            AdicionaNfImpressaCall?.Invoke(this, EventArgs.Empty);
        }

        public void OnAdicionaNfOutroDocumentoCall()
        {
            AdicionaNfOutroDocumentoCall?.Invoke(this, EventArgs.Empty);
        }

        public void AdicionarDocumentoNfe(GridDocumentoNfeModel model)
        {
            ListaDocumentoNfe.Add(model);
            AtualizaTotalCarga();
        }

        public void OnAdicionaDocumentoAnteriorCall()
        {
            AdicionaDocumentoAnteriorCall?.Invoke(this, EventArgs.Empty);
        }

        public void DeletaDocumentoNfeSelecionada()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _cteFacade.DeletarDocumentoNfe(sessao, ItemSelecionadoDocumentoNfe.DocumentoNfe);

                transacao.Commit();
            }

            ListaDocumentoNfe.Remove(ItemSelecionadoDocumentoNfe);
            AtualizaTotalCarga();
        }

        public void AdicionarDocumentoNotaFiscalImpressa(GridDocumentoNotaFiscalImpressaModel model)
        {
            ListaDocumentoImpressos.Add(model);
            AtualizaTotalCarga();
        }

        public void DeletaDocumentoImpressoSelecionado()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _cteFacade.DeletarDocumentoImpresso(sessao, ItemSelecionadoDocumentoImpresso.DocumentoImpresso);

                transacao.Commit();
            }

            ListaDocumentoImpressos.Remove(ItemSelecionadoDocumentoImpresso);
            AtualizaTotalCarga();
        }

        public void AdicionarDocumentoOutroDocumento(GridOutroDocumentoModel model)
        {
            ListaDocumentoOutroDocumento.Add(model);
            AtualizaTotalCarga();
        }

        public void AdicionarDocumentoAnterior(GridDocumentoAnterior gridDocumentoAnterior)
        {
            ListaDocumentoAnterior.Add(gridDocumentoAnterior);
            AtualizaTotalCarga();
        }

        public void DeletaDocumentoOutroDocumentoSelecionado()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _cteFacade.DeletarDocumentoOutro(sessao, ItemSelecionadoDocumentoOutroDocumento.DocumentoOutro);

                transacao.Commit();
            }

            ListaDocumentoOutroDocumento.Remove(ItemSelecionadoDocumentoOutroDocumento);
            AtualizaTotalCarga();
        }

        public void DeletaDocumentoAnteriorSelecionado()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _cteFacade.DeletarDocumentoAnterior(sessao, ItemSelecionadoDocumentoAnterior.CteDocumentoAnterior);

                transacao.Commit();
            }

            ListaDocumentoAnterior.Remove(ItemSelecionadoDocumentoAnterior);
            AtualizaTotalCarga();
        }

        public void AtualizaTotalCarga()
        {
            if (CalcularTotalCargaAutomatico == false)
            {
                return;
            }

            var totalNfe = ListaDocumentoNfe.Sum(nfe => nfe.TotalNFe);
            var totalNfImpressa = ListaDocumentoImpressos.Sum(di => di.ValorTotalNf);
            var totalOutros = ListaDocumentoOutroDocumento.Sum(o => o.ValorTotal);
            TotalCarga = totalNfe + totalNfImpressa + totalOutros;
            TotalCarga = TotalCarga.Format("N2");
        }

        public void PreencerCom(Cte cte)
        {
            CarregaListaNfe(cte);
            CarregaListaDocumentoImpressos(cte);
            CarregaListaDocumentoOutros(cte);
            CarregaListaDocumentoAnteriores(cte);
            CarregaListaComponente(cte);
            TotalCarga = cte.ValorTotalCarga;
            CalcularTotalCargaAutomatico = cte.CalcularTotalCargaAutomatico;

            IsSubcontratacao = cte.TipoServico == TipoServico.Subcontratacao || cte.CteDocumentoAnteriores.Count != 0;

            if (cte.TipoServico == TipoServico.Normal && cte.CteDocumentoAnteriores.Count == 0)
            {
                IsSubcontratacao = false;
            }
        }

        private void CarregaListaComponente(Cte cte)
        {
            cte.CteComponenteValorPrestacaos.ForEach(comp => AdicionarComponente(GridComponenteValorPrestacao.Cria(comp)));
        }

        private void CarregaListaNfe(Cte cte)
        {
            cte.CteDocumentoNfes.ForEach(nfe => { AdicionarDocumentoNfe(GridDocumentoNfeModel.Cria(nfe)); });
        }

        private void CarregaListaDocumentoImpressos(Cte cte)
        {
            cte.CteDocumentoImpressos.ForEach(
                di => { AdicionarDocumentoNotaFiscalImpressa(GridDocumentoNotaFiscalImpressaModel.Cria(di)); });
        }

        private void CarregaListaDocumentoOutros(Cte cte)
        {
            cte.CteDocumentoOutros.ForEach(
                outro => { AdicionarDocumentoOutroDocumento(GridOutroDocumentoModel.Cria(outro)); });
        }

        private void CarregaListaDocumentoAnteriores(Cte cte)
        {
            cte.CteDocumentoAnteriores.ForEach(ant =>
            {
                AdicionarDocumentoAnterior(GridDocumentoAnterior.Cria(ant));
            });
        }

        private void Inicializa()
        {
            if (ListaDocumentoNfe == null)
                ListaDocumentoNfe = new ObservableCollection<GridDocumentoNfeModel>();

            if (ListaDocumentoImpressos == null)
                ListaDocumentoImpressos = new ObservableCollection<GridDocumentoNotaFiscalImpressaModel>();

            if (ListaDocumentoOutroDocumento == null)
                ListaDocumentoOutroDocumento = new ObservableCollection<GridOutroDocumentoModel>();

            if (ListaDocumentoAnterior == null) 
                ListaDocumentoAnterior = new ObservableCollection<GridDocumentoAnterior>();

            if (ListaComponenteValorPrestacao == null)
                ListaComponenteValorPrestacao = new ObservableCollection<GridComponenteValorPrestacao>();
        }

        public void VerificaSeChaveExisteJa(string chaveNfe)
        {
            ListaDocumentoNfe.ForEach(nfe =>
            {
                if (nfe.ChaveNfe.Equals(chaveNfe))
                    throw new InvalidOperationException("Chave de NF-e já existe na lista");
            });
        }

        public bool TodosDocumentosVazios()
        {
            return ListaDocumentoImpressos.Count == 0 && ListaDocumentoNfe.Count == 0 &&
                   ListaDocumentoOutroDocumento.Count == 0;
        }

        public void OnAdicionaComponenteValorPrestacaoCall()
        {
            AdicionaComponenteValorPrestacaoCall?.Invoke(this, EventArgs.Empty);
        }

        public void DeletaComponenteValorPrestacaoSelecionado()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _cteFacade.DeletarComponenteValorPrestacao(sessao, ItemSelecionadoComponenteValorPrestacao.Componente);

                transacao.Commit();
            }

            ListaComponenteValorPrestacao.Remove(ItemSelecionadoComponenteValorPrestacao);
            AtualizaTotalCompoentes();
        }

        public void AdicionarComponente(GridComponenteValorPrestacao gridComponenteValorPrestacao)
        {
            ListaComponenteValorPrestacao.Add(gridComponenteValorPrestacao);
            AtualizaTotalCompoentes();
        }

        private void AtualizaTotalCompoentes()
        {
            ValorTotalComponentes = ListaComponenteValorPrestacao.Sum(comp => comp.Componente.Valor);
        }
    }
}