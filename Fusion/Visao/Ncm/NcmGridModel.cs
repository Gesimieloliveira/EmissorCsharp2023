using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Ncm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.Ncm
{
    public class NcmGridModel : GridPadraoModel<NcmDTO>
    {
        private readonly RepositorioComun<NcmDTO> _repositorio;

        public NcmGridModel()
        {
            _repositorio = new RepositorioComun<NcmDTO>();
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var colunaCodigo = new DataGridTextColumn
            {
                Width = 100,
                Header = "Código NCM",
                Binding = new Binding("Id")
            };

            var colunaDescricao = new DataGridTextColumn
            {
                Header = "Descrição do ncm",
                Binding = new Binding("Descricao")
            };

            var colunaCest = new DataGridTextColumn
            {
                Width = 100,
                Header = "Cest",
                Binding = new Binding(nameof(NcmDTO.Cest))
            };

            colunas.Add(colunaCodigo);
            colunas.Add(colunaDescricao);
            colunas.Add(colunaCest);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            if (texto.IsNullOrEmpty())
            {
                PopularLista();
                return;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _repositorio.Sessao = sessao;
                Lista = _repositorio.Busca(new NcmBuscaRapida(texto));
            }
        }

        public override Window JanelaNovo()
        {
            return new NcmForm(new NcmDTO());
        }

        public override Window JanelaAlterar()
        {
            return new NcmForm(Selecionado);
        }

        public override void PopularLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _repositorio.Sessao = sessao;
                Lista = _repositorio.Busca(new TodosNcm());
            }
        }
    }
}