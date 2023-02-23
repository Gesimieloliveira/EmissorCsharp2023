using System;
using System.Linq;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.VisaoModel;
using NHibernate;

namespace FusionNfce.Visao.Principal.ConsultaProdutoRapida
{
    public class ConsultaProdutoRapidaFormModel : ViewModel
    {
        private string _codigoBarra;
        private ProdutoNfce _produto;
        private readonly TabelaPrecoNfce _tabelaPrecoNfce;
        private readonly AtualizaPrecoProdutoTabelaPreco _atualizaPrecoProdutoTabelaPreco;

        public ConsultaProdutoRapidaFormModel(TabelaPrecoNfce tabelaPrecoNfce)
        {
            _tabelaPrecoNfce = tabelaPrecoNfce;
            _atualizaPrecoProdutoTabelaPreco = new AtualizaPrecoProdutoTabelaPreco(_tabelaPrecoNfce);
        }

        public string CodigoBarra
        {
            get => _codigoBarra;
            set
            {
                if (value == _codigoBarra) return;
                _codigoBarra = value;
                PropriedadeAlterada();
            }
        }

        public ProdutoNfce Produto
        {
            get => _produto;
            set
            {
                if (Equals(value, _produto)) return;
                _produto = value;
                PropriedadeAlterada();
            }
        }

        public void BuscarProduto()
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

            using (sessao)
            {
                var repositorioProduto = new RepositorioProdutoNfce(sessao);
                var produtos = repositorioProduto.BuscaPorCodigoOuCodigoBarras(CodigoBarra);

                if (produtos.Count == 0)
                {
                    throw new InvalidOperationException("Não encontrei produto para esse código");
                }

                if (produtos.Count > 1)
                {
                    throw new InvalidOperationException(
                        "Encontrei mais de 1 produto para esse código, preciso que acesse o administrador e corrija.");
                }

                var produto = produtos.FirstOrDefault();
                NHibernateUtil.Initialize(produto.ProdutosAlias);


                Produto = _atualizaPrecoProdutoTabelaPreco.CalculaProdutoComBaseTabelaPreco(new RepositorioTabelaPrecoNfce(sessao), produto) as ProdutoNfce;
            }
        }
    }
}