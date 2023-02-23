using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Visao.EmissorFiscalEletronico
{
    public class EmissorFiscalGridModel : GridPadraoModel<EmissorFiscalVo>
    {
        public EmissorFiscalGridModel()
        {
            MostraPesquisaRapida = false;
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var id = new DataGridTextColumn {Header = "Id", Binding = new Binding(nameof(EmissorFiscalVo.Id)), Width = 100};
            colunas.Add(id);

            var descricao = new DataGridTextColumn {Header = "Descrição", Binding = new Binding(nameof(EmissorFiscalVo.Descricao))};
            colunas.Add(descricao);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                var emissores = repositorio.BuscaVo();

                Lista = new List<EmissorFiscalVo>(emissores);
            }
        }

        public override Window JanelaNovo()
        {
            return new EmissorFiscalForm(EmissorFiscalForm.CriaModel());
        }

        public override Window JanelaAlterar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var emissor = new RepositorioEmissorFiscal(sessao).GetPeloId(Selecionado.Id);

                if (emissor.EmissorFiscalNfe == null) emissor.EmissorFiscalNfe = new EmissorFiscalNFE();
                if (emissor.EmissorFiscalNfce == null) emissor.EmissorFiscalNfce = new EmissorFiscalNFCE();
                if (emissor.EmissorFiscalCte == null) emissor.EmissorFiscalCte = new EmissorFiscalCTE();
                if (emissor.EmissorFiscalCteOs == null) emissor.EmissorFiscalCteOs = new EmissorFiscalCTeOS();
                if (emissor.EmissorFiscalMdfe == null) emissor.EmissorFiscalMdfe = new EmissorFiscalMDFE();
                if (emissor.EmissorFiscalSat == null) emissor.EmissorFiscalSat = new EmissorFiscalSAT();
                if (emissor.AutorizadoBaixarXml == null) emissor.AutorizadoBaixarXml = new AutorizadoBaixarXml();

                return new EmissorFiscalForm(EmissorFiscalForm.CriaModel(emissor));
            }
        }

        public override void PopularLista()
        {
            AplicarPesquisa(string.Empty);
        }
    }
}