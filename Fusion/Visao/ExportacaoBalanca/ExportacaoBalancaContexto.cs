using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FusionCore.Exportacao.ItensBalanca;
using FusionCore.Helpers.Maquina;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.ExportacaoBalanca
{
    public class ExportacaoBalancaContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private readonly string _idMaquina = IdMaquinaProvider.Computa();
        public IList<string> Avisos = new List<string>();

        public ExportacaoBalancaContexto()
        {
            OpcoesExportacao = new List<OpcaoExportacao>();
            Preferencias = new ObservableCollection<PreferenciaExportacao>();
        }

        public IList<OpcaoExportacao> OpcoesExportacao
        {
            get => GetValue<IList<OpcaoExportacao>>();
            set => SetValue(value);
        }

        public ObservableCollection<PreferenciaExportacao> Preferencias
        {
            get => GetValue<ObservableCollection<PreferenciaExportacao>>();
            set => SetValue(value);
        }

        public OpcaoExportacao ExportacaoSelecionada
        {
            get => GetValue<OpcaoExportacao>();
            set => SetValue(value);
        }

        public bool LocalExportacaoIsEnable
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void Inicializar()
        {
            LocalExportacaoIsEnable = false;
            OpcoesExportacao.Add(new OpcaoExportacao("MGV6 - Filezolla/Toledo", new LayouotBalancaLinhaMgv6()));
        }

        public void ExportarParaLocal()
        {
            if (ExportacaoSelecionada == null)
            {
                throw new InvalidOperationException("Preciso que selecione um layout de exportação");
            }

            if (Preferencias.Count == 0)
            {
                throw new InvalidOperationException("Preciso que escolha no mínimo um local para a cópia de exportação");
            }

            var exportador = new Exportador(_sessaoManager);
            var layout = ExportacaoSelecionada.LayouotBalanca;
            var copias = new List<string>();

            foreach (var prf in Preferencias)
            {
                copias.Add(prf.LocalExportacao);
            }

            exportador.Exportar(layout, copias);

            Avisos = exportador.Avisos;
        }

        public void AdicionaDestinoCopia(string fullName)
        {
            if (ExportacaoSelecionada == null)
            {
                throw new InvalidOperationException("Preciso que informe um layout antes de adicionar um local");
            }

            var preferencia = new PreferenciaExportacao(_idMaquina, fullName, ExportacaoSelecionada.LayouotBalanca.Tag);

            using (var sessao = _sessaoManager.CriaStatelessSession())
            {
                sessao.Insert(preferencia);
            }

            Preferencias.Add(preferencia);
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
                var repositorio = new RepositorioExportacaoBalanca(sessao);

                var preferencias = repositorio.BuscaPreferencias(_idMaquina, ExportacaoSelecionada.LayouotBalanca.Tag);

                foreach (var pref in preferencias)
                {
                    Preferencias.Add(pref);
                }
            }

            LocalExportacaoIsEnable = true;
        }

        public void RemoverLocal(PreferenciaExportacao preferencia)
        {
            using (var sessao = _sessaoManager.CriaStatelessSession())
            {
                sessao.Delete(preferencia);
            }

            Preferencias.Remove(preferencia);
        }
    }
}