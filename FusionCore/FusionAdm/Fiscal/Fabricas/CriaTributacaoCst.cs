using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Repositorio.FusionNfce;

namespace FusionCore.FusionAdm.Fiscal.Fabricas
{
    public static class CriaTributacaoCst
    {
        public static INfceImpostoIcms CriaCsosnNfce(string cst, ProdutoNfce produto)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var tributacaoCstNfce = new RepositorioTributacaoCstNfce(sessao).BuscarPorId(cst);

                return CsosnFactory.CriaNfce(tributacaoCstNfce, produto.AliquotaIcms, produto.ReducaoIcms);
            }
        }

        public static NfceImpostoPis CriaPisNfce(ProdutoNfce produto)
        {
            return new NfceImpostoPis
            {
                Aliquota = produto.AliquotaPis,
                Pis = produto.Pis
            };
        }

        public static NfceImpostoCofins CriaCofinsNfce(ProdutoNfce produto)
        {
            return new NfceImpostoCofins
            {
                Aliquota = produto.AliquotaCofins,
                Cofins = produto.Cofins
            };
        }
    }
}