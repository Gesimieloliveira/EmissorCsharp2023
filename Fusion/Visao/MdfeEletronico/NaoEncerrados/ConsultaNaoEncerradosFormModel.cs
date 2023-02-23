using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.MdfeEletronico.Emissao;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Extencoes;
using FusionCore.FusionAdm.MdfeEletronico.Factory;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;
using MDFe.Classes.Informacoes.Evento.Flags;
using MDFe.Classes.Retorno.MDFeConsultaNaoEncerrado;
using MDFe.Classes.Retorno.MDFeEvento;
using MDFe.Servicos.ConsultaNaoEncerradosMDFe;
using MDFe.Servicos.EventosMDFe;
using MDFeEvento = FusionCore.FusionAdm.MdfeEletronico.MDFeEvento;
using ZeusMDFe = MDFe.Classes.Informacoes.MDFe;

namespace Fusion.Visao.MdfeEletronico.NaoEncerrados
{
    public class MDFeNaoEncerradoGrid
    {
        public string Chave { get; set; }
        public string Protocolo { get; set; }
        public string NumeroFiscal { get; set; }
    }

    public class ConsultaNaoEncerradosFormModel : ViewModel
    {
        private ObservableCollection<MDFeNaoEncerradoGrid> _listaMdFeNaoEncerrado;
        private MDFeNaoEncerradoGrid _itemSelecionadoMdFeNaoEncerrado;
        private ObservableCollection<EmissorFiscal> _listaEmissorFiscal;
        private EmissorFiscal _emissorSelecionado;
        private string _resultadoTexto;

        public ObservableCollection<MDFeNaoEncerradoGrid> ListaMDFeNaoEncerrado
        {
            get => _listaMdFeNaoEncerrado;
            set
            {
                _listaMdFeNaoEncerrado = value;
                PropriedadeAlterada();
            }
        }

        public MDFeNaoEncerradoGrid ItemSelecionadoMDFeNaoEncerrado
        {
            get => _itemSelecionadoMdFeNaoEncerrado;
            set
            {
                _itemSelecionadoMdFeNaoEncerrado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EmissorFiscal> ListaEmissorFiscal
        {
            get => _listaEmissorFiscal;
            set
            {
                _listaEmissorFiscal = value;
                PropriedadeAlterada();
            }
        }

        public EmissorFiscal EmissorSelecionado
        {
            get => _emissorSelecionado;
            set
            {
                _emissorSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public void PreencherEmissorFiscal()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var lista = new RepositorioEmissorFiscal(sessao).BuscaEmissorMDFe();

                ListaEmissorFiscal = new ObservableCollection<EmissorFiscal>(lista);
            }
        }

        public ConsultaNaoEncerradosFormModel()
        {
            Inicializa();
        }

        private void Inicializa()
        {
            PreencherEmissorFiscal();
            ListaMDFeNaoEncerrado = new ObservableCollection<MDFeNaoEncerradoGrid>();
            ResultadoTexto = "Resultado Consulta";
        }

        public ICommand CommandEfetuarConsulta => GetSimpleCommand(EfetuarConsulta);

        private async void EfetuarConsulta(object obj)
        {
            if (EmissorSelecionado == null)
            {
                DialogBox.MostraInformacao("Selecione um emissor");
                return;
            }

            await Task.Run(() => Consulta());
        }


        public string ResultadoTexto
        {
            get => _resultadoTexto;
            set
            {
                _resultadoTexto = value;
                PropriedadeAlterada();
            }
        }

        private void Consulta()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.ShowProgressBar);


                FactoryConfiguracoesZeusMdfe.CarregarConfiguracao(EmissorSelecionado.EmissorFiscalMdfe);

                var servicoConsultaNaoEncerrados = new ServicoMDFeConsultaNaoEncerrados();
                var resposta = servicoConsultaNaoEncerrados.MDFeConsultaNaoEncerrados(EmissorSelecionado.Empresa.Cnpj);

                if (resposta.InfMDFe != null && resposta.InfMDFe.Count > 0)
                {
                    Application.Current.Dispatcher.Invoke(ListaMDFeNaoEncerrado.Clear);
                    resposta.InfMDFe.ForEach(AtualizaView);
                }

                ResultadoTexto = resposta.XMotivo;
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            finally
            {
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
            }
        }

