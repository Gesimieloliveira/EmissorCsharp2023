using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.EstoqueEvento;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.EventoEstoque
{
    public class EventoEstoqueGridModel : GridPadraoModel<EstoqueEventoDTO>
    {
        private readonly RepositorioComun<EstoqueEventoDTO> _repositorio;

        public EventoEstoqueGridModel()
        {
            EditaRegistro = false;
            MostraBotaoNovo = false;
            MostraBotaoOpcoes = false;

            _repositorio = new RepositorioComun<EstoqueEventoDTO>();
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>
            {
                CriaColuna("Nome do produto", new Binding("ProdutoDTO.Nome")),
                CriaColuna("Detalhe da origem", new Binding("OrigemEventoDetalhe")),
                CriaColuna("Tipo", new Binding("TipoEventoTexto"), 120),
                CriaColuna("Estoque", new Binding("EstoqueAtual") {StringFormat = "N2"}, 120),
                CriaColuna("Movimento", new Binding("Movimento") {StringFormat = "N2"}, 120),
                CriaColuna("Saldo", new Binding("EstoqueFuturo") {StringFormat = "N2"}, 120)
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
                    var dataAtual = DateTime.Now;
                    var data30DiasAtras = dataAtual.AddDays(-30);
                    Lista = _repositorio.Busca(new EventosPorPeriodo(data30DiasAtras));
                    return;
                }

                Lista = _repositorio.Busca(new TodosEventos());
            }
        }

        public override Window JanelaNovo()
        {
            throw new InvalidOperationException("Não permite novo evento de estoque");
        }

        public override Window JanelaAlterar()
        {
            throw new InvalidOperationException("Não permite alterações no evento de estoque");
        }

        public override void PopularLista()
        {
            AplicarPesquisa(string.Empty);
        }
    }
}