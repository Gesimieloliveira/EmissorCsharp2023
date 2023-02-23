using System;
using System.Windows.Forms;
using System.Windows.Input;
using FusionCore.DFe.RegrasNegocios.Chave;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.FusionAdm.CteEletronico;
using NFe.Utils.NFe;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts.Models
{
    public class RetornoFlyoutNfe : EventArgs
    {
        public FlyoutAdicionarNfeModel FlyoutAdicionarNfeModel { get; set; }

        public RetornoFlyoutNfe(FlyoutAdicionarNfeModel flyoutAdicionarNfeModel)
        {
            FlyoutAdicionarNfeModel = flyoutAdicionarNfeModel;
        }
    }

    public class FlyoutAdicionarNfeModel : ViewModel
    {
        private bool _isOpen;
        private DateTime? _previsaoEntregaEm;
        private int _pinSuframa;
        private decimal _totalNFe;
        public ICommand CommandFiltroNfe => GetSimpleCommand(FiltroNfeAction);

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

        public string ChaveNfe
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public DateTime? PrevisaoEntregaEm
        {
            get { return _previsaoEntregaEm; }
            set
            {
                if (value.Equals(_previsaoEntregaEm)) return;
                _previsaoEntregaEm = value;
                PropriedadeAlterada();
            }
        }

        public int PinSuframa
        {
            get { return _pinSuframa; }
            set
            {
                if (value == _pinSuframa) return;
                _pinSuframa = value;
                PropriedadeAlterada();
            }
        }

        public decimal TotalNFe
        {
            get { return _totalNFe; }
            set
            {
                if (value == _totalNFe) return;
                _totalNFe = value;
                PropriedadeAlterada();
            }
        }

        private void FiltroNfeAction(object obj)
        {
            var model = new NfePickerModel();
            model.PickItemEvent += SelecionaNfeCompleted;
            model.GetPickerView().ShowDialog();
        }

        private void SelecionaNfeCompleted(object sender, GridPickerEventArgs e)
        {
            var nfePickerDTO = e.GetItem<NfePickerDTO>();
            ChaveNfe = nfePickerDTO?.Chave ?? string.Empty;
            TotalNFe = nfePickerDTO?.TotalNf ?? 0;
        }

        public event EventHandler<RetornoFlyoutNfe> AdicionaDocumentoNfe;

        public virtual void OnAdicionaDocumentoNfe()
        {
            ChaveNfe = ChaveNfe.TrimOrEmpty();
            TotalNFe = TotalNFe.Format("N2");

            Validacoes();

            AdicionaDocumentoNfe?.Invoke(this, new RetornoFlyoutNfe(this));
            LimpaCampos();
        }

        private void Validacoes()
        {
            if (string.IsNullOrEmpty(ChaveNfe)) throw new ArgumentException("Inserir uma chave");
            if (ChaveNfe.Length != 44) throw new ArgumentException("Chave deve ter 44 caracteres");

            ValidaDigitoVerificadorChave(ChaveNfe);

            if (PinSuframa != 0)
            {
                if (PinSuframa < 0) throw new ArgumentException("Pin Suframa não pode ser negativo");

                if (PinSuframa.ToString().Length < 2)
                    throw new ArgumentException("Pin Suframa deve ter no mínimo 2 digitos");
            }
        }

        private void ValidaDigitoVerificadorChave(string chaveNfe)
        {
            GerarChaveFiscal.ValidarChave(chaveNfe);
        }

        public void LimpaCampos()
        {
            ChaveNfe = default(string);
            PrevisaoEntregaEm = default(DateTime?);
            PinSuframa = default(int);
            TotalNFe = default(decimal);
        }

        public void ImportarNFe()
        {
            var dialog = new OpenFileDialog
            {
                Filter = @"Arquivos XML(*.xml)|*.xml"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            var caminho = dialog.FileName;

            try
            {
                var nfe = new NFe.Classes.NFe();
                nfe = nfe.CarregarDeArquivoXml(caminho);

                var totais = nfe.infNFe.total;
                var chave = nfe.infNFe.Id?.SubstringWithTrim(3, 44);

                TotalNFe = totais.ICMSTot.vNF;
                ChaveNfe = chave;
            }
            catch (Exception)
            {
                DialogBox.MostraInformacao("Xml selecionado incorreto");
            }
        }
    }
}