using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.NfeInutilizacaoNumeracao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Helpers;

namespace Fusion.Visao.NfeInutilizacaoNumeracao
{
    public class NfeInutilizacaoNumeracaoGridModel : GridPadraoModel<NfeInutilizacaoNumeracaoDTO>
    {
        private IList<NfeInutilizacaoNumeracaoDTO> _listaCache; 

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var id = new DataGridTextColumn
            {
                Width = 100,
                Header = "Codigo/ID",
                Binding = new Binding("Id") { StringFormat = "D11"}
            };

            var serie = new DataGridTextColumn
            {
                Width = 130,
                Header = "Série",
                Binding = new Binding(nameof(NfeInutilizacaoNumeracaoDTO.Serie)) { StringFormat = "D3" }
            };

            var numInicial = new DataGridTextColumn
            {
                Width = 130,
                Header = "Número Inicial",
                Binding = new Binding(nameof(NfeInutilizacaoNumeracaoDTO.NumeroInicial)) { StringFormat = "D8" }
            };

            var numeroFinal = new DataGridTextColumn
            {
                Width = 130,
                Header = "Número Inicial",
                Binding = new Binding(nameof(NfeInutilizacaoNumeracaoDTO.NumeroFinal)) { StringFormat = "D8" }
            };

            var protocolo = new DataGridTextColumn
            {
                Width = 130,
                Header = "Protocolo",
                Binding = new Binding(nameof(NfeInutilizacaoNumeracaoDTO.Protocolo))
            };

            var justificativa = new DataGridTextColumn
            {
                Header = "Justificativa",
                Binding = new Binding("Justificativa")
            };

            DataGridColumnHelper.SetAlign(serie, HorizontalAlignment.Center);
            DataGridColumnHelper.SetAlign(numInicial, HorizontalAlignment.Center);
            DataGridColumnHelper.SetAlign(numeroFinal, HorizontalAlignment.Center);
            DataGridColumnHelper.SetAlign(protocolo, HorizontalAlignment.Center);

            colunas.Add(id);
            colunas.Add(serie);
            colunas.Add(numInicial);
            colunas.Add(numeroFinal);
            colunas.Add(protocolo);
            colunas.Add(justificativa);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            Lista = _listaCache.Where(i => i.Justificativa.Contains(texto) || i.Id.ToString().Equals(texto)).ToList();
        }

        public override Window JanelaNovo()
        {
            return new NfeInutilizacaoNumeracaoForm(new NfeInutilizacaoNumeracaoDTO());
        }

        public override Window JanelaAlterar()
        {
            return new NfeInutilizacaoNumeracaoForm(Selecionado);
        }

        public override void PopularLista()
        {
            using (var repositorio = new RepositorioComun<NfeInutilizacaoNumeracaoDTO>(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                _listaCache = repositorio.Busca(new TodasNfeInutilizacaoNumeracao());
            }

            Lista = _listaCache;
        }
    }
}
