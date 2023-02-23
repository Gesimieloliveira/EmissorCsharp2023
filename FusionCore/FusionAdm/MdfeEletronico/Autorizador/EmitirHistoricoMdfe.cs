using System;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.FusionAdm.MdfeEletronico.Autorizador
{
    public class EmitirHistoricoMdfe
    {
        public MDFeEmissaoHistorico GerarHistorico(MDFeEletronico mdfe, string chave, string xmlEnvio)
        {
            if (mdfe.Emissao?.Autorizado == true)
            {
                throw new ArgumentException("MDF-e não pode ser emitida; JÁ AUTORIZADA");
            }

            var historico = new MDFeEmissaoHistorico
            {
                Chave = chave,
                XmlRetorno = string.Empty,
                Finalizada = false,
                MDFeEletronico = mdfe,
                AmbienteSefaz = mdfe.EmissorFiscal.EmissorFiscalMdfe.Ambiente,
                CriadoEm = DateTime.Now,
                EnviadoEm = DateTime.Now,
                TipoEmissao = mdfe.TipoEmissao,
                XmlEnvio = xmlEnvio
            };

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);

                repositorioMdfe.SalvarHistorico(historico);

                transacao.Commit();
            }

            return historico;
        }
    }
}