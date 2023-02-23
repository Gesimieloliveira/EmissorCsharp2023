using System;
using System.Security.Cryptography.X509Certificates;
using DFe.DocumentosEletronicos.Flags;
using DFe.Ext;
using DFe.Utils;
using FusionCore.DFe.RegrasNegocios;
using FusionCore.DFe.RegrasNegocios.Chave;
using FusionCore.DFe.XmlCte;
using FusionCore.FusionAdm.ConfiguracoesDfe;
using FusionCore.FusionAdm.CteEletronico.Assinatura;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Flags.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Validacoes;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Repositorio.FusionAdm;
using Shared.NFe.Utils.InfRespTec;

// ReSharper disable PossibleNullReferenceException

namespace FusionCore.FusionAdm.CteEletronico.Autorizador
{
    public class EmitirCte
    {
        private readonly EmissorFiscal _emissor;
        private CteEmissaoHistorico _emissaoHistorico;

        public EmitirCte(EmissorFiscal emissor)
        {
            _emissor = emissor;
        }

        public CteEmissaoHistorico Emite(Cte cte)
        {
            if (cte.CteEmissao?.Autorizado == true)
            {
                throw new ArgumentException("CT-e não pode ser emitida; JÁ AUTORIZADA");
            }

            if (PossuiEmissaoPendente(cte))
                throw new InvalidOperationException("Não foi possivel emitir a NF-e, possui emissão pendente");

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {

                try
                {
                    _emissaoHistorico = GeraEmissao(cte);

                    var repositorioCte = new RepositorioCte(sessao);

                    repositorioCte.SalvarEmissaoHistorico(_emissaoHistorico);
                    repositorioCte.Salvar(cte);

                    transacao.Commit();
                }
                catch (Exception)
                {
                    transacao.Rollback();
                    throw;
                }
            }

            return _emissaoHistorico;
        }

        private bool PossuiEmissaoPendente(Cte cte)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCte(sessao);
                return repositorio.PossuiEmissaoPendente(cte);
            }
        }

        private CteEmissaoHistorico GeraEmissao(Cte cte)
        {
            var emissao = new CteEmissaoHistorico
            {
                CriadaEm = DateTime.Now,
                Cte = cte,
                AmbienteSefaz = _emissor.EmissorFiscalCte.Ambiente,
                Chave = string.Empty,
                Finalizada = false,
                NumeroRecibo = string.Empty,
                TipoEmissao = cte.TipoEmissao,
                XmlEnvio = string.Empty,
                XmlLote = null,
                XmlRetorno = null,
            };

            GerarChaveFiscal(cte, emissao);

            AssinarDocumento(cte, emissao);

            return emissao;
        }

        private void AssinarDocumento(Cte cte, CteEmissaoHistorico historicoEmissao)
        {
            var certificado = CertificadoDigitalFactory.Cria(cte.PerfilCte.EmissorFiscal, true);

            var cteEnvio = cte.ToCteEnvio(historicoEmissao);

            GerarHashCsrt(cte, cteEnvio, historicoEmissao.Chave);

            var cteXml = FuncoesXml.ClasseParaXmlString(cteEnvio).RemoverAcentos();

            var xmlAssinado = AssinaturaDigital.AssinaDocumento(cteXml, cteEnvio.InformacoesCTe.Id, certificado);

            new ValidarSchema().Validar(xmlAssinado, ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.Cte\cte_v3.00.xsd");

            var cteFusion = FuncoesXml.XmlStringParaClasse<FusionCTe>(xmlAssinado);

            ResolverQrCode(cteFusion, certificado);

            xmlAssinado = FuncoesXml.ClasseParaXmlString(cteFusion).RemoverAcentos();

            historicoEmissao.XmlEnvio = xmlAssinado;
        }

        private void ResolverQrCode(FusionCTe fusionCte, X509Certificate2 certificado)
        {
            var cte = fusionCte;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var uf = new RepositorioEstado(sessao).GetPelaSigla(cte.InformacoesCTe.Emitente.Endereco.SiglaUf);
                var tipoAmbiente = cte.InformacoesCTe.Identificacao.Ambiente == FusionTipoAmbienteCTe.Homologacao
                    ? FusionAdm.Fiscal.Flags.TipoAmbiente.Homologacao
                    : FusionAdm.Fiscal.Flags.TipoAmbiente.Producao;

                if (new RepositorioConfiguracaoDfe(sessao).AdicionarQrCode(uf, tipoAmbiente, TipoDocumentoFiscalEletronico.CTe))
                {
                    fusionCte.InfCTeSupl = ResolveQrCode.QrCode(fusionCte, certificado);
                }
            }
        }

        private void GerarHashCsrt(Cte cte, FusionCTe cteEnvio, string chave)
        {
            if (cteEnvio.InformacoesCTe.FusionInformacaoResponsabilidadeCTe == null) return;

            var ufId = cte.CteEmitente.Emitente.EstadoDTO.Id;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioResponsavelTecnico = new RepositorioResponsavelTecnico(sessao);

                if (repositorioResponsavelTecnico.ExisteCsrt(ufId, TipoDocumentoFiscalEletronico.CTe) == false) return;

                var reponsavelTecnico =
                    repositorioResponsavelTecnico.BuscarPorUf(ufId);

                cteEnvio.InformacoesCTe.FusionInformacaoResponsabilidadeCTe.hashCSRT = GerarHashCSRT.HashCSRT(reponsavelTecnico.Csrt, chave);
                cteEnvio.InformacoesCTe.FusionInformacaoResponsabilidadeCTe.idCSRT = reponsavelTecnico.CsrtId.ToString("D2");
            }
        }

        private static void GerarChaveFiscal(Cte cte, CteEmissaoHistorico emissao)
        {
            var chave = new GerarChaveFiscal(
                (int) ModeloDocumento.CTe,
                (int) cte.TipoEmissao.ToXml(cte.Estado),
                cte.CodigoNumericoEmissao,
                cte.CteEmitente.Emitente.EstadoDTO.CodigoIbge,
                cte.EmissaoEm,
                Convert.ToInt64(cte.CteEmitente.Emitente.DocumentoUnico),
                cte.NumeroFiscalEmissao,
                cte.SerieEmissao);

            emissao.Chave = chave.Chave;
        }
    }
}