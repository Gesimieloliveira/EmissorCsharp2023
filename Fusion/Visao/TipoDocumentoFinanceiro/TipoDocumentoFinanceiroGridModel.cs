using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.Wpf.Conversores;

namespace Fusion.Visao.TipoDocumentoFinanceiro
{
    public class TipoDocumentoFinanceiroGridModel : GridPadraoModel<TipoDocumento>
    {
        private IList<TipoDocumento> _cache;

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>
            {
                CriaColuna("Descrição", new Binding("Descricao")),
                CriaColuna("Ativo", new Binding("EstaAtivo") {Converter = new BooleanToSnConverter()})
            };
            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                Lista.Clear();
                Lista = _cache.ToList();
                return;
            }

            var lista = _cache.Where(td => td.Descricao.ToUpper().Contains(texto.ToUpper())).ToList();
            Lista.Clear();
            Lista = lista;
        }

        public override Window JanelaNovo()
        {
            return new TipoDocumentoFinanceiroForm(new TipoDocumento());
        }

        public override Window JanelaAlterar()
        {
            return new TipoDocumentoFinanceiroForm(BuscarTipoDocumentoPeloId());
        }

        public override void PopularLista()
        {
            Lista = new List<TipoDocumento>();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTipoDocumento(sessao);
                _cache = repositorio.BuscarTipoDocumentoGrid();
            }

            Lista = _cache.ToList();
        }

        private TipoDocumento BuscarTipoDocumentoPeloId()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTipoDocumento(sessao);
                return repositorio.GetPeloId(Selecionado.Id);
            }
        }
    }
}