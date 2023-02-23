using System.Collections.ObjectModel;
using System.Windows.Input;
using Fusion.Sessao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Pessoa
{
    public class PessoaGridModel : ViewModel
    {
        private bool _isPessoaInserir;
        private bool _isPessoaListar;
        public FiltroPessoaGrid Filtro { get; set; } = new FiltroPessoaGrid();

        public ObservableCollection<PessoaGrid> Pessoas
        {
            get => GetValue<ObservableCollection<PessoaGrid>>();
            set => SetValue(value);
        }

        public PessoaGrid Selecionado
        {
            get => GetValue<PessoaGrid>();
            set => SetValue(value);
        }

        public ICommand PesquisaCommand => GetSimpleCommand(o => AplicarPesquisa());

        public void Inicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var pessoas = repositorio.BuscaPessoasGridModel(Filtro);

                Pessoas = new ObservableCollection<PessoaGrid>(pessoas);
            }

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            IsPessoaInserir = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CADASTRO_PESSOA_INSERIR_ALTERAR);
            IsPessoaAlterar = IsPessoaInserir;
            IsPessoaVisualizar = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CADASTRO_PESSOA_VISUALIZAR);
            IsPessoaListar = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CADASTRO_PESSOA_LISTAR);
        }

        public bool IsPessoaListar
        {
            get => _isPessoaListar;
            set
            {
                if (value == _isPessoaListar) return;
                _isPessoaListar = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPessoaVisualizar { get; set; }

        public bool IsPessoaAlterar { get; set; }

        public bool IsPessoaInserir
        {
            get => _isPessoaInserir;
            set
            {
                if (value == _isPessoaInserir) return;
                _isPessoaInserir = value;
                PropriedadeAlterada();
            }
        }

        public void AplicarPesquisa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var lista = repositorio.BuscaPessoasGridModel(Filtro);

                Pessoas = new ObservableCollection<PessoaGrid>(lista);
            }
        }

        public void NovaPessoa()
        {
            var formModel = new PessoaFormModel();
            var form = new PessoaForm(formModel);

            form.ShowDialog();
            AplicarPesquisa();
        }

        public void AlterarSelecionado()
        {
            if (IsPessoaVisualizar == false) return;

            var formModel = new PessoaFormModel(Selecionado.Id);
            var form = new PessoaForm(formModel);

            form.ShowDialog();
            AplicarPesquisa();
        }
    }
}