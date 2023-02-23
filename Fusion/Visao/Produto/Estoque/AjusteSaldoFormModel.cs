using System;
using Fusion.Sessao;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Estoque;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Produto.Estoque
{
    public class AjusteSaldoFormModel : ModelBase
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;

        private decimal _precoCompra;
        private decimal _precoVenda;
        private ProdutoEstoqueDTO _produtoEstoque;
        private decimal _quantidade;
        private TipoEventoEstoque _tipoEvento;

        public AjusteSaldoFormModel(ProdutoDTO produtoAdm, TipoEventoEstoque tipoEvento)
        {
            ProdutoAdm = produtoAdm;
            TipoEvento = tipoEvento;
        }

        public ProdutoEstoqueDTO ProdutoEstoque
        {
            get { return _produtoEstoque; }
            set
            {
                if (Equals(value, _produtoEstoque)) return;
                _produtoEstoque = value;
                PropriedadeAlterada();
            }
        }

        public ProdutoDTO ProdutoAdm { get; }

        public decimal PrecoCompra
        {
            get { return _precoCompra; }
            set
            {
                if (value == _precoCompra) return;
                _precoCompra = value;
                PropriedadeAlterada();
            }
        }

        public decimal PrecoVenda
        {
            get { return _precoVenda; }
            set
            {
                if (value == _precoVenda) return;
                _precoVenda = value;
                PropriedadeAlterada();
            }
        }

        public decimal Quantidade
        {
            get { return _quantidade; }
            set
            {
                if (value == _quantidade) return;
                _quantidade = value;
                PropriedadeAlterada();
            }
        }

        public TipoEventoEstoque TipoEvento
        {
            get { return _tipoEvento; }
            set
            {
                if (value == _tipoEvento) return;
                _tipoEvento = value;
                PropriedadeAlterada();
            }
        }

        public void PreencherForm()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                PrecoCompra = ProdutoAdm.PrecoCompra;
                PrecoVenda = ProdutoAdm.PrecoVenda;

                var repositorio = new RepositorioComun<ProdutoEstoqueDTO>(sessao);
                ProdutoEstoque = repositorio.Busca(new EstoquePeloProduto(ProdutoAdm));
            }
        }

        public void SalvarForm()
        {
            if (PrecoVenda <= 0)
                throw new InvalidOperationException("Preço de venda precisa ser maior que zero (0).");
            if (Quantidade <= 0)
                throw new InvalidOperationException("Quantidade precisa ser maior que zero (0)");

            ProdutoAdm.PrecoCompra = PrecoCompra;
            ProdutoAdm.PrecoVenda = PrecoVenda;
            ProdutoAdm.AlteradoEm = DateTime.Now;

            SalvarEmBancoDeDados();
        }

        private void SalvarEmBancoDeDados()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var transacao = sessao.BeginTransaction();

                try
                {
                    var repositorioProduto = new RepositorioComun<ProdutoDTO>(sessao);
                    repositorioProduto.Mescla(ProdutoAdm);

                    var estoqueModel = new EstoqueModel
                    {
                        Produto = ProdutoAdm,
                        OrigemEvento = OrigemEventoEstoque.AjusteAvulso,
                        Usuario = _sessaoSistema.UsuarioLogado,
                        Quantidade = Quantidade
                    };

                    var estoqueServico = EstoqueServicoAdmFactory.Cria(sessao);

                    if (Equals(TipoEvento, TipoEventoEstoque.Entrada))
                        estoqueServico.Acrescentar(estoqueModel);
                    if (Equals(TipoEvento, TipoEventoEstoque.Saida))
                        estoqueServico.Descontar(estoqueModel);

                    transacao.Commit();
                }
                catch (Exception)
                {
                    transacao.Rollback();
                    throw;
                }
            }
        }
    }
}