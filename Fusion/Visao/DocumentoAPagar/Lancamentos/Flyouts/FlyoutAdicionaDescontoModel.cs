using System;
using FusionCore.FusionAdm.Financeiro;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.DocumentoAPagar.Lancamentos.Flyouts
{
    public class FlyoutAdicionaDescontoArgs : EventArgs
    {
        public decimal Desconto { get; private set; }
        public string Historico { get; private set; }

        public FlyoutAdicionaDescontoArgs(decimal desconto, string historico = null)
        {
            Desconto = desconto;
            Historico = historico ?? string.Empty;
        }
    }

    public class FlyoutAdicionaDescontoModel : ModelBase
    {
        private bool _isOpen;
        private readonly DocumentoPagar _documentoPagar;
        private decimal _valorOriginal;
        private decimal _valorAjustado;
        private decimal _valorQuitado;
        private decimal _valorJuros;
        private decimal _descontoDisponivel;
        private string _historico;
        private decimal _valor;
        private decimal _descontoDisponivelClone;
        private decimal _valorAjustadoClone;

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

        public decimal DescontoDisponivel
        {
            get => _descontoDisponivel.Arredonda(2);
            set
            {
                if (value == _descontoDisponivel) return;
                _descontoDisponivel = value.Arredonda(2);
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
            get => _valor;
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
                PropriedadeAlterada(nameof(DescontoDisponivel));
            }
        }

        private void RecalcularTotais(decimal valor)
        {
            if (valor > _descontoDisponivelClone)
            {
                throw new ArgumentException(@"O desconto não pode ser maior que o Desconto Disponível");
            }

            _descontoDisponivel = _descontoDisponivelClone - valor;

            if (valor == _valorAjustadoClone)
            {
                _valorAjustado = _valorAjustadoClone;
                return;
            }

            _valorAjustado = _valorAjustadoClone - valor;
        }


        public event EventHandler<FlyoutAdicionaDescontoArgs> Retorno;

        public FlyoutAdicionaDescontoModel(DocumentoPagar documentoPagar)
        {
            _documentoPagar = documentoPagar;

            IncializaModel();
        }

        private void IncializaModel()
        {
            ValorOriginal = _documentoPagar.ValorOriginal;
            ValorAjustado = _documentoPagar.ValorAjustado;
            ValorQuitado = _documentoPagar.ValorQuitado;
            ValorJuros = _documentoPagar.Juros;
            DescontoDisponivel = _valorAjustado - _valorQuitado;
            _descontoDisponivelClone = DescontoDisponivel;
            _valorAjustadoClone = _valorAjustado;
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

        protected virtual void OnRetorno(FlyoutAdicionaDescontoArgs args)
        {
            Retorno?.Invoke(this, args);
        }

        public void Salvar()
        {
            if (_descontoDisponivel < 0)
            {
                throw new ArgumentException("O valor do desconto disponivel não pode ser menor que zero (0), obrigado");
            }

            OnRetorno(new FlyoutAdicionaDescontoArgs(Valor, Historico));
        }
    }
}