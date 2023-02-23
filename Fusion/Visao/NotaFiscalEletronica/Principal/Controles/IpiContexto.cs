using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Calculadoras;
using FusionCore.Tributacoes.Federal;
using FusionCore.Tributacoes.Repositorio;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Controles
{
    public class IpiContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private readonly CalculadoraIpi _calculadoraIpi = new CalculadoraIpi();
        private ItemNfe _item;

        public IEnumerable<TributacaoIpi> IpisDisponiveis
        {
            get => GetValue<IEnumerable<TributacaoIpi>>();
            set => SetValue(value);
        }

        public TributacaoIpi IpiSelecionado
        {
            get => GetValue<TributacaoIpi>();
            set => SetValue(value);
        }

        public decimal ValorTributavel
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalcularIpi();
            }
        }

        public decimal Aliquota
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalcularIpi();
            }
        }

        public decimal BaseCalculo
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorIpi
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public bool AutoAjusteImposto
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                CalcularIpi();
            }
        }

        public void Inicializar()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var ipis = new RepositorioIpi(sessao);
                IpisDisponiveis = ipis.TodosParaSaida();
            }
        }

        public void CarregarCom(ProdutoDTO produto, ItemContexto contexto)
        {
            SetValue(produto.SituacaoTributariaIpi, nameof(IpiSelecionado));
            SetValue(produto.AliquotaIpi, nameof(Aliquota));
            SetValue(contexto.Total, nameof(ValorTributavel));

            CalcularIpi();
        }

        private void CalcularIpi()
        {
            if (!AutoAjusteImposto)
            {
                return;
            }

            _calculadoraIpi.Aliquota = Aliquota;
            _calculadoraIpi.ValorTributavel = ValorTributavel;

            var res = _calculadoraIpi.Calcula();

            SetValue(res.Bc, nameof(BaseCalculo));
            SetValue(res.Valor, nameof(ValorIpi));
        }

        public void AplicarAlteracoesEm(ItemNfe item)
        {
            if (item.Ipi == null)
            {
                item.Ipi = new ImpostoIpi(item, IpiSelecionado);
            }

            item.Ipi.TributacaoIpi = IpiSelecionado;
            item.Ipi.AliquotaIpi = Aliquota;
            item.Ipi.ValorBcIpi = BaseCalculo;
            item.Ipi.ValorIpi = ValorIpi;
        }

        public void Com(ItemNfe item)
        {
            SetValue(item.Ipi.TributacaoIpi, nameof(IpiSelecionado));
            SetValue(item.Ipi.AliquotaIpi, nameof(Aliquota));
            SetValue(item.Ipi.ValorBcIpi, nameof(BaseCalculo));
            SetValue(item.Ipi.ValorIpi, nameof(ValorIpi));
            SetValue(item.AutoAjustarImposto, nameof(AutoAjusteImposto));

            _item = item;
        }

        public void ThrowExceptionSeInvalido()
        {
            if (IpiSelecionado == null) throw new InvalidOperationException("Preciso de um IPI");
            if (Aliquota < 0) throw new InvalidOperationException("Aliquota do IPI não pode ser negativa");
            if (ValorIpi < 0) throw new InvalidOperationException("Valor do IPI não pode ser negativo");
        }
    }
}