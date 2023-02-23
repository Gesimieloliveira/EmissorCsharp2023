using System;
using System.Collections.Generic;
using FusionCore.Core.Flags;
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
    public class PisCofinsContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private readonly CalculadoraPis _calculadoraPis = new CalculadoraPis();
        private readonly CalculadoraCofins _calculadoraCofins = new CalculadoraCofins();
        private ItemNfe _item;

        public decimal ValorTributavel
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarImpostoCofins();
                AjustarImpostoPis();
            }
        }

        public IEnumerable<TributacaoPis> PisDisponiveis
        {
            get => GetValue<IEnumerable<TributacaoPis>>();
            set => SetValue(value);
        }

        public TributacaoPis PisSelecionado
        {
            get => GetValue<TributacaoPis>();
            set => SetValue(value);
        }

        public IEnumerable<TributacaoCofins> CofinsDisponiveis
        {
            get => GetValue<IEnumerable<TributacaoCofins>>();
            set => SetValue(value);
        }

        public TributacaoCofins CofinsSelecionado
        {
            get => GetValue<TributacaoCofins>();
            set => SetValue(value);
        }

        public decimal AliquotaPis
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarImpostoPis();
            }
        }

        public decimal BaseCalculoPis
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorPis
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal AliquotoCofins
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarImpostoCofins();
            }
        }

        public decimal BaseCalculoCofins
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorCofins
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
                AjustarImpostoPis();
                AjustarImpostoCofins();
            }
        }

        public void Inicializar()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var pis = new RepositorioPis(sessao);
                PisDisponiveis = pis.TodosParaNfe(TipoOperacao.Saida);

                var cofins = new RepositorioCofins(sessao);
                CofinsDisponiveis = cofins.TodosParaNfe(TipoOperacao.Saida);
            }
        }

        public void CarregarCom(ProdutoDTO produto, ItemContexto contexto)
        {
            SetValue(contexto.Total, nameof(ValorTributavel));

            SetValue(produto.Pis, nameof(PisSelecionado));
            SetValue(produto.AliquotaPis, nameof(AliquotaPis));
            AjustarImpostoPis();

            SetValue(produto.Cofins, nameof(CofinsSelecionado));
            SetValue(produto.AliquotaCofins, nameof(AliquotoCofins));
            AjustarImpostoCofins();
        }

        private void AjustarImpostoPis()
        {
            if (!AutoAjusteImposto)
            {
                return;
            }

            _calculadoraPis.Aliquota = AliquotaPis;
            _calculadoraPis.ValorTributavel = ValorTributavel;

            var res = _calculadoraPis.Calcula();

            SetValue(res.Bc, nameof(BaseCalculoPis));
            SetValue(res.Valor, nameof(ValorPis));
        }

        private void AjustarImpostoCofins()
        {
            if (!AutoAjusteImposto)
            {
                return;
            }

            _calculadoraCofins.Aliquota = AliquotoCofins;
            _calculadoraCofins.ValorTributavel = ValorTributavel;

            var res = _calculadoraCofins.Calcula();

            SetValue(res.Bc, nameof(BaseCalculoCofins));
            SetValue(res.Valor, nameof(ValorCofins));
        }

        public void AplicarAlteracoesEm(ItemNfe item)
        {
            if (item.Pis == null)
            {
                item.Pis = new ImpostoPis(item);
            }

            item.Pis.Cst = PisSelecionado;
            item.Pis.AliquotaPis = AliquotaPis;
            item.Pis.ValorBcPis = BaseCalculoPis;
            item.Pis.ValorPis = ValorPis;

            if (item.Cofins == null)
            {
                item.Cofins = new ImpostoCofins(item);
            }

            item.Cofins.Cst = CofinsSelecionado;
            item.Cofins.AliquotaCofins = AliquotoCofins;
            item.Cofins.ValorBcCofins = BaseCalculoCofins;
            item.Cofins.ValorCofins = ValorCofins;
        }

        public void Com(ItemNfe item)
        {
            SetValue(item.Pis.Cst, nameof(PisSelecionado));
            SetValue(item.Pis.AliquotaPis, nameof(AliquotaPis));
            SetValue(item.Pis.ValorBcPis, nameof(BaseCalculoPis));
            SetValue(item.Pis.ValorPis, nameof(ValorPis));

            SetValue(item.Cofins.Cst, nameof(CofinsSelecionado));
            SetValue(item.Cofins.AliquotaCofins, nameof(AliquotoCofins));
            SetValue(item.Cofins.ValorBcCofins, nameof(BaseCalculoCofins));
            SetValue(item.Cofins.ValorCofins, nameof(ValorCofins));

            _item = item;
        }

        public void ThrowExceptionSeInvalido()
        {
            if (PisSelecionado == null) throw new InvalidOperationException("Preciso que escolha um PIS");
            if (CofinsDisponiveis == null) throw new InvalidOperationException("Preciso que escolha um COFINS");
            if (AliquotaPis < 0) throw new InvalidOperationException("Aliquota PIS não pode ser negativa");
            if (AliquotoCofins < 0) throw new InvalidOperationException("Aliquota Cofins não pode ser negativa");
        }
    }
}