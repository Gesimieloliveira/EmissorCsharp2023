using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.CteEletronicoOs.Perfil;
using FusionCore.FusionAdm.Servico.CteEletronicoOs.Perfil;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Visao.CteEletronicoOs.Perfil.Grid
{
    public class GridPerfilCteOsModel : GridPadraoModel<PerfilCteOsGrid>
    {
        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = CriaDataGrid();

            colunas.Add(CriaColuna("Descrição", new Binding("Descricao")));

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            PesquisarPerfil();
        }

        public override Window JanelaNovo()
        {
            return new PerfilCteOsForm(new PerfilCteOsFormModel(new PerfilCteOs()));
        }

        public override Window JanelaAlterar()
        {
            return new PerfilCteOsForm(new PerfilCteOsFormModel(BuscarPor(Selecionado.Id)));
        }

        public override void PopularLista()
        {
            PesquisarPerfil();
        }

        private void PesquisarPerfil()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var servico = new ServicoPerfilCteOs(new RepositorioPerfilCteOs(sessao));
                Lista = servico.Buscar(UltimoTextoPesquisado);
            }
        }

        private static PerfilCteOs BuscarPor(int perfilId)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var servico = new ServicoPerfilCteOs(new RepositorioPerfilCteOs(sessao));
                return servico.Buscar(perfilId);
            }
        }
    }
}