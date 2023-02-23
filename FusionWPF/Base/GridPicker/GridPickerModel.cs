using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker.Contrato;
using FusionWPF.Base.GridPicker.Filtros;
using FusionWPF.Base.GridPicker.Flyout;
using FusionWPF.Base.GridPicker.OpcoesBuscas;

namespace FusionWPF.Base.GridPicker
{
    public abstract class GridPickerModel : ViewModel
    {
        private UserControl _userControlFiltro;
        private bool _inicializarComPesquisa;
        private string _textoInicializarComPesquisa;
        private ObservableCollection<IOpcaoBusca> _tipoBuscas = new ObservableCollection<IOpcaoBusca>();
        private IOpcaoBusca _buscaSelecionada;
        private bool _isTemTipoBuscas;
        private object _modelFiltro;
        private bool _qtdeMaximaFoiAlcancada;
        private int _qtdeMaximaItens;
        public FlyoutGridPickerModel FlyoutGridPickerModel { get; } = new FlyoutGridPickerModel();

        public ObservableCollection<IGridPickerItem> ItensLista { get; private set; } =
            new ObservableCollection<IGridPickerItem>();

        public object ModelFiltro
        {
            get => _modelFiltro;
            set
            {
                _modelFiltro = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<IOpcaoBusca> TipoBuscas
        {
            get => _tipoBuscas;
            set
            {
                _tipoBuscas = value;
                PropriedadeAlterada();
            }
        }

        public bool IsTemTipoBuscas
        {
            get => _isTemTipoBuscas;
            set
            {
                _isTemTipoBuscas = value;
                PropriedadeAlterada();
            }
        }

        public IOpcaoBusca BuscaSelecionada
        {
            get => _buscaSelecionada;
            set
            {
                _buscaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public bool AtivaFiltro
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public IGridPickerItem ItemSelecionado
        {
            get => GetValue<IGridPickerItem>();
            set => SetValue(value);
        }

        public string Titulo
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string TextoPesquisado
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool HabilitaBotaoNovo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool HabilitaBotaoEditar
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ICommand CommandFiltro => GetSimpleCommand(CommandFiltroAction);
        public ICommand CommandNovo => GetSimpleCommand(CommandNovoAction);
        public ICommand AplicarPesquisaGridCommand => GetSimpleCommand(AplicarPesquisaAction);

        public bool QtdeMaximaFoiAlcancada
        {
            get => _qtdeMaximaFoiAlcancada;
            set
            {
                _qtdeMaximaFoiAlcancada = value;
                PropriedadeAlterada();
            }
        }

        public int QtdeMaximaItens
        {
            get => _qtdeMaximaItens;
            set
            {
                _qtdeMaximaItens = value;
                PropriedadeAlterada();
            }
        }

        private void AplicarPesquisaAction(object obj)
        {
            AplicaPesquisa(TextoPesquisado);
        }

        public event EventHandler<GridPickerEventArgs> PickItemEvent;
        internal event EventHandler CloseRequest;

        protected void SetItensLista(IEnumerable<IGridPickerItem> lista)
        {
            ItensLista = new ObservableCollection<IGridPickerItem>(lista);
            PropriedadeAlterada(nameof(ItensLista));
        }

        internal void Inicializar()
        {
            if (_inicializarComPesquisa)
            {
                TextoPesquisado = _textoInicializarComPesquisa;
                AplicaPesquisa(TextoPesquisado);
            }
            OnInicializar();
        }

        public void InicializarComPesquisa(string textoPesquisa)
        {
            _inicializarComPesquisa = true;
            _textoInicializarComPesquisa = textoPesquisa;
        }

        public void SelecionarItem()
        {
            OnPickItem(ItemSelecionado);
        }

        protected virtual void OnPickItem(IGridPickerItem item)
        {
            OnPickItemEvent(new GridPickerEventArgs(item));
        }

        protected void OnPickItemEvent(GridPickerEventArgs args)
        {
            try
            {
                PickItemEvent?.Invoke(this, args);
            }
            finally
            {
                CloseRequest?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CommandFiltroAction(object obj)
        {
            OnAntesDeAtivarFiltro(ModelFiltro);
            FlyoutGridPickerModel.IsOpen = true;
            FlyoutGridPickerModel.Filtro = ModelFiltro;
            FlyoutGridPickerModel.CommandAplicarFiltro = GetSimpleCommand(AplicaFiltroNaPesquisaAction);
        }

        protected virtual void OnAntesDeAtivarFiltro(object modelFiltro)
        {

        }

        private void AplicaFiltroNaPesquisaAction(object obj)
        {
            var objetoFiltro = obj as FlyoutGridPickerModel;
            var filtroRetorno = new FiltroRetorno(objetoFiltro?.Filtro, true);

            OnAplicaFiltro(filtroRetorno);

            if (filtroRetorno.IsFecharJanela)
                FlyoutGridPickerModel.IsOpen = false;
        }

        protected virtual void OnAplicaFiltro(FiltroRetorno filtroRetorno)
        {
        }

        public void InicializarFiltro()
        {
            var builder = new Filtro.FiltroBuilder();

            UsarFiltro(builder);

            var filtro = builder.BuilderFiltro();

            _userControlFiltro = filtro.UserControlFiltro;
            ModelFiltro = filtro.Model;
            AtivaFiltro = filtro.AtivarFiltro;
        }

        protected virtual void UsarFiltro(Filtro.FiltroBuilder builder)
        {
        }

        public GridPicker GetPickerView()
        {
            return new GridPicker(this);
        }

        public void ShowPickerDialog()
        {
            GetPickerView().ShowDialog();
        }

        public UserControl FiltroFlayout()
        {
            return _userControlFiltro;
        }

        private void CommandNovoAction(object obj)
        {
            OnNovoRegistro();
        }

        internal void EditarItemSelecionado()
        {
            OnEditarRegistro(ItemSelecionado);
        }

        protected virtual void OnEditarRegistro(IGridPickerItem item)
        {
        }

        protected virtual void OnNovoRegistro()
        {
        }

        protected virtual void OnInicializar()
        {
        }

        public abstract void AplicaPesquisa(string input);
    }
}