using System;
using System.Windows.Input;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Flyouts
{
    public class FlyoutAddComponenteCteOsModel : ViewModel
    {
        private readonly CteOsEmitirFormModel _cteOsEmitirModel;

        private bool _isOpen;
        private string _nomeDoComponente;
        private decimal _valorDoComponente;

        public FlyoutAddComponenteCteOsModel(CteOsEmitirFormModel cteOsEmitirModel)
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

        public string NomeDoComponente
        {
            get => _nomeDoComponente;
            set
            {
                if (value == _nomeDoComponente) return;
                _nomeDoComponente = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorDoComponente
        {
            get => _valorDoComponente;
            set
            {
                if (value == _valorDoComponente) return;
                _valorDoComponente = value;
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

        private void LimparCampos()
        {
            NomeDoComponente = string.Empty;
            ValorDoComponente = 0.0m;
        }

        private void Salvar()
        {
            try
            {
                Validacoes();
                _cteOsEmitirModel.AdicionaComponente(new CteOsComponenteValorPrestacao
                {
                    Nome = NomeDoComponente.TrimOrEmpty(),
                    Valor = ValorDoComponente
                });
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void Validacoes()
        {
            if (ExtValidaListasEColecoes.IsNullOrEmpty(NomeDoComponente))
                throw new InvalidOperationException("Nome do componente é obrigatório");
        }

        private void AdicionarEFecharAction(object obj)
        {
            Salvar();
            IsOpen = false;
        }
    }
}