using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fusion.Sessao;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Produto
{
    public class ProdutoGridModel : ViewModel
    {
        private bool _isProdutoInserirAlterar;
        private bool _isProdutoListar;
        private FiltroProdutoGridControl _filtro = new FiltroProdutoGridControl();

        public ProdutoGridModel()
        {
            Produtos = new ObservableCollection<ProdutoGrid>();

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
            IsProdutoInserirAlterar = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CADASTRO_PRODUTO_INSERIR_ALTERAR);
            IsProdutoVisualizar = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CADASTRO_PRODUTO_VISUALIZAR);
        }

        public FiltroProdutoGridControl Filtro
        {
            get => _filtro;
            set
            {
                _filtro = value;
                PropriedadeAlterada();
            }
        }

        public IList<ProdutoGrupoDTO> Grupos
        {
            get => CarregarGrupos();
        }

        private bool IsProdutoVisualizar { get; set; }

        public bool IsProdutoInserirAlterar
        {
            get => _isProdutoInserirAlterar;
            set
            {
                if (value == _isProdutoInserirAlterar) return;
                _isProdutoInserirAlterar = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<ProdutoGrid> Produtos
        {
            get => GetValue<ObservableCollection<ProdutoGrid>>();
            set => SetValue(value);
        }

        public ProdutoGrid Selecionado
        {
            get => GetValue<ProdutoGrid>();
            set => SetValue(value);
        }

        public void Inicializar()
        {
            AplicaPesquisa();
        }

        public void AplicaPesquisa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioProduto = new RepositorioProduto(sessao);
                Produtos = new ObservableCollection<ProdutoGrid>(repositorioProduto.BuscaParaGridComFiltro(_filtro));
            }
        }

        public void OpcoesProduto()
        {
            var janela = new ProdutoOpcoes();

            janela.CarregarCom(Selecionado);
            janela.ShowDialog();

            AplicaPesquisa();
        }

        public void AlteraSelecionado()
        {
            if (IsProdutoVisualizar == false) return;
            var formModel = new ProdutoFormModel(Selecionado.Id);
            new ProdutoForm(formModel).ShowDialog();

            AplicaPesquisa();
        }

        public void NovoProduto()
        {
            var formModel = new ProdutoFormModel();
            new ProdutoForm(formModel).ShowDialog();

            AplicaPesquisa();
        }

        public IList<ProdutoGrupoDTO> CarregarGrupos()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var grupos = new RepositorioGrupoProduto(sessao);
                
                var listaGrupos = grupos.BuscaTodos();

                return listaGrupos;
            }
        }
    }
}