using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using DFe.Utils;
using DFe.Utils.Assinatura;
using FusionCore.DFe.RegrasNegocios;
using FusionCore.DFe.XmlCte;
using FusionCore.DFe.XmlCte.XmlCte.LoteEnvio;
using FusionCore.DFe.XmlCte.XmlCte.Retorno;
using FusionCore.DFe.XmlCte.XmlCte.RetornoRecepcao;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Flags.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Validacoes;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.FusionAdm.CteEletronico.Autorizador
{
    public class AutorizaNaSefazCte
    {
        private readonly X509Certificate2 _certificado;

        public AutorizaNaSefazCte(X509Certificate2 certificado)
        {
            _certificado = certificado;
        }

        public void AutorizaNaSefaz(CteEmissaoHistorico emissao)
        {
            var xmlEnvio = MontaLoteDeEnvio(emissao, out var wsResultado, out var xmlRequest);

            FusionInformacaoProcessadaCTe resposta = null;
            FusionRetornoRecepcaoCTe retornoRecepcao = null;
            var dataEnvio = DateTime.Now;
            emissao.EnviadaEm = dataEnvio;

            try
            {
                FusionResultadoEnvioLoteCTe retornoTratado = null;

                var wsAutorizacao = new AutorizacaoCte();
                wsResultado = wsAutorizacao.Executa(xmlRequest,
                    emissao.Cte.PerfilCte.EmissorFiscal.Empresa.EstadoDTO.ToXml(),
                    _certificado,
                    emissao.Cte.PerfilCte.EmissorFiscal.EmissorFiscalCte.Ambiente.ToXml(), emissao.Cte.TipoEmissao.ToXml(emissao.Cte.Estado));

                var xmlLote = wsResultado.OuterXml;
                var retornoAutorizacao = FuncoesXml.XmlStringParaClasse<FusionRetornoEnvioCTe>(xmlLote);

                var numeroRecibo = retornoAutorizacao.DadosRecibo?.Numero ?? string.Empty;


                if (retornoAutorizacao.StatusResposta == 999)
                {
                    throw new InvalidOperationException("Rejeição: 999,\ntente novamente");
                }

                emissao.XmlLote = xmlLote;
                emissao.NumeroRecibo = numeroRecibo;
                emissao.StatusConsultaRecibo = emissao.NumeroRecibo.IsNotNullOrEmpty() ? CteEmissaoStatus.Pendente : CteEmissaoStatus.Vazio;

                SalvarHistorico(emissao);
            }
            catch (WebException)
            {
                FalhaAoReceberRecibo(emissao);
                CertificadoDigital.ClearCache();
                throw;
            }
            catch (InvalidOperationException)
            {
                FalhaAoReceberRecibo(emissao);
                throw;
            }
            catch (Exception ex)
            {
                FalhaAoReceberRecibo(emissao);
                throw;
            }
        }

        private static string MontaLoteDeEnvio(CteEmissaoHistorico emissao, out XmlNode wsResultado, out XmlDocument xmlRequest)
        {
            var cte = FuncoesXml.XmlStringParaClasse<FusionCTe>(emissao.XmlEnvio);

            var lote = new FusionEnvioCTe {IdLote = 1.ToString()};
            lote.FusionCTes.Add(cte);

            var xmlEnvio = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + FuncoesXml.ClasseParaXmlString(lote);

            ValidarSchema(xmlEnvio, lote);

            wsResultado = default(XmlNode);
            xmlRequest = new XmlDocument();

            if (cte.InformacoesCTe.Emitente.Endereco.SiglaUf == "PR" 
            || emissao.Cte.CteEmitente.Emitente.EstadoDTO.Sigla == "MT")
                xmlEnvio = xmlEnvio.Replace("<CTe>", "<CTe xmlns=\"http://www.portalfiscal.inf.br/cte\">");

            xmlRequest.LoadXml(xmlEnvio);
            return xmlEnvio;
        }

        private void FalhaAoReceberRecibo(CteEmissaoHistorico emissao)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCte = new RepositorioCte(sessao);

                emissao.FalhaReceberLote = true;
                repositorioCte.SalvarEmissaoHistorico(emissao);

                transacao.Commit();
            }
        }

        private static void SalvarHistorico(CteEmissaoHistorico emissao)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCte(sessao);
                repositorio.SalvarEmissaoHistorico(emissao);

                sessao.Flush();
                sessao.Clear();

                transacao.Commit();
            }
        }

        private static void ValidarSchema(string xmlEnvio, FusionEnvioCTe lote)
        {
            var validarXml = new ValidarSchema();

            var xmlCTe = FuncoesXml.ClasseParaXmlString(lote.FusionCTes[0]);


            validarXml.Validar(xmlCTe, ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.Cte\cte_v3.00.xsd");

            if (lote.FusionCTes[0].InformacoesCTe.InformacoesCTeNormal != null)
            {
                var xmlRodoviario = FuncoesXml.ClasseParaXmlString(lote.FusionCTes[0].InformacoesCTe.InformacoesCTeNormal.Modal.Rodoviario);
                validarXml.Validar(xmlRodoviario, ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.Cte\cteModalRodoviario_v3.00.xsd");
            }

            validarXml.Validar(xmlEnvio, ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.Cte\enviCTe_v3.00.xsd");
        }
    }
}