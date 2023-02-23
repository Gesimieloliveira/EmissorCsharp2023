using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;
using CentroDeCusto = FusionCore.FusionAdm.Financeiro.CentroCusto;

namespace Fusion.Visao.CentroCusto
{
    public class CentroCustoFormModel : ModelValidation
    {
        private readonly CentroDeCusto _centroCusto;
        private CentroDeCusto _itemSelecionado;
        private bool _camposEditaveis;
        public ObservableCollection<CentroDeCusto> Items { get; }

        public CentroDeCusto ItemSelecionado
        {
            get { return _itemSelecionado; }
            set
            {
                if (Equals(value, _itemSelecionado)) return;
                _itemSelecionado = value;
                PropriedadeAlterada();
            }
        }

        [Required(ErrorMessage = @"Porfavor digitar uma descrição")]
        public string Descricao
        {
            get { return GetValue(() => Descricao); }
            set { SetValue(value); }
        }

        public bool CamposEditaveis
        {
            get { return _camposEditaveis; }
            set
            {
                if (value == _camposEditaveis) return;
                _camposEditaveis = value;
                PropriedadeAlterada();
            }
        }

        public CentroCustoFormModel(CentroDeCusto centroCusto)
        {
            Items = new ObservableCollection<CentroDeCusto>();
            _centroCusto = centroCusto;

            AtualizaModel();
            PreencheLista();
        }

        private void PreencheLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCentroCusto(sessao);
                var items = repositorio.BuscaTodos();

                Items.Clear();
                items.ForEach(Items.Add);
            }
        }

        private void AtualizaModel()
        {
            ItemSelecionado = _centroCusto.CentroCustoPai;
            Descricao = _centroCusto.Descricao;
            CamposEditaveis = _centroCusto.Id == 0;
        }

        public void Salvar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCentroCusto(sessao);

                _centroCusto.CentroCustoPai = ItemSelecionado;
                _centroCusto.Descricao = Descricao;

                var centrosCustosPais = repositorio.ObterCategoriasPai();

                if(CamposEditaveis)
                    _centroCusto.GerarNivel((CentroDeCusto) centrosCustosPais.OrderByDescending(p => p.Ordenacao).FirstOrNull());

                sessao.Clear();
                repositorio.Salvar(_centroCusto);

                transacao.Commit();
            }
        }

        public void Deletar()
        {
            if (_centroCusto.Id == 0)
            {
                DialogBox.MostraInformacao("Não e possivel deletar, pois é um registro novo");
                return;
            }

            if (_centroCusto.Itens != null && _centroCusto.Itens.Count != 0)
            {
                DialogBox.MostraInformacao("Não e possivel deletar, pois contém dependencias (filhos)");
                return;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCentroCusto(sessao);

                repositorio.Deletar(_centroCusto);
                transacao.Commit();
            }

            DialogBox.MostraMensagemDeletouComSucesso();
        }
    }
}