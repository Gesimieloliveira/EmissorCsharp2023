using System.Collections.ObjectModel;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.TabelasPrecos
{
    public class TabelaPrecosListagemModel : ViewModel
    {
        private ObservableCollection<TabelaPreco> _tabelas = new ObservableCollection<TabelaPreco>();
        private string _descricao;
        private TabelaPreco _tabelaSelecionada;

        public TabelaPreco TabelaSelecionada
        {
            get => _tabelaSelecionada;
            set
            {
                _tabelaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<TabelaPreco> Tabelas
        {
            get => _tabelas;
            set
            {
                _tabelas = value;
                PropriedadeAlterada();
            }
        }

        public string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value;
                PropriedadeAlterada();
            }
        }

        public TabelaPrecosListagemModel()
        {
            Inicializa();
        }

        private void Inicializa()
        {
            AplicaPesquisa();
        }

        public void AplicaPesquisa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                Tabelas = new ObservableCollection<TabelaPreco>(new RepositorioTabelaPreco(sessao).PesquisarTabelasDePrecos(Descricao));
            }
        }

        public TabelaPreco ObtemTabelaPreco()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioTabelaPreco(sessao).GetPeloId(TabelaSelecionada.Id);
            }
        }
    }
}