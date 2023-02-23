using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Regras;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Produto.Models
{
    internal class ProdutoIcmsModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;

        public ProdutoIcmsModel(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public IEnumerable<RegraTributacaoSaidaSlim> RegrasDisponiveis
        {
            get => GetValue<IEnumerable<RegraTributacaoSaidaSlim>>();
            set => SetValue(value);
        }

        public RegraTributacaoSaidaSlim RegraSelecionada
        {
            get => GetValue<RegraTributacaoSaidaSlim>();
            set
            {
                SetValue(value);
                ProcessaPermissoesCst();
            }
        }

        public decimal Aliquota
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal Reducao
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal Mva
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public bool PermiteAliquota
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool PermiteReducao
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool PermiteMva
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void Inicializa()
        {
            CarregaRegrasTributacao();
        }

        public void CarregaRegrasTributacao()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                RegrasDisponiveis = new RepositorioRegraTributacao(sessao).ListaRegrasAtivas();
            }
        }

        public void PreencherComProduto(ProdutoDTO produto)
        {
            RegraSelecionada = RegraTributacaoSaidaSlim.From(produto.RegraTributacaoSaida);
            Aliquota = produto.AliquotaIcms;
            Reducao = produto.ReducaoIcms;
            Mva = produto.PercentualMva;

            if (RegraSelecionada != null && RegrasDisponiveis.All(r => r != RegraSelecionada))
            {
                var novasRegras = new List<RegraTributacaoSaidaSlim>(RegrasDisponiveis)
                {
                    RegraSelecionada
                };

                RegrasDisponiveis = novasRegras.OrderBy(i => i.Descricao);
            }

            ProcessaPermissoesCst();
        }

        private void ProcessaPermissoesCst()
        {
            if (string.IsNullOrWhiteSpace(RegraSelecionada?.Cst))
            {
                PermiteAliquota = false;
                PermiteMva = false;
                PermiteReducao = false;

                return;
            }

            var permissao = new PermissaoCst(RegraSelecionada.Cst);

            PermiteAliquota = permissao.TemIcms() || permissao.TemIcmsSt();
            PermiteReducao = permissao.TemReducao() || permissao.TemReducaoSt();
            PermiteMva = permissao.TemMva();
        }
    }
}
