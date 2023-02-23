using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Fusion.Visao.MdfeEletronico.Aba.Entidades;
using Fusion.Visao.MdfeEletronico.Aba.IncluirPagamento;
using Fusion.Visao.Produto;
using FusionCore.Core.Estoque;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Autorizador;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace Fusion.Visao.MdfeEletronico.Aba.Model
{
    public class EmitirMdfeEventArgs : EventArgs
    {
        public EmitirMdfeEventArgs(AbaRodoviarioMdfeModel model)
        {
            Model = model;
        }

        public AbaRodoviarioMdfeModel Model { get; set; }
    }

    public class EditarInformacaoPagamento
    {
        private readonly MdfeAutorizacaoInformacaoPagamento _informacaoPagamento;
        public bool Editar { get; }

        public EditarInformacaoPagamento(MdfeAutorizacaoInformacaoPagamento informacaoPagamento,
            bool editar)
        {
            _informacaoPagamento = informacaoPagamento;
            Editar = editar;
        }

        public MdfeAutorizacaoInformacaoPagamento ObterInformacaoPagamento()
        {
            return Editar ? _informacaoPagamento : null;
        }
    }

    public class AbaRodoviarioMdfeModel : ViewModel
    {
        private bool _habilitado;
        private bool _selecionado;
        private ObservableCollection<GridVeiculoTracao> _listaVeiculoTracao;
        private GridVeiculoTracao _veiculoTracaoSelecionado;
        private ObservableCollection<GridCondutor> _listaCondutor;
        private GridCondutor _condutorSelecionado;
        private ObservableCollection<GridVeiculoReboque> _listaVeiculoReboque;
        private GridVeiculoReboque _veiculoReboqueSelecionado;
        private ObservableCollection<GirdValePedagio> _listaValePedagio;
        private GirdValePedagio _valePedagioSelecionado;
        private string _rntrc;
        private string _codigoAgendamentoPorto;
        private ObservableCollection<GridContratante> _listaContratante;
        private GridContratante _contratanteSelecionado;
        private ObservableCollection<GridCiot> _listaCiot;
        private GridCiot _ciotSelecionado;
        private string _nomeProdutoPredominante;
        private TipoCarga _tipoCarga = TipoCarga.Nenhuma;
        private string _codigoBarrasProdutoPredominante;
        private string _ncmProdutoPredominante;
        private ObservableCollection<MdfeAutorizacaoInformacaoPagamento> _listarPagamentos = new ObservableCollection<MdfeAutorizacaoInformacaoPagamento>();
        private MdfeAutorizacaoInformacaoPagamento _pagamentoSelecionado;
        private CategoriaComercialVeiculo _categoriaComercialVeiculo = CategoriaComercialVeiculo.VeiculoComercial2Eixos;

        public ICommand ComandoBuscaProduto => GetSimpleCommand(AcaoBuscaProduto);
        public ICommand ComandoLimpaProdutoPredominante => GetSimpleCommand(AcaoLimpaProdutoPredominante);

        private void AcaoLimpaProdutoPredominante(object obj)
        {
            NomeProdutoPredominante = string.Empty;
            NcmProdutoPredominante = string.Empty;
            CodigoBarrasProdutoPredominante = string.Empty;
        }

        private void AcaoBuscaProduto(object obj)
        {
            var pickerModel = new ProdutoGridPickerModel();

            pickerModel.PickItemEvent += (s, ev) =>
            {
                var produto = ev.GetItem<IProdutoSelecionado>().CarregaProduto();
                NomeProdutoPredominante = produto.Nome;
                NcmProdutoPredominante = produto.Ncm;
                CodigoBarrasProdutoPredominante =
                    produto.ProdutosAlias.FirstOrDefault(x => x.IsCodigoBarras)?.Alias ?? string.Empty;
            };

            pickerModel.GetPickerView().ShowDialog();
        }

        public CategoriaComercialVeiculo CategoriaComercialVeiculo
        {
            get => _categoriaComercialVeiculo;
            set
            {
                _categoriaComercialVeiculo = value;
                PropriedadeAlterada();
            }
        }

        public string Rntrc
        {
            get { return _rntrc; }
            set
            {
                if (value == _rntrc) return;
                _rntrc = value;
                PropriedadeAlterada();
            }
        }

        public string CodigoAgendamentoPorto
        {
            get { return _codigoAgendamentoPorto; }
            set
            {
                if (value == _codigoAgendamentoPorto) return;
                _codigoAgendamentoPorto = value;
                PropriedadeAlterada();
            }
        }


        public ObservableCollection<GridVeiculoTracao> ListaVeiculoTracao
        {
            get { return _listaVeiculoTracao; }
            set
            {
                if (Equals(value, _listaVeiculoTracao)) return;
                _listaVeiculoTracao = value;
                PropriedadeAlterada();
            }
        }

        public GridVeiculoTracao VeiculoTracaoSelecionado
        {
            get { return _veiculoTracaoSelecionado; }
            set
            {
                if (Equals(value, _veiculoTracaoSelecionado)) return;
                _veiculoTracaoSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public string NomeProdutoPredominante
        {
            get => _nomeProdutoPredominante;
            set
            {
                _nomeProdutoPredominante = value;
                PropriedadeAlterada();
            }
        }

        public string CodigoBarrasProdutoPredominante
        {
            get => _codigoBarrasProdutoPredominante;
            set
            {
                _codigoBarrasProdutoPredominante = value;
                PropriedadeAlterada();
            }
        }

        public string NcmProdutoPredominante
        {
            get => _ncmProdutoPredominante;
            set
            {
                if (value == _ncmProdutoPredominante) return;
                _ncmProdutoPredominante = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridCondutor> ListaCondutor
        {
            get { return _listaCondutor; }
            set
            {
                if (Equals(value, _listaCondutor)) return;
                _listaCondutor = value;
                PropriedadeAlterada();
            }
        }

        public GridCondutor CondutorSelecionado
        {
            get { return _condutorSelecionado; }
            set
            {
                if (Equals(value, _condutorSelecionado)) return;
                _condutorSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridVeiculoReboque> ListaVeiculoReboque
        {
            get { return _listaVeiculoReboque; }
            set
            {
                if (Equals(value, _listaVeiculoReboque)) return;
                _listaVeiculoReboque = value;
                PropriedadeAlterada();
            }
        }

        public GridVeiculoReboque VeiculoReboqueSelecionado
        {
            get { return _veiculoReboqueSelecionado; }
            set
            {
                if (Equals(value, _veiculoReboqueSelecionado)) return;
                _veiculoReboqueSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GirdValePedagio> ListaValePedagio
        {
            get { return _listaValePedagio; }
            set
            {
                if (Equals(value, _listaValePedagio)) return;
                _listaValePedagio = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridContratante> ListaContratante
        {
            get { return _listaContratante; }
            set
            {
                if (Equals(value, _listaContratante)) return;
                _listaContratante = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridCiot> ListaCiot
        {
            get { return _listaCiot; }
            set
            {
                if (Equals(value, _listaCiot)) return;
                _listaCiot = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<MdfeAutorizacaoInformacaoPagamento> ListarPagamentos
        {
            get => _listarPagamentos;
            set
            {
                if (Equals(value, _listarPagamentos)) return;
                _listarPagamentos = value;
                PropriedadeAlterada();
            }
        }

        public GridCiot CiotSelecionado
        {
            get { return _ciotSelecionado; }
            set
            {
                if (Equals(value, _ciotSelecionado)) return;
                _ciotSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public GridContratante ContratanteSelecionado
        {
            get { return _contratanteSelecionado; }
            set
            {
                if (Equals(value, _contratanteSelecionado)) return;
                _contratanteSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public MdfeAutorizacaoInformacaoPagamento PagamentoSelecionado
        {
            get => _pagamentoSelecionado;
            set
            {
                if (Equals(value, _pagamentoSelecionado)) return;
                _pagamentoSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public GirdValePedagio ValePedagioSelecionado
        {
            get { return _valePedagioSelecionado; }
            set
            {
                if (Equals(value, _valePedagioSelecionado)) return;
                _valePedagioSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public bool Habilitado
        {
            get { return _habilitado; }
            set
            {
                _habilitado = value;
                PropriedadeAlterada();
            }
        }

        public bool Selecionado
        {
            get { return _selecionado; }
            set
            {
                _selecionado = value;
                PropriedadeAlterada();
            }
        }

        public TipoCarga TipoCarga
        {
            get => _tipoCarga;
            set
            {
                _tipoCarga = value;
                PropriedadeAlterada();
            }
        }

        public AbaRodoviarioMdfeModel()
        {
            InicializaComponentes();
        }

        public event EventHandler AnteriorHandler;
        public event EventHandler AbrirFlyoutAddVeiculoTracaoHandler;
        public event EventHandler AbrirFlyoutAddCondutorHandler;
        public event EventHandler AbrirFlyoutAddVeiculoReboqueHandler;
        public event EventHandler AbrirFlyoutAddValePedagioHandler;
        public event EventHandler AbrirFlyoutAddContratanteHandler;
        public event EventHandler AbrirFlyoutAddCiotHandler;
        public event EventHandler<EditarInformacaoPagamento> AbrirDialogInformacaoPagamento;
        public event EventHandler<EmitirMdfeEventArgs> EmitirMdfe; 

        private void InicializaComponentes()
        {
            ListaVeiculoTracao = new ObservableCollection<GridVeiculoTracao>();
            ListaCondutor = new ObservableCollection<GridCondutor>();
            ListaVeiculoReboque = new ObservableCollection<GridVeiculoReboque>();
            ListaValePedagio = new ObservableCollection<GirdValePedagio>();
            ListaContratante = new ObservableCollection<GridContratante>();
            ListaCiot = new ObservableCollection<GridCiot>();
            CategoriaComercialVeiculo = CategoriaComercialVeiculo.VeiculoComercial2Eixos;
        }

        public void DeletarVeiculoReboqueSelecionado()
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                repositorio.DeletarVeiculoReboque(VeiculoReboqueSelecionado.MFDeVeiculoReboque);

                transacao.Commit();
            }

            ListaVeiculoReboque.Remove(VeiculoReboqueSelecionado);
        }

        public void DeletarCondutorSelecionado()
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);

                repositorioMdfe.DeletarCondutor(CondutorSelecionado.MDFeCondutor);

                transacao.Commit();
            }

            ListaCondutor.Remove(CondutorSelecionado);
        }

        public virtual void OnAnteriorHandler()
        {
            AnteriorHandler?.Invoke(this, EventArgs.Empty);
        }

        public void AbirFlyoutVeiculoTracao()
        {
            OnAbrirFlyoutAddVeiculoTracaoHandler();
        }

        public void AbrirFlyoutContratante()
        {
            OnAbrirFlyoutAddContratanteHandler();
        }

        protected virtual void OnAbrirFlyoutAddVeiculoTracaoHandler()
        {
            AbrirFlyoutAddVeiculoTracaoHandler?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAbrirFlyoutAddCondutorHandler()
        {
            AbrirFlyoutAddCondutorHandler?.Invoke(this, EventArgs.Empty);
        }

        public void FlyoutAddCondutor()
        {
            OnAbrirFlyoutAddCondutorHandler();
        }

        protected virtual void OnAbrirFlyoutAddVeiculoReboqueHandler()
        {
            AbrirFlyoutAddVeiculoReboqueHandler?.Invoke(this, EventArgs.Empty);
        }

        public void FlyoutAddReboqueMdfe()
        {
            OnAbrirFlyoutAddVeiculoReboqueHandler();
        }

        public void FlyoutAddValePedagio()
        {
            OnAbrirFlyoutAddValePedagioHandler();
        }

        protected virtual void OnAbrirFlyoutAddValePedagioHandler()
        {
            AbrirFlyoutAddValePedagioHandler?.Invoke(this, EventArgs.Empty);
        }

        public void FlyoutAddCiot()
        {
            OnAbrirFlyoutAddCiotHandler();
        }

        public void AdicionarVeiculoTracao(GridVeiculoTracao gridVeiculoTracao)
        {
            ListaVeiculoTracao.Add(gridVeiculoTracao);
        }

        public void DeletarVeiculoTracaoSelecionado()
        {

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);

                var condutores = repositorioMdfe.BuscarCondutoresPorVeiculoTracao(VeiculoTracaoSelecionado.MDFeVeiculoTracao);


                if (condutores.Count != 0)
                {
                    throw new ArgumentException("Para deletar o veículo tração você deve deletar todos os condutores");
                }

                repositorioMdfe.DeletarVeiculoTracao(VeiculoTracaoSelecionado.MDFeVeiculoTracao);

                transacao.Commit();
            }

            ListaVeiculoTracao.Remove(VeiculoTracaoSelecionado);
        }

        public void AdicionarCondutor(GridCondutor condutor)
        {
            ListaCondutor.Add(condutor);
        }

        public void AdicionarVeiculoReboque(GridVeiculoReboque gridVeiculoReboque)
        {
            ListaVeiculoReboque.Add(gridVeiculoReboque);
        }

        public void AdicionarValePedagio(GirdValePedagio girdValePedagio)
        {
            ListaValePedagio.Add(girdValePedagio);
        }

        public void AdicionarContratante(GridContratante contratante)
        {
            ListaContratante.Add(contratante);
        }

        public void AdicionarCiot(GridCiot ciot)
        {
            ListaCiot.Add(ciot);
        }

        public void AdicionarInformacaoPagamento(MdfeAutorizacaoInformacaoPagamento informacaoPagamento)
        {
            ListarPagamentos.Add(informacaoPagamento);
        }

        public void DeletaValePedagioSelecionado()
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                repositorio.DeletarValePedagio(ValePedagioSelecionado.MDFeValePedagio);
                
                transacao.Commit();
            }


            ListaValePedagio.Remove(ValePedagioSelecionado);
        }

        public void DeletarContratanteSelecionado()
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                repositorio.DeletarContratante(ContratanteSelecionado.Contratante);

                transacao.Commit();
            }

            ListaContratante.Remove(ContratanteSelecionado);
        }

        public void DeletarCiotSelecionado()
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                repositorio.DeletarCiot(CiotSelecionado.MDFeCiot);

                transacao.Commit();
            }

            ListaCiot.Remove(CiotSelecionado);
        }

        protected virtual void OnEmitirMdfe()
        {
            Hidrata();
            
            if (TipoCarga != TipoCarga.Nenhuma && NomeProdutoPredominante.IsNullOrEmpty())
                throw new InvalidOperationException("Adicione nome do produto predominante");

            if (NomeProdutoPredominante.IsNotNullOrEmpty() && TipoCarga == TipoCarga.Nenhuma)
                throw new InvalidOperationException("Adicione um tipo carga");


            EmitirMdfe?.Invoke(this, new EmitirMdfeEventArgs(this));
        }

        private void Hidrata()
        {
            Rntrc = Rntrc.TrimOrEmpty();
            CodigoAgendamentoPorto = CodigoAgendamentoPorto.TrimOrEmpty();
            NomeProdutoPredominante = NomeProdutoPredominante.TrimOrEmpty();
            CodigoBarrasProdutoPredominante = CodigoBarrasProdutoPredominante.TrimOrEmpty();
            NcmProdutoPredominante = NcmProdutoPredominante.TrimOrEmpty();
        }

        public void Inicializa(MDFeEletronico mdfe)
        {
            if (Rntrc.IsNullOrEmpty() || Rntrc.Equals("0"))
                Rntrc = mdfe.Emitente.Empresa.Rntrc.TrimOrEmpty();

            if (CodigoAgendamentoPorto.IsNullOrEmpty())
                CodigoAgendamentoPorto = string.Empty;
        }

        public void Emitir()
        {
            OnEmitirMdfe();
        }

        public void ComMdfe(MDFeEletronico mdfe)
        {
            if (mdfe.Rodoviario == null || mdfe.Rodoviario.MDFeId == 0) return;

            LoadRodoviario(mdfe);
            LoadVeiculoTracao(mdfe.Rodoviario.VeiculoTracao);
            LoadCondutores(mdfe.Rodoviario.VeiculoTracao);
            LoadVeiculoReboque(mdfe.Rodoviario.VeiculosReboques);
            LoadValesPedagio(mdfe.Rodoviario.ValesPedagios);
            LoadContratante(mdfe.Rodoviario.Contratantes);
            LoadCiots(mdfe.Rodoviario.Ciots);
            LoadInformacaoPagamento(mdfe.InformacaoPagamentos);

            TipoCarga = mdfe.ProdutoPredominante.TipoCarga;
            NomeProdutoPredominante = mdfe.ProdutoPredominante.Nome;
            NcmProdutoPredominante = mdfe.ProdutoPredominante.Ncm;
            CodigoBarrasProdutoPredominante = mdfe.ProdutoPredominante.CodigoBarras;
            CategoriaComercialVeiculo = mdfe.CategoriaComercialVeiculo;
        }

        private void LoadInformacaoPagamento(IList<MdfeAutorizacaoInformacaoPagamento> informacaoPagamentos)
        {
            informacaoPagamentos?.ForEach(AdicionarInformacaoPagamento);
        }

        private void LoadCiots(IEnumerable<MDFeCiot> ciots)
        {
            ciots?.ForEach(c =>
            {
                AdicionarCiot(new GridCiot
                {
                    DocumentoUnico = c.DocumentoUnico,
                    Ciot = c.Ciot,
                    MDFeCiot = c
                });
            });
        }

        private void LoadContratante(IEnumerable<MDFeContratante> rodoviarioContratantes)
        {
            rodoviarioContratantes?.ForEach(c =>
            {
                AdicionarContratante(new GridContratante
                {
                    DocumentoUnico = c.PessoaEntidade.GetDocumentoUnico(),
                    Nome = c.PessoaEntidade.Nome,
                    Contratante = c
                });
            });
        }

        private void LoadValesPedagio(IEnumerable<MDFeValePedagio> valesPedagios)
        {
            valesPedagios?.ForEach(v =>
            {
                ListaValePedagio.Add(new GirdValePedagio
                {
                    CnpjEmpresaFornecedora = v.CnpjEmpresaFornecedora,
                    MDFeValePedagio = v,
                    NumeroCompra = v.NumeroComprovante,
                    CnpjResponsavel = v.CnpjResponsavelPagamento,
                    Valor = v.Valor,
                    CpfResponsavel = v.CpfResponsavel
                });
            });
        }

        private void LoadVeiculoReboque(IEnumerable<MDFeVeiculoReboque> veiculosReboques)
        {
            veiculosReboques?.ForEach(v =>
            {
                ListaVeiculoReboque.Add(new GridVeiculoReboque
                {
                    Veiculo = v.Veiculo,
                    TipoCarroceria = v.Veiculo.TipoCarroceria,
                    TipoRodado = v.Veiculo.TipoRodado,
                    SiglaUf = v.Veiculo.SiglaUf,
                    TipoPropriedadeVeiculo = v.Veiculo.TipoProprietario,
                    Renavam = v.Veiculo.Renavam,
                    TipoVeiculo = v.Veiculo.TipoVeiculo,
                    CapacidadeEmM3 = v.Veiculo.CapacidadeEmM3,
                    CapacidadeEmKg = v.Veiculo.CapacidadeEmKg,
                    Placa = v.Veiculo.Placa,
                    CodigoInterno = v.Veiculo.Id.ToString(),
                    Tara = v.Veiculo.TaraEmKg,
                    MFDeVeiculoReboque = v
                });
            });
        }

        private void LoadCondutores(MDFeVeiculoTracao veiculoTracao)
        {
            veiculoTracao?.Condutores?.ForEach(c =>
            {
                ListaCondutor.Add(new GridCondutor
                {
                    Nome = c.Condutor.Nome,
                    Cpf = c.Condutor.Cpf.Valor,
                    MDFeCondutor = c,
                    Condutor = c.Condutor
                });
            });
        }

        private void LoadVeiculoTracao(MDFeVeiculoTracao veiculoTracao)
        {
            if (veiculoTracao == null) return;

            ListaVeiculoTracao.Add(new GridVeiculoTracao
            {
                Veiculo = veiculoTracao.Veiculo,
                TipoCarroceria = veiculoTracao.Veiculo.TipoCarroceria,
                SiglaUf = veiculoTracao.Veiculo.SiglaUf,
                TipoRodado = veiculoTracao.Veiculo.TipoRodado,
                MDFeVeiculoTracao = veiculoTracao,
                TipoPropriedadeVeiculo = veiculoTracao.Veiculo.TipoProprietario,
                Renavam = veiculoTracao.Veiculo.Renavam,
                TipoVeiculo = veiculoTracao.Veiculo.TipoVeiculo,
                CapacidadeEmM3 = veiculoTracao.Veiculo.CapacidadeEmM3,
                CapacidadeEmKg = veiculoTracao.Veiculo.CapacidadeEmKg,
                Placa = veiculoTracao.Veiculo.Placa,
                CodigoInterno = veiculoTracao.Veiculo.Id.ToString(),
                Tara = veiculoTracao.Veiculo.TaraEmKg
            });
        }

        private void LoadRodoviario(MDFeEletronico mdfe)
        {
            if (Rntrc.IsNullOrEmpty())
                Rntrc = mdfe.Rodoviario?.Rntrc.ToString();

            if (CodigoAgendamentoPorto.IsNullOrEmpty())
                CodigoAgendamentoPorto = mdfe.Rodoviario?.CodigoAgendamentoPorto;
        }

        protected virtual void OnAbrirFlyoutAddContratanteHandler()
        {
            AbrirFlyoutAddContratanteHandler?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAbrirFlyoutAddCiotHandler()
        {
            AbrirFlyoutAddCiotHandler?.Invoke(this, EventArgs.Empty);
        }

        public void IncluirPagamento(IncluirPagamentoRetorno informacaoPagamento)
        {
            ListarPagamentos.Remove(informacaoPagamento.Remover);
            AdicionarInformacaoPagamento(informacaoPagamento.Novo);
            PagamentoSelecionado = null;
        }

        protected virtual void OnAbrirDialogInformacaoPagamento(bool editarInformacaoPagamento)
        {
            AbrirDialogInformacaoPagamento?.Invoke(this, new EditarInformacaoPagamento(PagamentoSelecionado, editarInformacaoPagamento));
        }

        public void AddInformacaoPagamento(bool editarInformacaoPagamento)
        {
            OnAbrirDialogInformacaoPagamento(editarInformacaoPagamento);
        }

        public void DeletarInformacaoPagamento()
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                PagamentoSelecionado.Parcelas.ForEach(repositorio.Deletar);
                PagamentoSelecionado.ComponentePagamentoFrete.ForEach(repositorio.Deletar);
                repositorio.Deletar(PagamentoSelecionado);

                transacao.Commit();
            }

            ListarPagamentos.Remove(PagamentoSelecionado);
        }
    }
}