using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fusion.Sessao;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Sessao;
using FusionCore.GerenciarManifestacoesEletronicas;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace Fusion.Visao.GerenciadorManifestacoesDestinatarios
{
    public class EfetuarDistribuicaoModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;

        public EfetuarDistribuicaoModel()
        {
            _sessaoManager = SessaoSistema.Instancia.SessaoManager;
            ResultadoDocumentosProcessados = new List<string>();
            ListaEmissorFiscal = new ObservableCollection<EmissorFiscalComboBox>();
            EmissorFiscal = null;
        }

        public IList<string> ResultadoDocumentosProcessados
        {
            get => GetValue<IList<string>>();
            set => SetValue(value);
        }

        public ObservableCollection<EmissorFiscalComboBox> ListaEmissorFiscal
        {
            get => GetValue<ObservableCollection<EmissorFiscalComboBox>>();
            set => SetValue(value);
        }

        public EmissorFiscalComboBox EmissorFiscal
        {
            get => GetValue<EmissorFiscalComboBox>();
            set
            {
                SetValue(value);
                CarregarUltimaConsulta();
            }
        }

        public bool UsarNsuZero
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public MdeConsulta UltimaConsulta
        {
            get => GetValue<MdeConsulta>();
            private set => SetValue(value);
        }

        public DateTime? ProximaConsulta
        {
            get => GetValue<DateTime?>();
            private set => SetValue(value);
        }

        public bool PodeConsultar
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public void Inicializar()
        {
            ListaEmissorFiscal = BuscarListaEmissorFiscal();
            EmissorFiscal = ListaEmissorFiscal.FirstOrNull() as EmissorFiscalComboBox;
        }

        private ObservableCollection<EmissorFiscalComboBox> BuscarListaEmissorFiscal()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                var emissores = repositorio.BuscaTodosQueSejamNfeParaComboBox();

                return new ObservableCollection<EmissorFiscalComboBox>(emissores);
            }
        }

        public void EfetuarDistribuicao()
        {
            var emissor = ObterEmissor();

            var servico = new MdeServico(emissor, SessaoSistema.Instancia.SessaoManager)
            {
                ForcarNsuZero = UsarNsuZero
            };

            servico.ConsultarDocumentos();
            var logs = servico.ProcessarDocumentos();

            ResultadoDocumentosProcessados = logs;
            CarregarUltimaConsulta();
        }

        private EmissorFiscalNFE ObterEmissor()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                return new RepositorioEmissorFiscal(sessao)
                    .GetPeloId(EmissorFiscal.Id)
                    .EmissorFiscalNfe;
            }
        }

        private void CarregarUltimaConsulta()
        {
            if (EmissorFiscal == null)
            {
                UltimaConsulta = null;
                PodeConsultar = true;
                return;
            }

            var emissor = ObterEmissor();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioDistribuicaoDFe(sessao);

                var ultima = repositorio.BuscarUltimaConsulta(
                    emissor.EmissorFiscal.Empresa.DocumentoUnico,
                    emissor.Ambiente,
                    emissor.EmissorFiscal.Uf
                );

                UltimaConsulta = ultima;
                ProximaConsulta = ultima?.DataResposta.AddMinutes(65) ?? DateTime.Now;
                PodeConsultar = DateTime.Now >= ProximaConsulta;
            }
        }
    }
}