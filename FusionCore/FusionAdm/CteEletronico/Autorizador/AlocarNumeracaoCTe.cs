using System;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.FusionAdm.CteEletronico.Autorizador
{
    public class AlocarNumeracaoCTe
    {
        private readonly Cte _cte;

        public AlocarNumeracaoCTe(Cte cte)
        {
            _cte = cte;
        }

        public void AlocarNumeroFiscal()
        {
            var emissor = _cte.EmissorFiscal;
            AtualizarEmissor(_cte.EmissorFiscal);

            var numeroAtual = ++emissor.EmissorFiscalCte.NumeroAtual;

            _cte.NumeroFiscalEmissao = numeroAtual;
            _cte.SerieEmissao = emissor.EmissorFiscalCte.Serie;

            if (_cte.CodigoNumericoEmissao == 0)
            {
                _cte.CodigoNumericoEmissao = CodigoNumeroEmissao(_cte);
            }
            
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioEmissao = new RepositorioEmissorFiscal(sessao);
                var repositorioCte = new RepositorioCte(sessao);

                repositorioCte.Salvar(_cte);
                repositorioEmissao.SalvarEmissorFiscalCte(emissor.EmissorFiscalCte);

                transacao.Commit();
            }
        }

        private void AtualizarEmissor(EmissorFiscal cteEmissorFiscal)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                new RepositorioCte(sessao).AtualizarEmissor(cteEmissorFiscal);
            }
        }

        private int CodigoNumeroEmissao(Cte cte)
        {
            var random = new Random().Next(1, 99999999);

            if (random == cte.NumeroFiscalEmissao)
            {
                return CodigoNumeroEmissao(cte);
            }

            return random;
        }
    }
}