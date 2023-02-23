using System;
using System.Xml;
using DFe.Utils;
using FusionCore.DFe.WsdlCte.Homologacao.Evento;
using FusionCore.DFe.WsdlCte.Homologacao.Helper;
using FusionCore.DFe.XmlCte.XmlCte.RegistroEventos;
using FusionCore.FusionAdm.CteEletronico.Assinatura;
using FusionCore.FusionAdm.CteEletronico.ConsultarProtocolos;
using FusionCore.FusionAdm.CteEletronico.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Flags.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Validacoes;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.FusionAdm.CteEletronico.Cancelar
{
    public class Sucesso
    {
        public FusionRetornoRegistroEventoCTe RetornoCancelamento { get; }

        public Sucesso(FusionRetornoRegistroEventoCTe retornoCancelamento)
        {
            RetornoCancelamento = retornoCancelamento;
        }
    }

    public class Falhou
    {
        public ArgumentException Exception { get; }

        public Falhou(ArgumentException exception)
        {
            Exception = exception;
        }
    }

    public class CancelarRn
    {
        private readonly ICancelarCte _cte;
        private string _justificativa;
        private FusionRetornoRegistroEventoCTe _retorno;
        private string _xmlEnviar;
        private string _xmlResposta;
        public ICancelarCte Cte => _cte;

        public CancelarRn(ICancelarCte cte)
        {
            _cte = cte;
        }

        public event EventHandler<Sucesso> SucessoHandler;
        public event EventHandler<Falhou> FalhouHandler;

        public void Cancela()
        {
            try
            {
                var registroEventoCancelamento = ConstroiObjetoFusionCancelamento();

                ValidaSchemas(registroEventoCancelamento);

                XmlDocument xmlRequest;
                var cancelarWsdl = EnviaSefaz(registroEventoCancelamento, out xmlRequest);

                var xmlResposta = cancelarWsdl.cteRecepcaoEvento(xmlRequest);

                _xmlResposta = xmlResposta.OuterXml;
                _retorno = FuncoesXml.XmlStringParaClasse<FusionRetornoRegistroEventoCTe>(xmlResposta.OuterXml);

                if (_retorno.RetornoInformacaoEvento.CodigoStatus == 631)
                {
                    var retorno = new NegocioConsultaProtocolo(_cte).Consultar();

                    foreach (var fusionProcEventoCTe in retorno.FusionProcEventoCTe)
                    {
                        if (fusionProcEventoCTe.FusionRetornoEventoCTe.RetornoInformacaoEvento.TipoEvento == 110111)
                        {
                            _retorno.RetornoInformacaoEvento = retorno.FusionProcEventoCTe[0].FusionRetornoEventoCTe.RetornoInformacaoEvento;
                        }
                    }
                }


                OnSucessoHandler(_retorno);
            }
            catch (Exception ex)
            {
                OnFalhouHandler(new Falhou(new ArgumentException(ex.Message, ex)));
            }
        }

        private FusionRegistroEventoCTe ConstroiObjetoFusionCancelamento()
        {
            var cancelamentoFusion = new FusionRegistroEventoCTe {Versao = "3.00"};

            var informacao = cancelamentoFusion.InformacaoEvento;

            const int tipoEvento = 110111;
            var chave = _cte.Chave;
            const int sequenciaEvento = 1;

            informacao.Id = "ID" + tipoEvento + chave + sequenciaEvento.ToString("D2");
            informacao.CodigoOrgao = _cte.Estado.CodigoIbge;
            informacao.Ambiente = _cte.TipoAmbiente.ToXml();
            informacao.Cnpj = _cte.CnpjCpfEmitente.Trim();
            informacao.Chave = chave;
            informacao.HoraEvento = DateTime.Now.ParaDataHoraStringUtc();
            informacao.TipoEvento = tipoEvento;
            informacao.SequencialEvento = sequenciaEvento;

            var detalheEvento = informacao.DetalheEvento;
            detalheEvento.VersaoEvento = "3.00";
            detalheEvento.EventoCancelamento = new FusionEventoCancelamentoCTe();

            var cancelamento = detalheEvento.EventoCancelamento;
            cancelamento.DescricaoEvento = "Cancelamento";
            cancelamento.NumeroProtocolo = _cte.Protocolo;
            cancelamento.Justificativa = _justificativa.RemoverAcentos();

            var certificado = CertificadoDigitalFactory.Cria(_cte.EmissorFiscal, true);

            var xml = FuncoesXml.ClasseParaXmlString(cancelamentoFusion).RemoverAcentos();
            var xmlAssinado = AssinaturaDigital.AssinaDocumento(xml, informacao.Id, certificado);

            return FuncoesXml.XmlStringParaClasse<FusionRegistroEventoCTe>(xmlAssinado);
        }

        private static void ValidaSchemas(FusionRegistroEventoCTe registroEventoCancelamento)
        {
            var xmlEvento = FuncoesXml.ClasseParaXmlString(registroEventoCancelamento);
            var xmlCancelamento =
                FuncoesXml.ClasseParaXmlString(
                    registroEventoCancelamento.InformacaoEvento.DetalheEvento.EventoCancelamento);

            var validacao = new ValidarSchema();
            validacao.Validar(xmlEvento, ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.Cte\eventoCTe_v3.00.xsd");

            validacao.Validar(xmlCancelamento, ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.Cte\evCancCTe_v3.00.xsd");
        }

        private CteRecepcaoEvento EnviaSefaz(FusionRegistroEventoCTe registroEventoCancelamento, out XmlDocument xmlRequest)
        {
            _xmlEnviar = FuncoesXml.ClasseParaXmlString(registroEventoCancelamento);

            var url = UrlHelper.ObterUrl(_cte.Estado.ToXml(), _cte.TipoAmbiente.ToXml(), _cte.TipoEmissao.ToXml(_cte.Estado));

            var cancelarWsdl = new CteRecepcaoEvento(url.CteRecepcaoEvento)
            {
                cteCabecMsgValue = new cteCabecMsg
                {
                    cUF = _cte.Estado.CodigoIbge.ToString(),
                    versaoDados = "3.00"
                }
            };

            cancelarWsdl.ClientCertificates.Add(CertificadoDigitalFactory.Cria(_cte.EmissorFiscal, true));

            xmlRequest = new XmlDocument();
            xmlRequest.LoadXml(_xmlEnviar);
            return cancelarWsdl;
        }

        protected virtual void OnSucessoHandler(FusionRetornoRegistroEventoCTe retorno)
        {
            SucessoHandler?.Invoke(this, new Sucesso(retorno));
        }

        protected virtual void OnFalhouHandler(Falhou falhou)
        {
            FalhouHandler?.Invoke(this, falhou);
        }

        public void AdicionarJustificativa(string justificativa)
        {
            _justificativa = justificativa?.Trim();
        }

        public CancelamentoCteDados ObterCTeCancelamento()
        {
            var cancelamento = new CancelamentoCteDados();
            cancelamento.XmlEnvio = _xmlEnviar;
            cancelamento.XmlRetorno = _xmlResposta;
            cancelamento.Cte = _cte.GetCte();
            cancelamento.TipoAmbiente = _cte.TipoAmbiente;
            cancelamento.StatusResposta = _retorno.RetornoInformacaoEvento.CodigoStatus;
            cancelamento.Justificativa = _justificativa;
            cancelamento.OcorreuEm = _retorno.RetornoInformacaoEvento.DataEHoraRegistroEvento;
            cancelamento.Motivo = _retorno.RetornoInformacaoEvento.Motivo;

            return cancelamento;
        }
    }
}