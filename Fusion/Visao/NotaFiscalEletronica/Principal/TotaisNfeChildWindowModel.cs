using System;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Principal
{
    public class TotaisNfeChildWindowModel : ViewModel
    {
        private readonly Nfeletronica _nfe;
        public EventHandler<Nfeletronica> AlteracoesSalva;

        public decimal ValorDesconto
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorFrete
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorSeguro
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorDespesas
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public TotaisNfeChildWindowModel(Nfeletronica nfe)
        {
            _nfe = nfe;

            ValorDespesas = _nfe.ValorDespesasFixa;
            ValorFrete = _nfe.ValorFreteFixo;
            ValorDesconto = _nfe.ValorDescontoFixo;
            ValorSeguro = _nfe.ValorSeguroFixo;
        }

        public void SalvarAlteracoes()
        {
            ThrowExceptionSeModelInvalido();

            _nfe.ValorSeguroFixo = ValorSeguro;
            _nfe.ValorDespesasFixa = ValorDespesas;
            _nfe.ValorFreteFixo = ValorFrete;
            _nfe.ValorDescontoFixo = ValorDesconto;

            _nfe.CalcularItens();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNfe(sessao);
                repositorio.SalvarAlteracoes(_nfe);

                transacao.Commit();
            }

            AlteracoesSalva?.Invoke(this, _nfe);
        }

        private void ThrowExceptionSeModelInvalido()
        {
            if (ValorDesconto < 0)
            {
                throw new InvalidOperationException("Valor do desconto não pode ser negativo");
            }

            if (ValorFrete < 0)
            {
                throw new InvalidOperationException("Valor do frete não pode ser negativo");
            }

            if (ValorSeguro < 0)
            {
                throw new InvalidOperationException("Valor do seguro não pode ser negativo");
            }

            if (ValorDespesas < 0)
            {
                throw new InvalidOperationException("Valor das despesas não pode ser negativo");
            }
        }
    }
}