using FusionCore.FusionAdm.Produtos;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Produto
{
    public sealed class RegraTribuacaoInfo : ViewModel
    {
        public EstadoDTO Uf
        {
            get { return GetValue<EstadoDTO>(); }
            set
            {
                SetValue(value);
                UnidadeFederativaId = value?.Id ?? 0;
            }
        }

        public int UnidadeFederativaId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public decimal Aliquota
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public decimal AliquotaFcp
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public ProdutoRegraTributacao RegraVinculada { get; set; }

        public RegraTribuacaoInfo()
        {
        }

        public RegraTribuacaoInfo(ProdutoRegraTributacao regra)
        {
            Uf = regra.Uf;
            Aliquota = regra.Aliquota;
            AliquotaFcp = regra.AliquotaFcp;
            RegraVinculada = regra;
        }

        public ProdutoRegraTributacao CriaRegra(ProdutoDTO produto)
        {
            return new ProdutoRegraTributacao(produto, Uf, Aliquota, AliquotaFcp);
        }
    }
}