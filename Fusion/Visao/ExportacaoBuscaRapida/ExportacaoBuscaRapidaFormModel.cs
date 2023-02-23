using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.Exportacao.ItensBuscaRapida;
using FusionCore.Helpers.Maquina;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.ExportacaoBuscaRapida
{
    public class ExportacaoBuscaRapidaFormModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private readonly string _idMaquina = IdMaquinaProvider.Computa();
        public IList<string> Avisos = new List<string>();
        private ObservableCollection<Local> _arquivoLocais = new ObservableCollection<Local>();
        private CasasDecimais _casaDecimalSelecionada = CasasDecimais.Duas;

        public ExportacaoBuscaRapidaFormModel()
        {
            OpcoesExportacao = new List<OpcaoExportacaoBuscaRapida>();
            Preferencias = new List<PreferenciaBuscaRapida>();
        }

        private IList<PreferenciaBuscaRapida> Preferencias { get; }

        public IList<OpcaoExportacaoBuscaRapida> OpcoesExportacao
        {
            get => GetValue<IList<OpcaoExportacaoBuscaRapida>>();
            set => SetValue(value);
        }

        public ObservableCollection<Local> ArquivoLocais
        {
            get => _arquivoLocais;
            set
            {
                _arquivoLocais = value;
                PropriedadeAlterada();
            }
        }

        public OpcaoExportacaoBuscaRapida ExportacaoSelecionada
        {
            get => GetValue<OpcaoExportacaoBuscaRapida>();
            set => SetValue(value);
        }

        public CasasDecimais CasaDecimalSelecionada
        {
            get => _casaDecimalSelecionada;
            set
            {
                _casaDecimalSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public bool LocalExportacaoIsEnable
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void Inicializar()
        {
            LocalExportacaoIsEnable = false;
            OpcoesExportacao.Add(new OpcaoExportacaoBuscaRapida("Gertec", new LayoutBuscaRapidaGertec()));
        }

        public void ExportarParaLocal()
        {
            if (ExportacaoSelecionada == null)
            {
                throw new InvalidOperationException("Preciso que selecione um layout de exportação");
            }

            var exportador = new Exportador(_sessaoManager);
            var layout = ExportacaoSelecionada.LayoutBuscaRapida;

            var copias = ArquivoLocais.Select(x => x.Localizacao).ToList();

            layout.CasasDecimais = CasaDecimalSelecionada;

            exportador.Exportar(layout, copias);

            Avisos = exportador.Avisos;
        }

        public void AdicionaDestinoCopia(string fullName)
        {
            if (ExportacaoSelecionada == null)
            {
                throw new InvalidOperationException("Preciso que informe um layout antes de adicionar um local");
            }

            var preferencia = new PreferenciaBuscaRapida(_idMaquina, fullName, ExportacaoSelecionada.LayoutBuscaRapida.Tag);

            using (var sessao = _sessaoManager.CriaStatelessSession())
            {
                sessao.Insert(preferencia);
            }

            Preferencias.Add(preferencia);

            ArquivoLocais.Add(new Local(fullName, preferencia.Id));
        }

        public void RemoverLocal(Local local)
        {
            var preferencia = Preferencias.SingleOrDefault(x => x.Id == local.Id);

            using (var sessao = _sessaoManager.CriaStatelessSession())
            {
                sessao.Delete(preferencia);
            }

            ArquivoLocais.Remove(local);
            Preferencias.Remove(preferencia);
        }

        public void CarregarPreferencias()
        {
            Preferencias.Clear();

            if (ExportacaoSelecionada == null)
            {
                LocalExportacaoIsEnable = false;
                return;
            }

            using (var sessao = _sessaoManager.CriaStatelessSession())
            {

                var preferencias = sessao.QueryOver<PreferenciaBuscaRapida>()
                    .Where(i => i.Identificador == _idMaquina && i.Tag == ExportacaoSelecionada.LayoutBuscaRapida.Tag)
                    .List<PreferenciaBuscaRapida>();


                foreach (var pref in preferencias)
                {
                    Preferencias.Add(pref);
                    ArquivoLocais.Add(new Local(pref.LocalExportacao, pref.Id));
                }
            }

            LocalExportacaoIsEnable = true;
        }
    }
}