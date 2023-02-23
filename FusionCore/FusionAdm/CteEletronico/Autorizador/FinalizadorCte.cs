using System;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;

namespace FusionCore.FusionAdm.CteEletronico.Autorizador
{
    public class FinalizadorCte
    {
        public void FinalizarEmsisao(Cte cte, CteEmissaoHistorico historico)
        {
            var emissorFiscal = cte.EmissorFiscal;
            var certificado = CertificadoDigitalFactory.Cria(emissorFiscal, true);

            var situacaoNotaSefaz = new SituacaoNotaSefazCte(certificado, historico);
            ProcessarFinalizacao(historico, situacaoNotaSefaz);

            if (historico.IsRejeicao999())
            {
                throw new InvalidOperationException(historico.GetTextoRejeicao());
            }

            using (var sessao = new SessaoManagerAdm().CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCte = new RepositorioCte(sessao);

                repositorioCte.SalvarEmissaoHistorico(historico);

                if (historico.IsAutorizadoUsoDaEmissao() || historico.IsDenegadoUsoDaEmissao())
                {
                    var cteEmissao = new CteEmissao(historico.Cte, (int)cte.NumeroFiscalEmissao, cte.SerieEmissao, historico.AmbienteSefaz)
                    {
                        Chave = historico.Chave,
                        RecebidoEm = historico.RecebidoEm,
                        NumeroRecibo = historico.NumeroRecibo,
                        EmitidaEm = cte.EmissaoEm,
                        Autorizado = true,
                        CodigoAutorizacao = 100,
                        CodigoNumerico = cte.CodigoNumericoEmissao,
                        DigestValue = historico.ObterDigestValue(),
                        DigitoVerificador = historico.ObterDigitoVerificador(),
                        Modelo = ModeloDocumento.CTe,
                        Motivo = historico.Motivo,
                        Numero = (int) cte.NumeroFiscalEmissao,
                        Cte = historico.Cte,
                        Ambiente = historico.AmbienteSefaz,
                        StatusConsultaRecibo = historico.IsAutorizadoUsoDaEmissao() ? CteEmissaoStatus.Autorizado : CteEmissaoStatus.Denegado,
                        Protocolo = historico.ObterProtocolo(),
                        Serie = historico.Cte.SerieEmissao,
                        TagId = $"CTe{historico.Chave}",
                        VersaoAplicativoAutorizacao = historico.ObterVersaoAplicativoAutorizacao(),
                        XmlAssinado = historico.XmlEnvio,
                        XmlAutorizado = historico.XmlRetorno
                    };

                    cteEmissao.AutonrizaXml();

                    cte.CteEmissao = cteEmissao;

                    repositorioCte.SalvarEmissao(cteEmissao);
                }

                transacao.Commit();
            }
        }

        private void ProcessarFinalizacao(CteEmissaoHistorico historico, SituacaoNotaSefazCte situacaoNotaSefaz)
        {
            if (historico.PossuiRecibo())
            {
                var respostaRecibo = situacaoNotaSefaz.GetSituacaoPeloRecibo(historico.NumeroRecibo, historico.AmbienteSefaz);
                historico.ProcessarRespotaLote(respostaRecibo);

                if (historico.IsRejeicao999())
                {
                    throw new InvalidOperationException(historico.GetTextoRejeicao());
                }
                return;
            }

            var respostaChave = situacaoNotaSefaz.GetSituacaoPelaChave(historico.Chave);
            historico.ProcessarRespostaPelaChave(respostaChave);

            if (historico.IsRejeicao999())
            {
                throw new InvalidOperationException(historico.GetTextoRejeicao());
            }
        }
    }
}