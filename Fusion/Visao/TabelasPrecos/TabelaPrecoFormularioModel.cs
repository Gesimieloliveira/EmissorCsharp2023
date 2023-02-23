using System;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.TabelasPrecos
{
    public class TabelaPrecoFormularioModel : ViewModel
    {
        private string _descricao;
        private TipoAjustePreco _tipoAjustePreco;
        private decimal _percentualAjuste;
        private bool _apenasItensDaLista;
        private TabelaPreco _tabelaPreco;
        private AjusteDiferenciado _ajusteDiferenciadoSelecionada;
        private ObservableCollection<AjusteDiferenciado> _ajusteDiferenciadoListagem = new ObservableCollection<AjusteDiferenciado>();
        private bool _isPodeEditar;
        private bool _status = true;
        private string _nomeParaPesquisa;

        public TabelaPrecoFormularioModel(TabelaPreco tabelaPreco)
        {
            _tabelaPreco = tabelaPreco;

            Edicao();
        }

        private void Edicao()
        {
            AtualizaListagem();

            if (_tabelaPreco.Id == 0) return;

            Descricao = _tabelaPreco.Descricao;
            PercentualAjuste = _tabelaPreco.PercentualAjuste;
            ApenasItensDaLista = _tabelaPreco.ApenasItensDaLista;
            TipoAjustePreco = _tabelaPreco.TipoAjustePreco;
            Status = _tabelaPreco.Status;
        }

        public bool IsPodeEditar
        {
            get => _isPodeEditar;
            set
            {
                _isPodeEditar = value;
                PropriedadeAlterada();
            }
        }

        public AjusteDiferenciado AjusteDiferenciadoSelecionada
        {
            get => _ajusteDiferenciadoSelecionada;
            set
            {
                _ajusteDiferenciadoSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<AjusteDiferenciado> AjusteDiferenciadoListagem
        {
            get => _ajusteDiferenciadoListagem;
            set
            {
                _ajusteDiferenciadoListagem = value;
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

        public TabelaPreco TabelaPreco
        {
            get => _tabelaPreco;
            set
            {
                _tabelaPreco = value;
                PropriedadeAlterada();
            }
        }

        public TipoAjustePreco TipoAjustePreco
        {
            get => _tipoAjustePreco;
            set
            {
                _tipoAjustePreco = value;
                PropriedadeAlterada();
            }
        }

        public decimal PercentualAjuste
        {
            get => _percentualAjuste;
            set
            {
                _percentualAjuste = value;
                PropriedadeAlterada();
            }
        }

        public bool ApenasItensDaLista
        {
            get => _apenasItensDaLista;
            set
            {
                _apenasItensDaLista = value;
                PropriedadeAlterada();
            }
        }

        public bool Status
        {
            get => _status;
            set
            {
                _status = value;
                PropriedadeAlterada();
            }
        }

        public string NomeParaPesquisa
        {
            get => _nomeParaPesquisa;
            set
            {
                _nomeParaPesquisa = value;
                PropriedadeAlterada();
            }
        }

        public void Salvar()
        {
            _tabelaPreco.Descricao = Descricao.TrimOrEmpty();
            _tabelaPreco.PercentualAjuste = PercentualAjuste;
            _tabelaPreco.TipoAjustePreco = TipoAjustePreco;
            _tabelaPreco.ApenasItensDaLista = ApenasItensDaLista;
            _tabelaPreco.Status = Status;

            if (_tabelaPreco.Descricao.Length == 0)
                throw new InvalidOperationException("Preciso da Descrição da tabela de preços");

            new SalvarTabelaPreco().Salvar(_tabelaPreco);
        }

        public void TabelaPrecoEstaSalva()
        {
            if (_tabelaPreco.Id != 0) return;

            throw new InvalidOperationException("Você precisa salvar a tabela de preço antes");
        }

        public TabelaPreco ObterTabelaPreco() => _tabelaPreco;

        public void ExcluirAjusteDiferenciado()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioTabelaPreco = new RepositorioTabelaPreco(sessao);

                repositorioTabelaPreco.Remover(AjusteDiferenciadoSelecionada);

                transacao.Commit();
            }


            TabelaPreco.AjusteDiferenciadoLista.Remove(AjusteDiferenciadoSelecionada);

            Salvar();
            AtualizaListagem();
        }

        public void AtualizaListagem()
        {
            AjusteDiferenciadoListagem = new ObservableCollection<AjusteDiferenciado>(_tabelaPreco.AjusteDiferenciadoLista);

            IsPodeEditar = AjusteDiferenciadoListagem.Count == 0;
        }

        public void PesquisarProdutos(string textoParaPesquisa)
        {
            AjusteDiferenciadoListagem = new ObservableCollection<AjusteDiferenciado>(
                _tabelaPreco.AjusteDiferenciadoLista.Where(a => a.Produto.Nome.Contains(textoParaPesquisa)));

            IsPodeEditar = AjusteDiferenciadoListagem.Count == 0;
        }
    }
}