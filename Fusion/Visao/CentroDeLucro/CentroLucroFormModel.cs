using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.CentroDeLucro
{
    public class CentroLucroFormModel : ModelValidation
    {
        private readonly CentroLucro _centroLucro;
        private bool _camposEditaveis;
        public ObservableCollection<CentroLucro> Items { get; }

        private CentroLucro _itemSelecionado;

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

        public CentroLucro ItemSelecionado
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
            set
            {
                SetValue(value);
            }
        }

        public CentroLucroFormModel(CentroLucro centroLucro)
        {
            Items = new ObservableCollection<CentroLucro>();
            _centroLucro = centroLucro;

            AtualizaModel();
            PreencheLista();
        }

        private void PreencheLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCentroLucro(sessao);
                var items = repositorio.BuscaTodos();

                Items.Clear();
                items.ForEach(Items.Add);
            }
        }

        private void AtualizaModel()
        {
            ItemSelecionado = _centroLucro.CentroLucroPai;
            Descricao = _centroLucro.Descricao;
            CamposEditaveis = _centroLucro.Id == 0;
        }

        public void Salvar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCentroLucro(sessao);

                _centroLucro.CentroLucroPai = ItemSelecionado;
                _centroLucro.Descricao = Descricao;

                var centrosCustosPais = repositorio.ObterCategoriasPai();

                if (CamposEditaveis)
                    _centroLucro.GerarNivel((CentroLucro)centrosCustosPais.OrderByDescending(p => p.Ordenacao).FirstOrNull());

                sessao.Clear();
                repositorio.Salvar(_centroLucro);

                transacao.Commit();
                DialogBox.MostraMensagemSalvouComSucesso();
            }
        }

        public void Deletar()
        {
            if (_centroLucro.Id == 0)
            {
                DialogBox.MostraInformacao("Não e possivel deletar, pois é um registro novo");
                return;
            }

            if (_centroLucro.Itens != null && _centroLucro.Itens.Count != 0)
            {
                DialogBox.MostraInformacao("Não e possivel deletar, pois contém dependencias (filhos)");
                return;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCentroLucro(sessao);

                repositorio.Deletar(_centroLucro);
                transacao.Commit();
            }

            DialogBox.MostraMensagemDeletouComSucesso();
        }
    }
}