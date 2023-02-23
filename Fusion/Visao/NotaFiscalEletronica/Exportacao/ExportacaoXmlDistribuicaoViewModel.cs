using System.Collections.ObjectModel;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using System.Linq;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionCore.Core.Flags;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using FusionLibrary.Helper.Diversos;
using System.Windows.Input;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;
using FusionCore.ExportacaoPacote;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.GerenciarManifestacoesEletronicas;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ConfiguracaoEmail;
using Application = System.Windows.Application;

namespace Fusion.Visao.NotaFiscalEletronica.Exportacao
{
    public class ExportacaoXmlDistribuicaoViewModel : ViewModel
    {
        public ICommand ChoseArquivoCommand => GetSimpleCommand(ChoseArquivoAction);
        public ICommand ExportacaoCommand => GetSimpleCommand(ExportacaoAction);

        public ExportacaoXmlDistribuicaoViewModel()
        {
            Mes = (Mes)DateTime.Now.Month;
        }

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

        public string AssuntoEmail
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

        public Mes Mes
        {
            get => GetValue<Mes>();
            set
            {
                SetValue(value);
                var mes = (int)value;
                var dataPrimeiroDia = new DateTime(DateTime.Now.Year, mes, 1);

                FiltroDataInicio = dataPrimeiroDia.PrimeiroDiaDoMesAtual();
                FiltroDataFinal = dataPrimeiroDia.UltimoDiaDoMesAtual();
            }
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

        private void ChoseArquivoAction(object obj)
        {
            var dialog = new SaveFileDialog { Filter = @"Arquivo Zipado (.zip)|*.zip" };
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

        private async Task ProcessaExportacaoAsync()
        {
            ProgressBarAgil4.ShowProgressBar();

                await Task.Run(() =>
                {
                    try
                    {
                        using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                        {
                            var repositorio = new RepositorioDistribuicaoDFe(sessao);
                            var exportador = new ExportadorXml(repositorio, TipoDocumentoFiscalEletronico.NotaFiscalCompra);

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