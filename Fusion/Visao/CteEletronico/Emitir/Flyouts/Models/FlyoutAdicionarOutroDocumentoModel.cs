using System;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts.Models
{
    public class RetornoOutroDocumento
    {
        public FlyoutAdicionarOutroDocumentoModel FlyoutAdicionarOutroDocumentoModel { get; set; }

        public RetornoOutroDocumento(FlyoutAdicionarOutroDocumentoModel flyoutAdicionarOutroDocumentoModel)
        {
            FlyoutAdicionarOutroDocumentoModel = flyoutAdicionarOutroDocumentoModel;
        }
    }

    public class FlyoutAdicionarOutroDocumentoModel : ViewModel
    {
        private bool _isOpen;
        private TipoDocumento _tipoDocumento;
        private string _descricaoOutros;
        private string _numero;
        private DateTime? _emitidoEm;
        private DateTime? _previsaoEntregaEm;
        private decimal _valorTotal;
        private bool _editarDescricaoOutros;

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

        public TipoDocumento TipoDocumento
        {
            get { return _tipoDocumento; }
            set
            {
                if (value == _tipoDocumento) return;
                _tipoDocumento = value;
                PropriedadeAlterada();
                DeterminaCampoDescricaoOutros(value);
            }
        }

        public string DescricaoOutros
        {
            get { return _descricaoOutros; }
            set
            {
                if (value == _descricaoOutros) return;
                _descricaoOutros = value;
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

        public DateTime? EmitidoEm
        {
            get { return _emitidoEm; }
            set
            {
                if (value.Equals(_emitidoEm)) return;
                _emitidoEm = value;
                PropriedadeAlterada();
            }
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

        public decimal ValorTotal
        {
            get { return _valorTotal; }
            set
            {
                if (value == _valorTotal) return;
                _valorTotal = value;
                PropriedadeAlterada();
            }
        }

        public bool EditarDescricaoOutros
        {
            get { return _editarDescricaoOutros; }
            set
            {
                if (value == _editarDescricaoOutros) return;
                _editarDescricaoOutros = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAdicionarOutroDocumentoModel()
        {
            TipoDocumento = TipoDocumento.Outros;
        }

        public event EventHandler<RetornoOutroDocumento> AdicionarOutroDocumento;

        private void DeterminaCampoDescricaoOutros(TipoDocumento tipoDocumento)
        {
            switch (tipoDocumento)
            {
                case TipoDocumento.Declaracao:
                case TipoDocumento.Dutoviario:
                case TipoDocumento.CfeSat:
                case TipoDocumento.Nfce:
                    EditarDescricaoOutros = false;
                    DescricaoOutros = string.Empty;
                    break;
                case TipoDocumento.Outros:
                    EditarDescricaoOutros = true;
                    DescricaoOutros = string.Empty;
                    break;
            }
        }

        public virtual void OnAdicionarOutroDocumento()
        {
            DescricaoOutros = DescricaoOutros.TrimOrEmpty();
            Numero = Numero.TrimOrEmpty();
            ValorTotal = ValorTotal.Format("N2");

            Validacoes();

            AdicionarOutroDocumento?.Invoke(this, new RetornoOutroDocumento(this));

            LimpaCampos();
        }

        private void Validacoes()
        {
            if (TipoDocumento == TipoDocumento.Outros && string.IsNullOrEmpty(DescricaoOutros))
                throw new ArgumentException("Adicionar descrição outros");
        }

        public void LimpaCampos()
        {
            TipoDocumento = TipoDocumento.Outros;
            DescricaoOutros = string.Empty;
            Numero = string.Empty;
            EmitidoEm = null;
            PrevisaoEntregaEm = null;
            ValorTotal = 0;
        }
    }
}