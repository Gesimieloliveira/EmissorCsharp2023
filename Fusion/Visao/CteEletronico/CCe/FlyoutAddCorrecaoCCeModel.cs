using System;
using System.Windows.Input;
using FusionCore.FusionAdm.CteEletronico.CCe;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.FusionAdm.CteEletronico;

namespace Fusion.Visao.CteEletronico.CCe
{
    public class FlyoutAddCorrecaoCCeModelEvent : EventArgs
    {
        public FlyoutAddCorrecaoCCeModel Model { get; private set; }

        public FlyoutAddCorrecaoCCeModelEvent(FlyoutAddCorrecaoCCeModel model)
        {
            Model = model;
        }
    }

    public class FlyoutAddCorrecaoCCeModel : ViewModel
    {
        private bool _isOpen;
        private string _grupoAlterado;
        private string _campoAlterado;
        private string _valorAlterado;
        private byte _numeroItem;
        private bool _adicionarManual;
        private string _elementoCorrigido = "Buscar elemento para correção";
        public ICommand CommandAdicionarCorrecao => GetSimpleCommand(AdicionarCorrecao);
        public ICommand CommandBuscarPreCorrecao => GetSimpleCommand(BuscarPreCorrecao);

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;

                if (_isOpen) LimparCampos();

                PropriedadeAlterada();
            }
        }

        public string ElementoCorrigido
        {
            get { return _elementoCorrigido; }
            set
            {
                if (value == _elementoCorrigido) return;
                _elementoCorrigido = value;
                PropriedadeAlterada();
            }
        }

        public bool AdicionarManual
        {
            get { return _adicionarManual; }
            set
            {
                if (value == _adicionarManual) return;
                _adicionarManual = value;
                PropriedadeAlterada();
            }
        }

        public string GrupoAlterado
        {
            get { return _grupoAlterado; }
            set
            {
                if (value == _grupoAlterado) return;
                _grupoAlterado = value;
                PropriedadeAlterada();
            }
        }

        public string CampoAlterado
        {
            get { return _campoAlterado; }
            set
            {
                if (value == _campoAlterado) return;
                _campoAlterado = value;
                PropriedadeAlterada();
            }
        }

        public string ValorAlterado
        {
            get { return _valorAlterado; }
            set
            {
                if (value == _valorAlterado) return;
                _valorAlterado = value;
                PropriedadeAlterada();
            }
        }

        public byte NumeroItem
        {
            get { return _numeroItem; }
            set
            {
                if (value == _numeroItem) return;
                _numeroItem = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<FlyoutAddCorrecaoCCeModelEvent> AdicionarCartaCorrecao;

        private void BuscarPreCorrecao(object obj)
        {
            var model = new CteCCePickerModel();
            model.PickItemEvent += RetornoPickerItem;
            model.GetPickerView().ShowDialog();
        }

        private void RetornoPickerItem(object sender, GridPickerEventArgs e)
        {
            var cce = e.GetItem<ElementoCCe>();

            if (cce == null) return;

            GrupoAlterado = cce.Pai?.Tag;
            CampoAlterado = cce.Tag;
            ElementoCorrigido = cce.Descricao;
        }

        private void AdicionarCorrecao(object obj)
        {
            try
            {
                Validacoes();

                OnAdicionarCartaCorrecao(this);

                LimparCampos();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void Validacoes()
        {
            if (GrupoAlterado.IsNullOrEmpty()) throw new InvalidOperationException("Grupo Alterado não pode ser vazio");
            if (CampoAlterado.IsNullOrEmpty()) throw new InvalidOperationException("Campo Alterado não pode ser vazio");
            if (ValorAlterado.IsNullOrEmpty()) throw new InvalidOperationException("Valor Alterado não pode ser vazio");


            GrupoAlterado = GrupoAlterado.Trim();
            CampoAlterado = CampoAlterado.Trim();
            ValorAlterado = ValorAlterado.Trim();
        }

        private void LimparCampos()
        {
            ElementoCorrigido = "Buscar elemento para correção";
            GrupoAlterado = string.Empty;
            CampoAlterado = string.Empty;
            ValorAlterado = string.Empty;
            AdicionarManual = false;
            NumeroItem = 0;
        }

        protected virtual void OnAdicionarCartaCorrecao(FlyoutAddCorrecaoCCeModel model)
        {
            AdicionarCartaCorrecao?.Invoke(this, new FlyoutAddCorrecaoCCeModelEvent(model));
        }
    }
}