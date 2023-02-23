using System;
using System.Windows.Input;
using DFe.Utils;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.DocAnt
{
    public class RetornoAddDocumentoAnterior : EventArgs
    {
        public FlyoutAddDocumentoAnteriorModel Model { get; }

        public RetornoAddDocumentoAnterior(FlyoutAddDocumentoAnteriorModel model)
        {
            Model = model;
        }
    }

    public class FlyoutAddDocumentoAnteriorModel : ViewModel
    {
        private bool _isOpen;
        private TipoDocumentoAnterior _tipoDocumentoAnterior;
        private string _chaveCTe;
        private short _serie;
        private short _subSerie;
        private string _numeroDocumento;
        private DateTime _emissaoEm;
        private bool _isCte;
        private bool _isNotCte;

        public FlyoutAddDocumentoAnteriorModel()
        {
            TipoDocumentoAnterior = TipoDocumentoAnterior.CTe;
            EmissaoEm = DateTime.Now;
        }


        public event EventHandler<RetornoAddDocumentoAnterior> AddDocumentoAnteriorHandler; 

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        public TipoDocumentoAnterior TipoDocumentoAnterior
        {
            get { return _tipoDocumentoAnterior; }
            set
            {
                if (value == _tipoDocumentoAnterior) return;
                _tipoDocumentoAnterior = value;

                IsCte = value == TipoDocumentoAnterior.CTe;
                IsNotCte = !IsCte;
                PropriedadeAlterada();
                LimparCampos();
            }
        }

        public bool IsNotCte
        {
            get { return _isNotCte; }
            set
            {
                if (value == _isNotCte) return;
                _isNotCte = value;
                PropriedadeAlterada();
            }
        }

        public bool IsCte
        {
            get { return _isCte; }
            set
            {
                if (value == _isCte) return;
                _isCte = value;
                PropriedadeAlterada();
            }
        }

        public short Serie
        {
            get { return _serie; }
            set
            {
                if (value == _serie) return;
                _serie = value;
                PropriedadeAlterada();
            }
        }

        public short SubSerie
        {
            get { return _subSerie; }
            set
            {
                if (value == _subSerie) return;
                _subSerie = value;
                PropriedadeAlterada();
            }
        }

        public string ChaveCTe
        {
            get { return _chaveCTe; }
            set
            {
                if (value == _chaveCTe) return;
                _chaveCTe = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroDocumento
        {
            get { return _numeroDocumento; }
            set
            {
                if (value == _numeroDocumento) return;
                _numeroDocumento = value;
                PropriedadeAlterada();
            }
        }

        public DateTime EmissaoEm
        {
            get { return _emissaoEm; }
            set
            {
                if (value.Equals(_emissaoEm)) return;
                _emissaoEm = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandAdicionarDocumento => GetSimpleCommand(AdicionarDocumentoAction);

        private void AdicionarDocumentoAction(object obj)
        {
            try
            {
                HidratarValores();
                Validar();
                OnAddDocumentoAnteriorHandler();
                LimparCampos();
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void HidratarValores()
        {
            NumeroDocumento = NumeroDocumento.TrimOrEmpty();
            ChaveCTe = ChaveCTe.TrimOrEmpty();
        }

        private void Validar()
        {
            if (NumeroDocumento.IsNullOrEmpty() && TipoDocumentoAnterior != TipoDocumentoAnterior.CTe)
                throw new ArgumentException("Digitar um número documento");

            if (ChaveCTe.IsNullOrEmpty() && TipoDocumentoAnterior == TipoDocumentoAnterior.CTe)
                throw new ArgumentException("Digitar uma chave CT-e");

            if (ChaveCTe.Length != 44 && TipoDocumentoAnterior == TipoDocumentoAnterior.CTe)
                throw new ArgumentException("A Chave do CT-e deve ter 44 dígitos");

            if (ChaveCTe.Length == 44 && !ChaveFiscal.ChaveValida(ChaveCTe) && TipoDocumentoAnterior == TipoDocumentoAnterior.CTe)
                throw new ArgumentException("Chave CT-e esta inválida");
        }

        protected virtual void OnAddDocumentoAnteriorHandler()
        {
            AddDocumentoAnteriorHandler?.Invoke(this, new RetornoAddDocumentoAnterior(this));
        }

        private void LimparCampos()
        {
            ChaveCTe = string.Empty;
            Serie = 0;
            SubSerie = 0;
            NumeroDocumento = string.Empty;
            EmissaoEm = DateTime.Now;
        }
    }
}