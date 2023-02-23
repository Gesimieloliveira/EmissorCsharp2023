using System;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model
{
    public class SalvarValePedagioEventArgs : EventArgs
    {
        public SalvarValePedagioEventArgs(FlyoutAddValePedagioModel model)
        {
            Model = model;
        }

        public FlyoutAddValePedagioModel Model { get; set; }
    }

    public class FlyoutAddValePedagioModel : ViewModel
    {
        private bool _isOpen;
        private string _cnpjEmpresaFornecedora;
        private string _cnpjResposavelPagamento;
        private string _numeroComprovante;
        private string _cpfResposavelPagamento;
        private decimal _valor;

        public event EventHandler<SalvarValePedagioEventArgs> SalvarValePedagioHandler; 

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

        public string CnpjEmpresaFornecedora
        {
            get { return _cnpjEmpresaFornecedora; }
            set
            {
                if (value == _cnpjEmpresaFornecedora) return;
                _cnpjEmpresaFornecedora = value;
                PropriedadeAlterada();
            }
        }

        public string CnpjResposavelPagamento
        {
            get { return _cnpjResposavelPagamento; }
            set
            {
                if (value == _cnpjResposavelPagamento) return;
                _cnpjResposavelPagamento = value;
                PropriedadeAlterada();
            }
        }

        public string CpfResposavelPagamento
        {
            get { return _cpfResposavelPagamento; }
            set
            {
                if (value == _cpfResposavelPagamento) return;
                _cpfResposavelPagamento = value;
                PropriedadeAlterada();
            }
        }

        public decimal Valor
        {
            get { return _valor; }
            set
            {
                if (value == _valor) return;
                _valor = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroComprovante
        {
            get { return _numeroComprovante; }
            set
            {
                if (value == _numeroComprovante) return;
                _numeroComprovante = value;
                PropriedadeAlterada();
            }
        }

        public void LimpaCampos()
        {
            CnpjEmpresaFornecedora = string.Empty;
            CnpjResposavelPagamento = string.Empty;
            NumeroComprovante = string.Empty;
            Valor = 0.0m;
            CnpjResposavelPagamento = string.Empty;
            CpfResposavelPagamento = string.Empty;
        }

        public void SalvarValePedagio()
        {
            Hidrata();
            Validar();
            OnSalvarValePedagioHandler();
            LimpaCampos();
        }

        private void Hidrata()
        {
            CnpjEmpresaFornecedora = CnpjEmpresaFornecedora.TrimOrEmpty();
            CnpjResposavelPagamento = CnpjResposavelPagamento.TrimOrEmpty();
            NumeroComprovante = NumeroComprovante.TrimOrEmpty();
            CpfResposavelPagamento = CpfResposavelPagamento.TrimOrEmpty();
            Valor = decimal.Parse(Valor.ToString("N2"));
        }

        private void Validar()
        {
            if (CnpjEmpresaFornecedora.IsNullOrEmpty()) throw new ArgumentException("Cnpj Empresa Fornecedora é obrigatório");
            if (NumeroComprovante.IsNullOrEmpty()) throw new ArgumentException("Número do comprovante é obrigatório");

            if (CnpjResposavelPagamento.IsNotNullOrEmpty() && CpfResposavelPagamento.IsNotNullOrEmpty())
                throw new ArgumentException("Digitar somente o Cnpj do responsável ou Cpf do responsável, apenas um dos dois");
        }

        protected virtual void OnSalvarValePedagioHandler()
        {
            SalvarValePedagioHandler?.Invoke(this, new SalvarValePedagioEventArgs(this));
        }
    }
}