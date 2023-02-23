using System;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Contratos;

namespace Fusion.Visao.TabelasPrecos
{
    public class Porcentagem
    {
        public Porcentagem(decimal valor)
        {
            Valor = valor;
        }

        public decimal Valor { get; }
    }

    public class CalculadoraAjusteItemContexto : ViewModel, IChildContext
    {
        public event EventHandler<Porcentagem> HandlerPorcentagemCalculada;
        private decimal _novoPrecoVenda;
        private decimal _percentualAjuste;
        private readonly ICalculoAjustePreco _calculadoraAjuste;

        public CalculadoraAjusteItemContexto(string titulo, decimal precoVenda, TipoAjustePreco tipoAjustePreco)
        {
            TituloChild = titulo;
            PrecoVenda = precoVenda;
            _calculadoraAjuste = FabricaCalculoPeloTipoAjuste.ObterCalculadoraDeAjuste(tipoAjustePreco);
        }

        public decimal PrecoVenda { get; }

        public string TituloChild { get; }
        public event EventHandler SolicitaFechamento;

        public decimal NovoPrecoVenda
        {
            get => _novoPrecoVenda;
            set
            {
                _novoPrecoVenda = value;
                PropriedadeAlterada();

                PercentualAjuste = _calculadoraAjuste.CalcularPercentualAjuste(value, PrecoVenda);
            }
        }

        public decimal PercentualAjuste
        {
            get => _percentualAjuste;
            set
            {
                _percentualAjuste = value;
                PropriedadeAlterada();
            }
        }

        public void OnHandlerPorcentagemCalculada()
        {
            _calculadoraAjuste.ThrowValidaCalcularPercentualAjuste(NovoPrecoVenda, PrecoVenda);
            HandlerPorcentagemCalculada?.Invoke(this, new Porcentagem(PercentualAjuste));
            OnFechar();
        }
    }
}