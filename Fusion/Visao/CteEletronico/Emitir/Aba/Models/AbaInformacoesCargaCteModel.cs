using System;
using System.Collections.ObjectModel;
using Fusion.Visao.CteEletronico.Emitir.EntidadesModels;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.CteEletronico.Emitir.Aba.Models
{
    public class RetornoInformacoesCarga
    {
        public AbaInformacoesCargaCteModel AbaInformacoesCargaCteModel { get; set; }

        public RetornoInformacoesCarga(AbaInformacoesCargaCteModel abaInformacoesCargaCteModel)
        {
            AbaInformacoesCargaCteModel = abaInformacoesCargaCteModel;
        }
    }

    public sealed class AbaInformacoesCargaCteModel : ModelBase
    {
        public static string HeaderPasso6 = "Passo 6: Informações da Carga";
        public static string HeaderPasso5 = "Passo 5: Informações da Carga";

        private readonly CteFacade _cteFacade;
        private bool _selecionado;
        private ObservableCollection<GridInformacaoCarga> _listaCarga;
        private GridInformacaoCarga _cargaSelecionada;
        private bool _habilitado;
        private decimal _valorTotalCarga;
        private string _nomeProdutoPredominante;
        private string _caracteristicaProdutoPredominante;

        private ObservableCollection<GridVeiculoParaTransporteModel> _listaVeiculoParaTransporte;
        private GridVeiculoParaTransporteModel _veiculoParaTransporteSelecionado;
        private string _header;
        private bool _isNaoEComplementar;

        public string NomeProdutoPredominante
        {
            get => _nomeProdutoPredominante;
            set
            {
                if (value == _nomeProdutoPredominante) return;
                _nomeProdutoPredominante = value;
                PropriedadeAlterada();
            }
        }

        public string CaracteristicaProdutoPredominante
        {
            get => _caracteristicaProdutoPredominante;
            set
            {
                if (value == _caracteristicaProdutoPredominante) return;
                _caracteristicaProdutoPredominante = value;
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

        public ObservableCollection<GridInformacaoCarga> ListaCarga
        {
            get => _listaCarga;
            set
            {
                if (Equals(value, _listaCarga)) return;
                _listaCarga = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridVeiculoParaTransporteModel> ListaVeiculoParaTransporte
        {
            get => _listaVeiculoParaTransporte;
            set
            {
                if (Equals(value, _listaVeiculoParaTransporte)) return;
                _listaVeiculoParaTransporte = value;
                PropriedadeAlterada();
            }
        }

        public GridVeiculoParaTransporteModel VeiculoParaTransporteSelecionado
        {
            get => _veiculoParaTransporteSelecionado;
            set
            {
                if (Equals(value, _veiculoParaTransporteSelecionado)) return;
                _veiculoParaTransporteSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public GridInformacaoCarga CargaSelecionada
        {
            get => _cargaSelecionada;
            set
            {
                if (Equals(value, _cargaSelecionada)) return;
                _cargaSelecionada = value;
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

        public decimal ValorTotalCarga
        {
            get => _valorTotalCarga;
            set
            {
                if (value == _valorTotalCarga) return;
                _valorTotalCarga = value;
                PropriedadeAlterada();
            }
        }

        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                PropriedadeAlterada();
            }
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

        public AbaInformacoesCargaCteModel(CteFacade cteFacade)
        {
            _cteFacade = cteFacade;
            Inicializa();
        }

        public event EventHandler PassoAnterior;
        public event EventHandler AdicionarInformacaoCargaCall;
        public event EventHandler<RetornoInformacoesCarga> EmitirCte;

        public void Anterior()
        {
            OnPassoAnterior();
        }

        private void Validacoes()
        {
            if (string.IsNullOrEmpty(NomeProdutoPredominante) && IsNaoEComplementar)
            {
                throw new ArgumentException("Adicionar produto predominante");
            }
        }

        private void OnPassoAnterior()
        {
            PassoAnterior?.Invoke(this, EventArgs.Empty);
        }

        public void OnAdicionarInformacaoCargaCall()
        {
            AdicionarInformacaoCargaCall?.Invoke(this, EventArgs.Empty);
        }

        public void DeletaCargaSelecionada()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _cteFacade.DeletaInfoQuantidadeCarga(sessao, CargaSelecionada.InfoQuantidadeCarga);

                transacao.Commit();
            }

            ListaCarga.Remove(CargaSelecionada);
        }

        private void Inicializa()
        {
            if (ListaCarga == null)
            {
                ListaCarga = new ObservableCollection<GridInformacaoCarga>();
            }

            if (ListaVeiculoParaTransporte == null)
            {
                ListaVeiculoParaTransporte = new ObservableCollection<GridVeiculoParaTransporteModel>();
            }
        }

        public void AdicionaCarga(GridInformacaoCarga carga)
        {
            ListaCarga.Add(carga);
        }

        public void PreencerCom(Cte cte)
        {
            CarregaListaCarga(cte);
            NomeProdutoPredominante = cte.NomeProdutoPredominante;
            CaracteristicaProdutoPredominante = cte.CaracteristicaProdutoPredominante;

            if (string.IsNullOrWhiteSpace(NomeProdutoPredominante))
            {
                NomeProdutoPredominante = ObterProdutoPredominanteDoPerfil(cte);
            }
        }

        private static string ObterProdutoPredominanteDoPerfil(Cte cte)
        {
            if (cte.IsNormal() == false) return string.Empty;

            return cte.PerfilCte.ProdutoPredominante;
        }

        private void CarregaListaCarga(Cte cte)
        {
            cte.CteInfoQuantidadeCargas.ForEach(carga => { AdicionaCarga(GridInformacaoCarga.Cria(carga)); });

            cte.CteVeiculoTransportados.ForEach(
                vt => { AdicionaVeiculoDeTransporte(GridVeiculoParaTransporteModel.Cria(vt)); });
        }

        public void EmiteCte()
        {
            try
            {
                NomeProdutoPredominante = NomeProdutoPredominante.TrimOrEmpty();
                CaracteristicaProdutoPredominante = CaracteristicaProdutoPredominante.TrimOrEmpty();
                ValorTotalCarga = ValorTotalCarga.Format("N2");

                Validacoes();

                OnEmitirCte();
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void OnEmitirCte()
        {
            EmitirCte?.Invoke(this, new RetornoInformacoesCarga(this));
        }

        public event EventHandler<EventArgs> AdicionarVeiculoNovoCall;

        public void OnAdicionarVeiculoNovoCall()
        {
            AdicionarVeiculoNovoCall?.Invoke(this, EventArgs.Empty);
        }

        public void DeletarVeiculoParaTransporteSelecionado()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _cteFacade.DeletaVeiculoTransportado(sessao, VeiculoParaTransporteSelecionado.VeiculoTransportado);

                transacao.Commit();
            }

            ListaVeiculoParaTransporte.Remove(VeiculoParaTransporteSelecionado);
        }

        public void AdicionaVeiculoDeTransporte(GridVeiculoParaTransporteModel model)
        {
            ListaVeiculoParaTransporte.Add(model);
        }
    }
}