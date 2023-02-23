using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Factory;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;
using MDFe.Classes.Informacoes.Evento.Flags;
using MDFe.Classes.Retorno.MDFeEvento;
using MDFe.Servicos.ConsultaProtocoloMDFe;
using MDFe.Servicos.EventosMDFe;
using ZeusMDFe = MDFe.Classes.Informacoes.MDFe;

namespace Fusion.Visao.MdfeEletronico.IncluirCondutor
{
    public sealed class IncluirCondutorFormModel : ViewModel
    {
        private readonly MDFeEletronico _mdfe;
        private PessoaEntidade _pessoa;
        private string _cpf;

        public PessoaEntidade Pessoa
        {
            get => _pessoa;
            set
            {
                _pessoa = value;
                PropriedadeAlterada();
            }
        }

        public string Cpf
        {
            get => _cpf;
            set
            {
                _cpf = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscarCondutor => GetSimpleCommand(BuscarCondutor);

        public IncluirCondutorFormModel(MDFeEletronico mdfe)
        {
            _mdfe = mdfe;
        }

        public event EventHandler JanelaCancelarFechada;

        private void BuscarCondutor(object obj)
        {
            var pickerModel = new PessoaPickerModel(new PessoaEngine());
            pickerModel.PickItemEvent += CondutorSelecionadoCompleted;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void CondutorSelecionadoCompleted(object sender, GridPickerEventArgs e)
        {
            try
            {
                var pessoa = e.GetItem<PessoaEntidade>();

                if (pessoa.Tipo != PessoaTipo.Fisica)
                {
                    throw new ArgumentException("Somente e permitido pessoas do tipo Física");
                }

                Pessoa = pessoa;
                Cpf = pessoa.Cpf.Valor;
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        public async void IncluirCondutor()
        {
            await Task.Run(() => Incluir());
        }

        private void Incluir()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.ShowProgressBar);

                FactoryConfiguracoesZeusMdfe.CarregarConfiguracao(_mdfe.EmissorFiscal.EmissorFiscalMdfe);


                var mdfe = ZeusMDFe.LoadXmlString(_mdfe.Emissao.XmlAssinado);

                var evento = new ServicoMDFeEvento();
                var resposta = evento.MDFeEventoIncluirCondutor(mdfe, 1, Pessoa.Nome, Pessoa.Cpf.Valor);

                // aguardar melhoria pois podemos ter mais de 1 aqui if (TrataDuplicidade631(resposta)) return;

                if (resposta.InfEvento.CStat == 135)
                    SalvarMDFe(_mdfe, resposta);

                DialogBox.MostraInformacao(resposta.InfEvento.XMotivo);
                OnJanelaCancelarFechada();
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
            var xmlObjeto = servicoFoda.MDFeConsultaProtocolo(_mdfe.Emissao.Chave);

            xmlObjeto.ProcEventoMDFe.ForEach(e =>
            {
                if (e.EventoMDFe.InfEvento.TpEvento != MDFeTipoEvento.InclusaoDeCondutor) return;

                resposta.InfEvento = e.RetEventoMDFe.InfEvento;

                SalvarMDFe(_mdfe, resposta);
            });

            DialogBox.MostraInformacao(resposta.InfEvento.XMotivo);
            OnJanelaCancelarFechada();
            return true;
        }


        private void SalvarMDFe(MDFeEletronico mdfe, MDFeRetEventoMDFe resposta)
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);
                repositorio.SalvarEvento(new MDFeEvento
                {
                    Evento = MDFeTipoEvento.InclusaoDeCondutor,
                    FeitoEm = (DateTime) resposta.InfEvento.DhRegEvento,
                    Mdfe = mdfe,
                    XmlEnvio = resposta.EnvioXmlString,
                    XmlRetorno = resposta.RetornoXmlString
                });

                transacao.Commit();
            }
        }

        private void OnJanelaCancelarFechada()
        {
            JanelaCancelarFechada?.Invoke(this, EventArgs.Empty);
        }
    }
}