        private void AtualizaView(MDFeNaoEncerradaInfMDFe i)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListaMDFeNaoEncerrado.Add(new MDFeNaoEncerradoGrid
                {
                    Chave = i.ChMDFe,
                    NumeroFiscal = i.ChMDFe.Substring(25, 9),
                    Protocolo = i.NProt
                });
            });
        }

        public void EncerrarMDFeSelecionado()
        {
            try
            {
                if (!DialogBox.MostraConfirmacao("Encerrar MDF-e?", MessageBoxImage.Question)) return;

                var mdfeFusion = BuscarMDFe(ItemSelecionadoMDFeNaoEncerrado.Chave);

                ZeusMDFe mdfe = null;
                EmissorFiscal emissor = null;

                if (mdfeFusion == null)
                {
                    emissor = BuscarEmissorSelecionado(EmissorSelecionado);

                    mdfe = new ZeusMDFe
                    {
                        InfMDFe =
                        {
                            Id = $"MDFe{ItemSelecionadoMDFeNaoEncerrado.Chave}",
                            Emit =
                            {
                                CNPJ = emissor.Empresa.Cnpj,
                                EnderEmit =
                                {
                                    UF = emissor.Empresa.EstadoDTO.ToZeusMdfe(),
                                    CMun = emissor.Empresa.CidadeDTO.CodigoIbge
                                }
                            }
                        }
                    };
                }

                if (mdfeFusion != null)
                {
                    mdfe = ZeusMDFe.LoadXmlString(mdfeFusion.Emissao.XmlAssinado);
                }

                var modeloUfMunicipioEncerramento = new MdfeUfEMunicipioEncerramentoFormModel();

                if (mdfeFusion == null)
                {
                    modeloUfMunicipioEncerramento.AdicionarUfEMunicipioEncerramentoPadrao(emissor.Empresa.EstadoDTO, emissor.Empresa.CidadeDTO);
                }

                if (mdfeFusion != null)
                {
                    var localidadeServico = LocalidadesServico.GetInstancia();
                    var estadoDto = localidadeServico.GetEstado(x => x.CodigoIbge == (int)mdfe.InfMDFe.Ide.UFFim);
                    var cidadeDto = localidadeServico.GetCidade(x =>
                        x.CodigoIbge == int.Parse(mdfe.InfMDFe.InfDoc.InfMunDescarga.LastOrDefault().CMunDescarga));

                    modeloUfMunicipioEncerramento.AdicionarUfEMunicipioEncerramentoPadrao(estadoDto, cidadeDto);
                }

                modeloUfMunicipioEncerramento.EnviarEncerramentoManipulador += async (sender, model) =>
                {
                    await Task.Run(() => Finalizar(mdfeFusion, mdfe, model));
                };

                new MdfeUfEMunicipioEncerramentoForm(modeloUfMunicipioEncerramento).ShowDialog();
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }

        }

        private void Finalizar(MDFeEletronico mdfeFusion, ZeusMDFe mdfe, MdfeUfEMunicipioEncerramentoFormModel model)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.ShowProgressBar);

                FactoryConfiguracoesZeusMdfe.CarregarConfiguracao(EmissorSelecionado.EmissorFiscalMdfe);

                var evento = new ServicoMDFeEvento();
                var resposta = evento.MDFeEventoEncerramentoMDFeEventoEncerramento(mdfe, model.Estado.ToZeusMdfe(), model.Cidade.CodigoIbge, 1, ItemSelecionadoMDFeNaoEncerrado.Protocolo);

                if (resposta.InfEvento.CStat == 135 && mdfeFusion != null)
                    SalvarMDFe(mdfeFusion, resposta);

                DialogBox.MostraInformacao(resposta.InfEvento.XMotivo);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ListaMDFeNaoEncerrado.Remove(ItemSelecionadoMDFeNaoEncerrado);
                });

            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            finally
            {
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
            }
        }

        private EmissorFiscal BuscarEmissorSelecionado(EmissorFiscal emissorSelecionado)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioEmissorFiscal(sessao).GetPeloId(emissorSelecionado.Id);
            }
        }

        private void SalvarMDFe(MDFeEletronico mdfe, MDFeRetEventoMDFe resposta)
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                mdfe.Status = MDFeStatus.Encerrada;
                repositorio.Salvar(mdfe);

                repositorio.SalvarEvento(new MDFeEvento
                {
                    Evento = MDFeTipoEvento.Encerramento,
                    FeitoEm = (DateTime)resposta.InfEvento.DhRegEvento,
                    Mdfe = mdfe,
                    XmlEnvio = resposta.EnvioXmlString,
                    XmlRetorno = resposta.RetornoXmlString
                });

                transacao.Commit();
            }

        }

        private MDFeEletronico BuscarMDFe(string chave)
        {
            MDFeEletronico mdfe;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioMdfe(sessao);
                mdfe = repositorio.BuscarPelaChave(chave);
            }

            return mdfe;
        }


    }
}