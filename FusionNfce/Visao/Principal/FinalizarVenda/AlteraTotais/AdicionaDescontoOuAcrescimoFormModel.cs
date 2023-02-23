using System;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Contratos;

namespace FusionNfce.Visao.Principal.FinalizarVenda.AlteraTotais
{
    public class AdicionaDescontoOuAcrescimoFormModel : ViewModel, IChildContext
    {
        public event EventHandler<AdicionaDescontoOuAcrescimoFormModel> EnviarDescontoOuAcrescimo; 

        public AdicionaDescontoOuAcrescimoFormModel(decimal totalRestante)
        {
            _totalRestante = totalRestante;
            TotalTexto = $"Total Restante {totalRestante}";
            TotalCalculado = totalRestante;
        }

        private decimal _valor;
        private decimal _acrescimoFixo;
        private decimal _acrescimoPercentual;
        private decimal _descontoFixo;
        private decimal _descontoPercentual;
        private decimal _totalCalculado;
        private readonly decimal _totalRestante;
        private string _totalTexto;
        public string TituloChild { get; } = "Aplicar Desconto ou Acréscimo";

        public decimal AcrescimoFixo
        {
            get => _acrescimoFixo;
            set
            {
                if (value == _acrescimoFixo) return;
                _acrescimoFixo = value;
                PropriedadeAlterada();

                CalculaAcrescimoFixo();
            }
        }

        private void CalculaAcrescimoFixo()
        {
            _valor = 0;
            LimpaAcrescimoPercentualSilenciosamente();
            LimpaDescontoFixoSilenciosamente();
            LimpaDescontoPercentualSilenciosamente();
            TotalCalculado = _totalRestante;

            _valor = AcrescimoFixo.Arredonda(2);
            TotalCalculado = (_totalRestante + _valor).Arredonda(2);
        }

        public decimal AcrescimoPercentual
        {
            get => _acrescimoPercentual;
            set
            {
                if (value == _acrescimoPercentual) return;
                _acrescimoPercentual = value;
                PropriedadeAlterada();

                CalculaAcrescimoPercentual();
            }
        }

        private void CalculaAcrescimoPercentual()
        {
            _valor = 0;
            LimpaAcrescimoFixoSilenciosamente();
            LimpaDescontoFixoSilenciosamente();
            LimpaDescontoPercentualSilenciosamente();
            TotalCalculado = _totalRestante;

            _valor = (_totalRestante * AcrescimoPercentual / 100).Arredonda(2);
            TotalCalculado = (_totalRestante + _valor).Arredonda(2);
        }

        public decimal DescontoFixo
        {
            get => _descontoFixo;
            set
            {
                if (value == _descontoFixo) return;
                _descontoFixo = value;
                PropriedadeAlterada();

                CalculaDescontoFixo();
            }
        }

        private void CalculaDescontoFixo()
        {
            _valor = 0;
            LimpaAcrescimoFixoSilenciosamente();
            LimpaAcrescimoPercentualSilenciosamente();
            LimpaDescontoPercentualSilenciosamente();
            TotalCalculado = _totalRestante;

            _valor = DescontoFixo.Arredonda(2);
            TotalCalculado = (_totalRestante - _valor).Arredonda(2);
        }

        public decimal DescontoPercentual
        {
            get => _descontoPercentual;
            set
            {
                if (value == _descontoPercentual) return;
                _descontoPercentual = value;
                PropriedadeAlterada();

                CalculaDescontoPercentual();
            }
        }

        private void CalculaDescontoPercentual()
        {
            _valor = 0;
            LimpaAcrescimoFixoSilenciosamente();
            LimpaAcrescimoPercentualSilenciosamente();
            LimpaDescontoFixoSilenciosamente();
            TotalCalculado = _totalRestante;

            _valor = (_totalRestante * DescontoPercentual / 100).Arredonda(2);
            TotalCalculado = (_totalRestante - _valor).Arredonda(2);
        }

        public decimal TotalCalculado
        {
            get => _totalCalculado;
            set
            {
                if (value == _totalCalculado) return;
                _totalCalculado = value;
                PropriedadeAlterada();
            }
        }

        public string TotalTexto
        {
            get => _totalTexto;
            set
            {
                if (value == _totalTexto) return;
                _totalTexto = value;
                PropriedadeAlterada();
            }
        }

        private void LimpaDescontoPercentualSilenciosamente()
        {
            _descontoPercentual = 0;
            PropriedadeAlterada(nameof(DescontoPercentual));
        }

        private void LimpaDescontoFixoSilenciosamente()
        {
            _descontoFixo = 0;
            PropriedadeAlterada(nameof(DescontoFixo));
        }

        private void LimpaAcrescimoFixoSilenciosamente()
        {
            _acrescimoFixo = 0;
            PropriedadeAlterada(nameof(AcrescimoFixo));
        }

        private void LimpaAcrescimoPercentualSilenciosamente()
        {
            _acrescimoPercentual = 0;
            PropriedadeAlterada(nameof(AcrescimoPercentual));
        }

        public event EventHandler SolicitaFechamento;

        public virtual void OnEnviarDescontoOuAcrescimo()
        {
            EnviarDescontoOuAcrescimo?.Invoke(this, this);
        }

        public decimal ObterValor()
        {
            return _valor;
        }

        public bool EUmAcrescimo()
        {
            return AcrescimoFixo != 0 || AcrescimoPercentual != 0;
        }

        public bool EUmDesconto()
        {
            return DescontoFixo != 0 || DescontoPercentual != 0;
        }
    }
}