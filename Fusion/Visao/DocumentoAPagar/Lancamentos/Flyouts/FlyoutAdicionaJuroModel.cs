using System;
using FusionCore.FusionAdm.Financeiro;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.DocumentoAPagar.Lancamentos.Flyouts
{
    public class FlyoutAdicionaJuroArgs : EventArgs
    {
        public decimal Juros { get; }
        public string Historico { get; }

        public FlyoutAdicionaJuroArgs(decimal juros, string historico = null)
        {
            Juros = juros;
            Historico = historico ?? string.Empty;
        }
    }

    public class FlyoutAdicionaJuroModel : ModelBase
    {
        private readonly DocumentoPagar _documentoPagar;
        private bool _isOpen;
        private decimal _valorOriginal;
        private decimal _valorAjustado;
        private decimal _valorQuitado;
        private decimal _valorJuros;
        private string _historico;
        private decimal _valor;
        private decimal _valorAjustadoCopia;

        public FlyoutAdicionaJuroModel(DocumentoPagar documentoPagar)
        {
            _documentoPagar = documentoPagar;
            InicializaModel();
        }

        public event EventHandler<FlyoutAdicionaJuroArgs> Retorno;

        private void InicializaModel()
        {
            ValorOriginal = _documentoPagar.ValorOriginal;
            ValorAjustado = _documentoPagar.ValorAjustado;
            _valorAjustadoCopia = _valorAjustado;
            ValorQuitado = _documentoPagar.ValorQuitado;

            Historico = string.Empty;
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

        public decimal ValorOriginal
        {
            get => _valorOriginal.Arredonda(2);
            set
            {
                if (value == _valorOriginal) return;
                _valorOriginal = value.Arredonda(2);
                PropriedadeAlterada();
            }
        }

        public decimal ValorAjustado
        {
            get => _valorAjustado.Arredonda(2);
            set
            {
                if (value == _valorAjustado) return;
                _valorAjustado = value.Arredonda(2);
                PropriedadeAlterada();
            }
        }

        public decimal ValorQuitado
        {
            get => _valorQuitado.Arredonda(2);
            set
            {
                if (value == _valorQuitado) return;
                _valorQuitado = value.Arredonda(2);
                PropriedadeAlterada();
            }
        }

        public decimal ValorJuros
        {
            get => _valorJuros.Arredonda(2);
            set
            {
                if (value == _valorJuros) return;
                _valorJuros = value.Arredonda(2);
                PropriedadeAlterada();
            }
        }

        public string Historico
        {
            get => _historico;
            set
            {
                if (value == _historico) return;
                _historico = value;
                PropriedadeAlterada();
            }
        }

        public decimal Valor
        {
            get => _valor.Arredonda(2);
            set
            {
                if (value == _valor) return;
                try
                {
                    RecalcularTotais(value.Arredonda(2));
                }
                catch (ArgumentException ex)
                {
                    DialogBox.MostraInformacao(ex.Message);
                    return;
                }

                _valor = value.Arredonda(2);
                PropriedadeAlterada();
                PropriedadeAlterada(nameof(ValorAjustado));
                PropriedadeAlterada(nameof(ValorJuros));
            }
        }

        private void RecalcularTotais(decimal valor)
        {
            _valorJuros = valor;
            _valorAjustado = valor + _valorAjustadoCopia;
        }

        public void Salvar()
        {
            if (ValorJuros <= 0)
            {
                throw new InvalidOperationException("Valor do juros precisa ser maior que 0,00");
            }

            OnRetorno(new FlyoutAdicionaJuroArgs(Valor, Historico));
        }

        protected virtual void OnRetorno(FlyoutAdicionaJuroArgs args)
        {
            Retorno?.Invoke(this, args);
        }
    }
}