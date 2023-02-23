using System;
using System.Windows.Input;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Flyouts
{
    public class FlyoutAddDocumentoReferenciadoCteOsModel : ViewModel
    {
        private readonly CteOsEmitirFormModel _cteOsEmitirModel;
        private bool _isOpen;
        private string _numero;
        private short? _serie;
        private short? _subSerie;
        private DateTime _emitidaEm = DateTime.Now;
        private decimal? _valor;

        public FlyoutAddDocumentoReferenciadoCteOsModel(CteOsEmitirFormModel cteOsEmitirModel)
        {
            _cteOsEmitirModel = cteOsEmitirModel;
        }

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        public string Numero
        {
            get => _numero;
            set
            {
                if (value == _numero) return;
                _numero = value;
                PropriedadeAlterada();
            }
        }

        public short? Serie
        {
            get => _serie;
            set
            {
                if (value == _serie) return;
                _serie = value;
                PropriedadeAlterada();
            }
        }

        public short? SubSerie
        {
            get => _subSerie;
            set
            {
                if (value == _subSerie) return;
                _subSerie = value;
                PropriedadeAlterada();
            }
        }

        public DateTime EmitidaEm
        {
            get => _emitidaEm;
            set
            {
                if (value.Equals(_emitidaEm)) return;
                _emitidaEm = value;
                PropriedadeAlterada();
            }
        }

        public decimal? Valor
        {
            get => _valor;
            set
            {
                if (value == _valor) return;
                _valor = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandAdicionar => GetSimpleCommand(AdicionarAction);

        public ICommand CommandAdicionarEFechar => GetSimpleCommand(AdicionarEFecharAction);

        private void AdicionarAction(object obj)
        {
            Salvar();
            LimparCampos();
        }

        private void AdicionarEFecharAction(object obj)
        {
            Salvar();
            IsOpen = false;
        }

        private void Salvar()
        {
            try
            {
                Numero = Numero.TrimOrEmpty();

                Validacoes();
                _cteOsEmitirModel.AdicionaDocumentoReferenciado(new CteOsDocumentoReferenciado
                {
                    Numero = Numero,
                    Serie = Serie,
                    SubSerie = SubSerie,
                    EmitidaEm = EmitidaEm,
                    Valor = Valor
                });
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void Validacoes()
        {
            if (Numero.IsNullOrEmpty())
                throw new InvalidOperationException("Campo número obrigatório");
        }

        private void LimparCampos()
        {
            Numero = string.Empty;
            Serie = null;
            SubSerie = null;
            EmitidaEm = DateTime.Now;
            Valor = null;
        }
    }
}