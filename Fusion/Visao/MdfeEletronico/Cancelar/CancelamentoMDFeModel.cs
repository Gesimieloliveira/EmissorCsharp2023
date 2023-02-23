using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Factory;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;
using MDFe.Classes.Informacoes.Evento.Flags;
using MDFe.Classes.Retorno.MDFeEvento;
using MDFe.Servicos.ConsultaProtocoloMDFe;
using MDFe.Servicos.EventosMDFe;
using MDFeEvento = FusionCore.FusionAdm.MdfeEletronico.MDFeEvento;
using ZeusMDFe = MDFe.Classes.Informacoes.MDFe;

namespace Fusion.Visao.MdfeEletronico.Cancelar
{
    public class CancelamentoMDFeModel : ViewModel
    {
        private string _chave;
        private string _numeroDocumento;
        public MDFeEletronico Mdfe { get; }

        [Required(ErrorMessage = @"Porfavor digitar uma justificativa com no mínimo 15 caracteres.")]
        [MinLength(15, ErrorMessage = @"Porfavor digitar uma justificativa com no mínimo 15 caracteres.")]
        public string Justificativa
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Chave
        {
            get { return _chave; }
            set
            {
                if (value == _chave) return;
                _chave = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroDocumento
        {
            get { return _numeroDocumento; }
            set
            {
                if (value == _numeroDocumento) return;
                _numeroDocumento = value;
                PropriedadeAlterada();
            }
        }

        public CancelamentoMDFeModel(MDFeEletronico mdfe)
        {
            Mdfe = mdfe;
            Chave = mdfe.Emissao.Chave;
            NumeroDocumento = mdfe.Emissao.NumeroDocumento.ToString();
        }

        public async void CancelarAsync()
        {
            await Task.Run(() => Cancelar());
        }

        public event EventHandler FecharTela;

        private void Cancelar()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.ShowProgressBar);

                FactoryConfiguracoesZeusMdfe.CarregarConfiguracao(Mdfe.EmissorFiscal.EmissorFiscalMdfe);

                var evento = new ServicoMDFeEvento();
                var zeusMdfe = ZeusMDFe.LoadXmlString(Mdfe.Emissao.XmlAssinado);

                var resposta = evento.MDFeEventoCancelar(zeusMdfe, 1, Mdfe.Emissao.Protocolo, Justificativa.Trim());

                if (TrataDuplicidade631(resposta)) return;

                if (resposta.InfEvento.CStat == 135)
                    SalvarMDFe(Mdfe, resposta);

                ProcessaResposta(resposta);

                if (resposta.InfEvento.CStat == 135)
                {
                    OnFecharTela();
                }
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

        private bool TrataDuplicidade631(MDFeRetEventoMDFe resposta)
        {
            if (resposta.InfEvento.CStat != 631) return false;

            var servicoFoda = new ServicoMDFeConsultaProtocolo();
            var xmlObjeto = servicoFoda.MDFeConsultaProtocolo(Mdfe.Emissao.Chave);

            xmlObjeto.ProcEventoMDFe.ForEach(e =>
            {
                if (e.EventoMDFe.InfEvento.TpEvento != MDFeTipoEvento.Cancelamento) return;

                resposta.InfEvento = e.RetEventoMDFe.InfEvento;

                SalvarMDFe(Mdfe, resposta);
            });

            ProcessaResposta(resposta);

            OnFecharTela();
            return true;
        }

        private void ProcessaResposta(MDFeRetEventoMDFe resposta)
        {
            DialogBox.MostraInformacao(resposta.InfEvento.XMotivo);
        }

        private void SalvarMDFe(MDFeEletronico mdfe, MDFeRetEventoMDFe resposta)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioMdfe(sessao);
                mdfe.Status = MDFeStatus.Cancelada;
                repositorio.Salvar(mdfe);

                repositorio.SalvarEvento(new MDFeEvento
                {
                    Evento = MDFeTipoEvento.Cancelamento,
                    FeitoEm = (DateTime)resposta.InfEvento.DhRegEvento,
                    Mdfe = mdfe,
                    XmlEnvio = resposta.EnvioXmlString,
                    XmlRetorno = resposta.RetornoXmlString
                });

                transacao.Commit();
            }
        }

        protected virtual void OnFecharTela()
        {
            FecharTela?.Invoke(this, EventArgs.Empty);
        }
    }
}