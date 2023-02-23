using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace FusionPdv.Visao.Produto
{
    public class ConsultarProdutoModel : ModelBase
    {
        private string _filtroPorNome;
        private IList<ProdutoDt> _listaDeProduto;
        private ProdutoDt _produtoSelecionado;

        public IList<ProdutoDt> ListaDeProduto
        {
            get { return _listaDeProduto; }

            set
            {
                _listaDeProduto = FiltraApenasAtivos(value);
                PropriedadeAlterada();
            }
        }

        public string FiltroPorNome
        {
            get { return _filtroPorNome; }

            set
            {
                _filtroPorNome = value;
                PropriedadeAlterada();
            }
        }

        public ProdutoDt ProdutoSelecionado
        {
            get { return _produtoSelecionado; }
            set
            {
                _produtoSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ProdutoDt PrimeiroItemDaLista => (ProdutoDt) _listaDeProduto.FirstOrNull();

        private static IList<ProdutoDt> FiltraApenasAtivos(IList<ProdutoDt> value)
        {
            // ReSharper disable once UseNullPropagation
            if (value == null)
                return null;

            var filtro = value.Where(p => p.Ativo == IntBinario.Sim);
            return filtro.ToList();
        }

        public void ConsultarProdutoPorNome()
        {
            try
            {
                using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
                {
                    ListaDeProduto = new ProdutoRepositorio(sessao).BuscarProdutos(_filtroPorNome ?? "");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Falha ao buscar o produto, tente novamente.", ex);
            }
        }
    }
}