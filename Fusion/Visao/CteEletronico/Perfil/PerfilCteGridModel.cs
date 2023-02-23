using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.CteEletronico;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Visao.CteEletronico.Perfil
{
    public class PerfilCteGridModel : GridPadraoModel<PerfilCteGrid>
    {
        private IList<PerfilCteGrid> _cache;

        public PerfilCteGridModel()
        {
            LabelPesquisaRapida = "Pesquisa por descrição";
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>
            {
                CriaColuna("Codigo/ID", new Binding("Id") {StringFormat = "D11"}, 100),
                CriaColuna("Descrição do Pefil", "Descricao", DataGridLength.Auto),
                CriaColuna("Tipo da CT-e", "TipoCte", DataGridLength.SizeToHeader),
                CriaColuna("Tipo de Serviço", "TipoServico", DataGridLength.Auto)
            };

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                Lista = _cache.ToList();
                return;
            }

            Lista = _cache.Where(p => p.Descricao.ToUpper().Contains(texto.ToUpper())).ToList();
        }

        public override Window JanelaNovo()
        {
            return new CtePerfilForm(new CtePerfilFormModel(new PerfilCte()));
        }

        public override Window JanelaAlterar()
        {
            return new CtePerfilForm(new CtePerfilFormModel(BuscarPerfilCte()));
        }

        public override void PopularLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilCte(sessao);

                _cache = repositorio.BuscaTodosParaGrid();

                Lista = _cache.ToList();
            }
        }

        private PerfilCte BuscarPerfilCte()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilCte(sessao);

                return repositorio.GetPeloId(Selecionado.Id);
            }
        }
    }
}