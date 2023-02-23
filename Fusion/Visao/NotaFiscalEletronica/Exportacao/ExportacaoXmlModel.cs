using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Fusion.Visao.NotaFiscalEletronica.Infraestrutura;
using FusionCore.Core.Flags;
using FusionCore.ExportacaoPacote;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ConfiguracaoEmail;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate;
using Application = System.Windows.Application;

namespace Fusion.Visao.NotaFiscalEletronica.Exportacao
{
    public class ExportacaoXmlModel : ViewModel
    {
        public ICommand ChoseArquivoCommand => GetSimpleCommand(ChoseArquivoAction);
        public ICommand ExportacaoCommand => GetSimpleCommand(ExportacaoAction);

        public string CaminhoArquivo
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string EmailDestino
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime FiltroDataInicio
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime FiltroDataFinal
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public string AssuntoEmail
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public TipoDocumentoFiscalEletronicoSelecao DocumentoFiscalEletronico
        {
            get => GetValue<TipoDocumentoFiscalEletronicoSelecao>();
            set => SetValue(value);
        }

        public ObservableCollection<EmpresaDTO> ListaDeEmpresas
        {
            get => GetValue<ObservableCollection<EmpresaDTO>>();
            set => SetValue(value);
        }

        public EmpresaDTO EmpresaSelecionada
        {
            get => GetValue<EmpresaDTO>();
            set => SetValue(value);
        }

        private bool TemEmailDestino => !string.IsNullOrWhiteSpace(EmailDestino);
        private bool TemCaminhoArquivo => !string.IsNullOrWhiteSpace(CaminhoArquivo);

        public Mes Mes
        {
            get => GetValue<Mes>();
            set
            {
                SetValue(value);
                var mes = (int) value;
                var dataPrimeiroDia = new DateTime(DateTime.Now.Year, mes, 1);

                FiltroDataInicio = dataPrimeiroDia.PrimeiroDiaDoMesAtual();
                FiltroDataFinal = dataPrimeiroDia.UltimoDiaDoMesAtual();
            } 
        }

        public ExportacaoXmlModel()
        {
            Mes = (Mes)DateTime.Now.Month;
            DocumentoFiscalEletronico = (TipoDocumentoFiscalEletronicoSelecao)TipoDocumentoFiscalEletronico.NFe;
        }

        public void Inicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmpresa(sessao);
                var empresas = repositorio.BuscaTodos();

                ListaDeEmpresas = new ObservableCollection<EmpresaDTO>(empresas);
                EmpresaSelecionada = ListaDeEmpresas.FirstOrDefault();
            }
        }

        private void ChoseArquivoAction(object obj)
        {
            var dialog = new SaveFileDialog {Filter = @"Arquivo Zipado (.zip)|*.zip"};
            dialog.ShowDialog();
            CaminhoArquivo = dialog.FileName;
        }

        private async void ExportacaoAction(object obj)
        {
            try
            {
                if (EmpresaSelecionada == null)
                {
                    throw new InvalidOperationException("Preciso que escolha qual empresa exportar.");
                }

                ThrowExceptionSeNenhumaOpcaoSelecionada();
                ThrowExceptionSeAssuntoVazio();

                await ProcessaExportacaoAsync();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void ThrowExceptionSeAssuntoVazio()
        {
            if (!TemEmailDestino)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(AssuntoEmail))
                throw new InvalidOperationException("Necessário informar um assunto válido");
        }

        private void ThrowExceptionSeNenhumaOpcaoSelecionada()
        {
            if (TemCaminhoArquivo)
                return;

            if (TemEmailDestino)
                return;

            throw new InvalidOperationException("Necessário informar no mínimo uma opção para exportação");
        }

        private async Task ProcessaExportacaoAsync()
        {
            ProgressBarAgil4.ShowProgressBar();

            await Task.Run(() =>
            {
                try
                {
                    using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                    {
                        var repositorio = CriaRepositorioAdequado(sessao);
                        var exportador = new ExportadorXml(repositorio, (TipoDocumentoFiscalEletronico)DocumentoFiscalEletronico);

                        exportador.Filtrar(FiltroDataInicio, FiltroDataFinal, EmpresaSelecionada);
                        exportador.GeraPacote();

                        if (TemCaminhoArquivo)
                        {
                            exportador.ArmazenaZipEmDisco(CaminhoArquivo);
                        }

                        if (TemEmailDestino)
                        {
                            EnviaPacotePorEmail(exportador);
                        }
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraInformacao("Documentos foram exportados/enviados com sucesso");
                    });
                }
                catch (InvalidOperationException e)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraAviso(e.Message);
                    });
                }
                catch (Exception e)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraErro(e.Message, e);
                    });
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
                }
            });
        }

        private IRepositorioExportacaoXml CriaRepositorioAdequado(ISession sessao)
        {
            switch (DocumentoFiscalEletronico)
            {
                case TipoDocumentoFiscalEletronicoSelecao.NFe:
                    return new RepositorioNfe(sessao);
                case TipoDocumentoFiscalEletronicoSelecao.NFCe:
                    return new RepositorioExportaXmlNfce(sessao);
                case TipoDocumentoFiscalEletronicoSelecao.CTe:
                    return new RepositorioCte(sessao);
                case TipoDocumentoFiscalEletronicoSelecao.MDFe:
                    return new RepositorioMdfe(sessao);
                case TipoDocumentoFiscalEletronicoSelecao.SAT:
                    return new RepositorioSatAdm(sessao);
                case TipoDocumentoFiscalEletronicoSelecao.CTeOs:
                    return new RepositorioCteOs(sessao);
                default:
                    throw new InvalidOperationException("Não foi possivel determinar o documento a exportar");
            }
        }

        private void EnviaPacotePorEmail(ExportadorXml exportador)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<ConfiguracaoEmailDTO>(sessao);
                var config = repositorio.Busca(new UnicaConfiguracaoEmail());

                if (config == null)
                {
                    throw new InvalidOperationException("Ops, servidor para envio dos e-mails não foi configurado");
                }

                exportador.SetConfiguracaoEmail(config);
                exportador.EnviaPorEmail(EmailDestino, AssuntoEmail);
            }
        }
    }
}