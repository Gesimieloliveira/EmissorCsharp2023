using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using JetBrains.Annotations;

namespace Fusion.Controles
{
    public class CidadeComboPicker : Control, INotifyPropertyChanged
    {
        private ComboBox _comboBoxCidades;
        private ObservableCollection<CidadeDTO> _cidades;
        private CidadeDTO _cidade;
        private const string PartCidades = "PART_Cidades";

        public static readonly DependencyProperty SelecionadoProperty = DependencyProperty.Register("Selecionado", typeof(CidadeDTO), typeof(CidadeComboPicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelecionadoChangedHandler));


        public CidadeDTO Selecionado
        {
            set => SetValue(SelecionadoProperty, value);
            get => (CidadeDTO)GetValue(SelecionadoProperty);
        }

        private static void SelecionadoChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _comboBoxCidades = (ComboBox) GetTemplateChild(PartCidades);

            if (_comboBoxCidades == null) return;

            Cidades = new ObservableCollection<CidadeDTO>(LocalidadesServico.GetInstancia().GetCidades());

            _comboBoxCidades.DataContext = this;
        }

        public CidadeDTO Cidade
        {
            get => _cidade;
            set
            {
                _cidade = value;
                Selecionado = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CidadeDTO> Cidades
        {
            get => _cidades;
            set
            {
                _cidades = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}