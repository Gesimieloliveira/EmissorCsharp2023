using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Ecf;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Wpf.Conversores;

namespace Fusion.Visao.Ecf
{
    public class EcfGridModel : GridPadraoModel<PdvEcfDTO>
    {
        private readonly RepositorioComun<PdvEcfDTO> _repositorio;

        public EcfGridModel()
        {
            _repositorio = new RepositorioComun<PdvEcfDTO>();
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>
            {
                CriaColuna("Id", new Binding("Id"), 80),
                CriaColuna("Ativo", new Binding("Ativo") {Converter = new BooleanToSnConverter()}, 90),
                CriaColuna("Numero", new Binding("NumeroEcf")),
                CriaColuna("Serie", new Binding("Serie")),
                CriaColuna("Modelo", new Binding("Modelo"))
            };

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _repositorio.Sessao = sessao;

                if (string.IsNullOrEmpty(texto))
                {
                    Lista = _repositorio.Busca(new TodosEcf());
                    return;
                }

                Lista = _repositorio.Busca(new BuscaRapidaEcf(texto));
            }
        }

        public override Window JanelaNovo()
        {
            var model = new EcfFormModel(new PdvEcfDTO());
            return new EcfForm(model);
        }

        public override Window JanelaAlterar()
        {
            var model = new EcfFormModel(Selecionado);
            return new EcfForm(model);
        }

        public override void PopularLista()
        {
            AplicarPesquisa(string.Empty);
        }
    }
}