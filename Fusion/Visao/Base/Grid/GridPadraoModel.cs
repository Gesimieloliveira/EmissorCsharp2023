using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FusionLibrary.VisaoModel;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Base.Grid
{
    public abstract class GridPadraoModel<T> : ViewModel, IGridPadraoModel
    {
        private string _labelPesquisaRapida = "Pesquisa rápida";
        private bool _mostraBotaoOpcoes;
        private bool _editaRegistro = true;
        private bool _mostraBotaoNovo = true;
        private T _selecionado;
        private bool _mostraPesquisaRapida = true;
        private bool _mostraBotaoFiltro;

        public bool MostraPesquisaRapida
        {
            get => _mostraPesquisaRapida;
            set
            {
                if (value == _mostraPesquisaRapida) return;
                _mostraPesquisaRapida = value;
                PropriedadeAlterada();
            }
        }

        public IList<T> Lista
        {
            get => ItemsSource;
            set => ItemsSource = new ObservableCollection<T>(value);
        }

        public ObservableCollection<T> ItemsSource
        {
            get => GetValue<ObservableCollection<T>>();
            set => SetValue(value);
        }

        public T Selecionado
        {
            get => _selecionado;
            set
            {
                if (Equals(value, _selecionado)) return;
                _selecionado = value;
                PropriedadeAlterada();
            }
        }

        public bool MostraBotaoOpcoes
        {
            get => _mostraBotaoOpcoes;
            set
            {
                if (Equals(_mostraBotaoOpcoes, value)) return;
                _mostraBotaoOpcoes = value;
                PropriedadeAlterada();
            }
        }

        public string UltimoTextoPesquisado { get; set; }

        public string LabelPesquisaRapida
        {
            get => _labelPesquisaRapida;
            set
            {
                _labelPesquisaRapida = value;
                PropriedadeAlterada();
            }
        }

        public bool EditaRegistro
        {
            get => _editaRegistro;
            set
            {
                _editaRegistro = value;
                PropriedadeAlterada();
            }
        }

        public bool MostraBotaoNovo
        {
            get => _mostraBotaoNovo;
            set
            {
                _mostraBotaoNovo = value;
                PropriedadeAlterada();
            }
        }

        public bool MostraBotaoFiltro
        {
            get => _mostraBotaoFiltro;
            set
            {
                if (value == _mostraBotaoFiltro) return;
                _mostraBotaoFiltro = value;
                PropriedadeAlterada();
            }
        }

        public bool AutoReload { get; set; } = true;

        public virtual Window JanelaOpcoes()
        {
            return null;
        }

        protected DataGridColumn CriaColuna(string header, string bindingPath)
        {
            return CriaColuna(header, bindingPath, DataGridLength.Auto);
        }

        protected DataGridColumn CriaColuna(string header, string bindingPath, DataGridLength width)
        {
            return new DataGridTextColumn
            {
                Header = header,
                Binding = new Binding(bindingPath),
                Width = width
            };
        }

        protected DataGridColumn CriaColunaDinheiroReal(string header,
            Binding binding,
            DataGridLength width)
        {
            var col = new DataGridTextColumn
            {
                Header = header,
                Binding = binding,
                Width = width,

            };

            var style = new Style(typeof(TextBlock), col.ElementStyle);
            style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            style.Seal();

            col.ElementStyle = style;

            return col;
        }

        protected DataGridColumn CriaColuna(string header, Binding binding, DataGridLength width)
        {
            return new DataGridTextColumn
            {
                Header = header,
                Binding = binding,
                Width = width
            };
        }

        protected DataGridColumn CriaColuna(string header, Binding binding)
        {
            return CriaColuna(header, binding, DataGridLength.Auto);
        }

        public abstract ObservableCollection<DataGridColumn> ColunasDaGrid();
        public abstract void AplicarPesquisa(string texto);
        public abstract Window JanelaNovo();

        public virtual ChildWindow JanelaFiltro()
        {
            return null;
        }

        public ObservableCollection<DataGridColumn> CriaDataGrid()
        {
            return new ObservableCollection<DataGridColumn>();
        }

        public abstract Window JanelaAlterar();
        public abstract void PopularLista();

        public void Loaded()
        {
            OnLoaded();
        }

        protected virtual void OnLoaded()
        {
        }
    }
}