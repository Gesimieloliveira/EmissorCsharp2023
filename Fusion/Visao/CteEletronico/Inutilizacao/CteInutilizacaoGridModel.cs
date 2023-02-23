using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Visao.CteEletronico.Inutilizacao
{
    public class CteInutilizacaoGridModel : GridPadraoModel<CteInutilizacaoDTO>
    {
        private IList<CteInutilizacaoDTO> _cache;

        public CteInutilizacaoGridModel()
        {
            LabelPesquisaRapida = "Pesquisa por Justificativa";
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            return new ObservableCollection<DataGridColumn>
            {
                CriaColuna("Codigo/ID", new Binding("Id") {StringFormat = "D11"}, 100),
                CriaColuna("Documento", "Documento", 120),
                CriaColuna("Numero Inicial", "NumeroInicial", 120),
                CriaColuna("Numero Final", "NumeroFinal", 120),
                CriaColuna("Justificativa", "Justificativa")
            };
        }

        public override void AplicarPesquisa(string texto)
        {
            if (texto.IsNullOrEmpty())
            {
                PopularLista();
                return;
            }

            var query = from i in _cache
                where i.Justificativa.ToUpper().Contains(texto.ToUpper())
                select i;

            Lista = query.ToList();
        }

        public override Window JanelaNovo()
        {
            return new CteInutilizacaoForm();
        }

        public override Window JanelaAlterar()
        {
            throw new ArgumentException("Não tem a opção de alterar");
        }

        public override void PopularLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCte(sessao);

                _cache = repositorio.BuscarInutilizacoes();

                Lista = _cache;
            }
        }
    }
}