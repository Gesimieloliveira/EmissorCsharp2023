using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.RecipienteDados;
using FusionCore.RecipienteDados.Adm.Impl;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Federal;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Produto.Models
{
    internal class ProdutoIpiModel : ViewModel
    {
        private readonly RecipienteIpi _recipienteIpi = RecipienteFactory.Get<RecipienteIpi>();

        public ObservableCollection<TributacaoIpi> Tributacoes
        {
            get => GetValue<ObservableCollection<TributacaoIpi>>();
            set => SetValue(value);
        }

        public TributacaoIpi TributacaoIpi
        {
            get => GetValue<TributacaoIpi>();
            set => SetValue(value);
        }

        public decimal Aliquota
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public ProdutoIpiModel()
        {
            Tributacoes = new ObservableCollection<TributacaoIpi>(_recipienteIpi.GetPorOperacao(TipoOperacao.Saida));
        }

        public void PreencherComProduto(ProdutoDTO produto)
        {
            TributacaoIpi = produto.SituacaoTributariaIpi;
            Aliquota = produto.AliquotaIpi;

            if (TributacaoIpi == null)
            {
                TributacaoIpi = Tributacoes.FirstOrDefault(i => i.Codigo == "99");
            }
        }
    }
}
