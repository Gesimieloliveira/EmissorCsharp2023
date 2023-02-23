using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.UF;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.EstadoUf
{
    public class UfGridModel : GridPadraoModel<EstadoDTO>
    {
        private readonly RepositorioComun<EstadoDTO> _repositorio;

        public UfGridModel()
        {
            _repositorio = new RepositorioComun<EstadoDTO>();
            MostraBotaoNovo = false;
            EditaRegistro = false;
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var sigla = new DataGridTextColumn {Header = "Sigla", Binding = new Binding("Sigla"), Width = 100};
            colunas.Add(sigla);

            var nome = new DataGridTextColumn {Header = "Nome do estado", Binding = new Binding("Nome")};
            colunas.Add(nome);

            var codigoIbge = new DataGridTextColumn
            {
                Header = "Código ibge",
                Binding = new Binding("CodigoIbge"),
                Width = 100
            };
            colunas.Add(codigoIbge);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _repositorio.Sessao = sessao;
                Lista = _repositorio.Busca(new BuscaRapidaDeUF(texto));
            }
        }

        public override Window JanelaNovo()
        {
            return null;
        }

        public override Window JanelaAlterar()
        {
            return null;
        }

        public override void PopularLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _repositorio.Sessao = sessao;
                Lista = _repositorio.Busca(new TodosUF());
            }
        }
    }
}