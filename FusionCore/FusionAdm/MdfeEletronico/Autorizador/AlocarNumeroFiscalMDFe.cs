using System;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.FusionAdm.MdfeEletronico.Autorizador
{
    public class AlocarNumeroFiscalMDFe
    {
        public void Alocar(MDFeEletronico mdfe)
        {
            var emissor = mdfe.EmissorFiscal.EmissorFiscalMdfe;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var numero = ++emissor.NumeroAtual;
                var serie = emissor.Serie;

                mdfe.NumeroFiscalEmissao = numero;
                mdfe.SerieEmissao = serie;
                mdfe.CodigoNumericoEmissao = CodigoNumeroEmissao(mdfe);

                new RepositorioMdfe(sessao).Salvar(mdfe);
                new RepositorioEmissorFiscal(sessao).Salvar(emissor.EmissorFiscal);

                transacao.Commit();
            }
        }

        private int CodigoNumeroEmissao(MDFeEletronico mdfe)
        {
            var random = new Random().Next(1, 99999999);

            if (random == mdfe.NumeroFiscalEmissao)
            {
                return CodigoNumeroEmissao(mdfe);
            }

            return random;
        }
    }
}