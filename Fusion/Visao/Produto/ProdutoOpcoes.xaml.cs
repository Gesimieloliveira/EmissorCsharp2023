using System.Windows;
using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Sessao;
using Fusion.Visao.EventoEstoque;
using Fusion.Visao.Produto.Estoque;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using FusionCore.Sessao;

namespace Fusion.Visao.Produto
{
    public partial class ProdutoOpcoes
    {
        private ProdutoDTO _produtoAdm;
        private readonly UsuarioDTO _usuarioLogado;

        public ProdutoOpcoes()
        {
            InitializeComponent();

            _usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
        }

        private void ClickBotaoAcrescentarEstoque(object sender, RoutedEventArgs e)
        {
            _usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.CADASTRO_PRODUTO_ACRESCENTAR_ESTOQUE_AVULSO);

            var formModel = new AjusteSaldoFormModel(_produtoAdm, TipoEventoEstoque.Entrada);
            var form = new AjusteSaldoForm(formModel);
            form.ShowDialog();
            DialogResult = true;
            Close();
        }

        private void ClickBotaoDescontarEstoque(object sender, RoutedEventArgs e)
        {
            _usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.CADASTRO_PRODUTO_DESCONTAR_ESTOQUE_AVULSO);

            var formModel = new AjusteSaldoFormModel(_produtoAdm, TipoEventoEstoque.Saida);
            var form = new AjusteSaldoForm(formModel);
            form.ShowDialog();
            DialogResult = true;
            Close();
        }

        private void ClickBotaoEventosDeEstoque(object sender, RoutedEventArgs e)
        {
            var janela = new EventoEstoqueWindow(_produtoAdm);
            janela.ShowDialog();
            DialogResult = true;
            Close();
        }

        private void ClickBotaoEditar(object sender, RoutedEventArgs e)
        {
            _usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.CADASTRO_PRODUTO_INSERIR_ALTERAR);

            var model = new ProdutoFormModel(_produtoAdm.Id);
            var form = new ProdutoForm(model);

            form.ShowDialog();
            DialogResult = true;

            Close();
        }

        public void CarregarCom(ProdutoGrid produtoGrid)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);
                _produtoAdm = repositorio.GetPeloId(produtoGrid.Id);
            }
        }

        private void ImprimirEtiquetaClickHandler(object sender, RoutedEventArgs e)
        {
            Close();

            using (var r = new RModeloEtiqueta(new SessaoManagerAdm()))
            {
                r.ComParametroId(_produtoAdm.Id);
                r.Visualizar();
            }
        }
    }
}