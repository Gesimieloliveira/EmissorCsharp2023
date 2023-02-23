using System;
using System.Windows.Input;
using Fusion.Visao.PerfilCfop;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts.Models
{
    public class RetornoNotaFiscalImpressa : EventArgs
    {
        public FlyoutAdicionarNotaFiscalImpressaModel FlyoutAdicionarNotaFiscalImpressaModel { get; set; }

        public RetornoNotaFiscalImpressa(FlyoutAdicionarNotaFiscalImpressaModel flyoutAdicionarNotaFiscalImpressaModel)
        {
            FlyoutAdicionarNotaFiscalImpressaModel = flyoutAdicionarNotaFiscalImpressaModel;
        }
    }

    public sealed class FlyoutAdicionarNotaFiscalImpressaModel : ModelBase
    {
        private bool _isOpen;
        private short _serie;
        private string _numeroRomaneiro;
        private string _numeroPedidoNf;
        private ModeloNotaFiscal _modeloNotaFiscal = ModeloNotaFiscal.NFModelo011AeAvulsa;
        private string _numero;
        private DateTime _emitidaEm = DateTime.Now;
        private decimal _valorBaseCalculoIcms;
        private decimal _valorTotalIcms;
        private decimal _valorBaseCalculoIcmsSt;
        private decimal _valorTotalIcmsSt;
        private decimal _valorTotalProduto;
        private decimal _valorTotalNf;
        private string _codigoPerfilCfop;
        private string _descricaoPerfilCfop;
        private PerfilCfopDTO _perfilCfop;
        private decimal _pesoTotalEmKg;
        private int _pinSuframa;
        private DateTime? _dataPrevistaEntrega;

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

        public string NumeroRomaneiro
        {
            get { return _numeroRomaneiro; }
            set
            {
                if (value == _numeroRomaneiro) return;
                _numeroRomaneiro = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroPedidoNf
        {
            get { return _numeroPedidoNf; }
            set
            {
                if (value == _numeroPedidoNf) return;
                _numeroPedidoNf = value;
                PropriedadeAlterada();
            }
        }

        public ModeloNotaFiscal ModeloNotaFiscal
        {
            get { return _modeloNotaFiscal; }
            set
            {
                if (value == _modeloNotaFiscal) return;
                _modeloNotaFiscal = value;
                PropriedadeAlterada();
            }
        }

        public string Numero
        {
            get { return _numero; }
            set
            {
                if (value == _numero) return;
                _numero = value;
                PropriedadeAlterada();
            }
        }

        public DateTime EmitidaEm
        {
            get { return _emitidaEm; }
            set
            {
                if (value.Equals(_emitidaEm)) return;
                _emitidaEm = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorBaseCalculoIcms
        {
            get { return _valorBaseCalculoIcms; }
            set
            {
                if (value == _valorBaseCalculoIcms) return;
                _valorBaseCalculoIcms = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorTotalIcms
        {
            get { return _valorTotalIcms; }
            set
            {
                if (value == _valorTotalIcms) return;
                _valorTotalIcms = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorBaseCalculoIcmsSt
        {
            get { return _valorBaseCalculoIcmsSt; }
            set
            {
                if (value == _valorBaseCalculoIcmsSt) return;
                _valorBaseCalculoIcmsSt = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorTotalIcmsSt
        {
            get { return _valorTotalIcmsSt; }
            set
            {
                if (value == _valorTotalIcmsSt) return;
                _valorTotalIcmsSt = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorTotalProduto
        {
            get { return _valorTotalProduto; }
            set
            {
                if (value == _valorTotalProduto) return;
                _valorTotalProduto = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorTotalNf
        {
            get { return _valorTotalNf; }
            set
            {
                if (value == _valorTotalNf) return;
                _valorTotalNf = value;
                PropriedadeAlterada();
            }
        }

        public string CodigoPerfilCfop
        {
            get { return _codigoPerfilCfop; }
            set
            {
                if (value == _codigoPerfilCfop) return;
                _codigoPerfilCfop = value;
                PropriedadeAlterada();
            }
        }

        public string DescricaoPerfilCfop
        {
            get { return _descricaoPerfilCfop; }
            set
            {
                if (value == _descricaoPerfilCfop) return;
                _descricaoPerfilCfop = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscaPerfilCfop => GetSimpleCommand(BuscaPerfilCfop);

        public PerfilCfopDTO PerfilCfop
        {
            get { return _perfilCfop; }
            set
            {
                if (Equals(value, _perfilCfop)) return;
                _perfilCfop = value;
                CodigoPerfilCfop = _perfilCfop?.Codigo;
                DescricaoPerfilCfop = _perfilCfop?.Descricao;
                PropriedadeAlterada();
            }
        }

        public decimal PesoTotalEmKg
        {
            get { return _pesoTotalEmKg; }
            set
            {
                if (value == _pesoTotalEmKg) return;
                _pesoTotalEmKg = value;
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

        public DateTime? DataPrevistaEntrega
        {
            get { return _dataPrevistaEntrega; }
            set
            {
                if (value.Equals(_dataPrevistaEntrega)) return;
                _dataPrevistaEntrega = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<RetornoNotaFiscalImpressa> AdicionarNotaFiscalImpressa;

        private void BuscaPerfilCfop(object obj)
        {
            var pickerModel = new PerfilCfopPickerModel();
            pickerModel.PickItemEvent += PerfilCfopSelecionado;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void PerfilCfopSelecionado(object sender, GridPickerEventArgs e)
        {
            PerfilCfop = e.GetItem<PerfilCfopDTO>();
        }

        public void OnAdicionarNotaFiscalImpressa()
        {
            NumeroRomaneiro = NumeroRomaneiro.TrimOrEmpty();
            NumeroPedidoNf = NumeroPedidoNf.TrimOrEmpty();
            Numero = Numero.TrimOrEmpty();
            ValorBaseCalculoIcms = ValorBaseCalculoIcms.Format("N2");
            ValorTotalIcms = ValorTotalIcms.Format("N2");
            ValorBaseCalculoIcmsSt = ValorBaseCalculoIcmsSt.Format("N2");
            ValorTotalIcmsSt = ValorTotalIcmsSt.Format("N2");
            ValorTotalProduto = ValorTotalProduto.Format("N2");
            ValorTotalNf = ValorTotalNf.Format("N2");
            PesoTotalEmKg = PesoTotalEmKg.Format("N3");

            Validacoes();

            AdicionarNotaFiscalImpressa?.Invoke(this, new RetornoNotaFiscalImpressa(this));

            LimpaCampos();
        }

        public void Validacoes()
        {
            if (Serie == 0) throw new ArgumentException("Digitar uma série");
            if (string.IsNullOrEmpty(Numero)) throw new ArgumentException("Digitar um número");
            if (PerfilCfop == null) throw new ArgumentException("Selecionar um cfop");

            ValidaPinSuframa();
        }

        private void ValidaPinSuframa()
        {
            if (PinSuframa == 0) return;
            if (PinSuframa < 0) throw new ArgumentException("Pin Suframa não pode ser negativo");
            if (PinSuframa.ToString().Length < 2)
                throw new ArgumentException("Pin Suframa deve ter no mínimo 2 digitos");
        }

        public void LimpaCampos()
        {
            Serie = 0;
            Numero = string.Empty;
            EmitidaEm = DateTime.Now;
            ModeloNotaFiscal = ModeloNotaFiscal.NFModelo011AeAvulsa;
            PerfilCfop = null;
            NumeroRomaneiro = string.Empty;
            NumeroPedidoNf = string.Empty;
            DataPrevistaEntrega = null;
            ValorBaseCalculoIcms = 0;
            ValorTotalIcms = 0;
            ValorBaseCalculoIcmsSt = 0;
            ValorTotalIcmsSt = 0;
            PesoTotalEmKg = 0;
            PinSuframa = 0;
            ValorTotalProduto = 0;
            ValorTotalNf = 0;
        }
    }
}