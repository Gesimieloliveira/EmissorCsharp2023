using System.Linq;
using System.Windows.Input;
using Fusion.Visao.Produto;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.TabelasPrecos
{
    public class NovoPrecoDiferenciadoItemFormularioModel : ViewModel
    {
        private readonly TabelaPreco _tabelaPreco;
        private AjusteDiferenciado _ajusteDiferenciado;
        private string _descricaoTabelaPreco;
        private decimal _percentualAjuste;
        private ProdutoDTO _produtoDTO;
        private decimal _precoAjustado;
        private bool _novoRegistro;

        public string DescricaoTabelaPreco
        {
            get => _descricaoTabelaPreco;
            set
            {
                _descricaoTabelaPreco = value;
                PropriedadeAlterada();
            }
        }

        public ProdutoDTO ProdutoDTO
        {
            get => _produtoDTO;
            set
            {
                _produtoDTO = value;
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
                CalculaPrecoAjustado();
            }
        }

        public decimal PrecoAjustado
        {
            get => _precoAjustado;
            set
            {
                _precoAjustado = value;
                PropriedadeAlterada();
            }
        }

        public NovoPrecoDiferenciadoItemFormularioModel(TabelaPreco tabelaPreco,
            AjusteDiferenciado ajusteDiferenciado = null)
        {
            _tabelaPreco = tabelaPreco;
            _ajusteDiferenciado = ajusteDiferenciado;
            DescricaoTabelaPreco = tabelaPreco.TipoAjustePreco.GetDescription();
            PercentualAjuste = tabelaPreco.PercentualAjuste;

            AtualizarAjusteDiferenciado();

            NovoRegistro = EUmNovoAjuste();
        }

        private void AtualizarAjusteDiferenciado()
        {
            if (EUmNovoAjuste()) return;

            _produtoDTO = _ajusteDiferenciado.Produto;
            _percentualAjuste = _ajusteDiferenciado.PercentualAjuste;
            _precoAjustado = _ajusteDiferenciado.NovoPreco;

            
            PropriedadeAlterada(nameof(ProdutoDTO));
            PropriedadeAlterada(nameof(PercentualAjuste));
            PropriedadeAlterada(nameof(PrecoAjustado));
        }

        public bool NovoRegistro
        {
            get => _novoRegistro;
            set
            {
                if (value == _novoRegistro) return;
                _novoRegistro = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscaProduto => GetSimpleCommand(BuscaProdutoAction);

        private void BuscaProdutoAction(object obj)
        {
            var pickerModel = new ProdutoGridPickerModel();

            pickerModel.PickItemEvent += (s, e) =>
            {
                ProdutoDTO = e.GetItem<ProdutoDTO>();

                CalculaPrecoAjustado();
            };

            pickerModel.GetPickerView().ShowDialog();
        }

        private void CalculaPrecoAjustado()
        {
            if (ProdutoDTO == null) return;

            PrecoAjustado = ProdutoDTO.PrecoVenda;

            var calculadora = FabricaCalculoPeloTipoAjuste.ObterCalculadoraDeAjuste(_tabelaPreco.TipoAjustePreco);

            PrecoAjustado = calculadora.Calcular(_produtoDTO, PercentualAjuste);
        }

        public void SalvarPrecoDiferenciado()
        {
            if (EUmNovoAjuste())
                _tabelaPreco.JaExisteProduto(_produtoDTO);

            var ajusteDiferenciado = new AjusteDiferenciado
            {
                Id = _ajusteDiferenciado?.Id ?? 0,
                PercentualAjuste = PercentualAjuste,
                Produto = ProdutoDTO,
                TabelaPreco = _tabelaPreco
            };

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioTabelaPreco = new RepositorioTabelaPreco(sessao);

                repositorioTabelaPreco.SalvaOuAtualiza(ajusteDiferenciado);

                transacao.Commit();
            }


            if (_tabelaPreco.AjusteDiferenciadoLista.Count != 0 
                && _ajusteDiferenciado != null)
            {
                _tabelaPreco.AjusteDiferenciadoLista.Remove(_ajusteDiferenciado);
            }

            _tabelaPreco.AjusteDiferenciadoLista.Add(ajusteDiferenciado);
            LimpaCampos();
        }

        private bool EUmNovoAjuste()
        {
            return _ajusteDiferenciado == null;
        }

        private void LimpaCampos()
        {
            ProdutoDTO = null;
            PercentualAjuste = _tabelaPreco.PercentualAjuste;
            PrecoAjustado = 0.0m;
            _ajusteDiferenciado = null;
        }

        public TabelaPreco ObterTabelaPreco()
        {
            return _tabelaPreco;
        }
    }
}