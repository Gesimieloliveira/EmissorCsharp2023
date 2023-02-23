using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.FusionAdm.CteEletronicoOs.NumeroFiscal
{
    public class AlocarNumeroFiscalCteOs
    {
        public void Alocar(CteOs cteOs)
        {
            var emissor = cteOs.Perfil.EmissorFiscal.EmissorFiscalCteOs;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var numero = ++emissor.NumeroAtual;
                var serie = emissor.Serie;

                cteOs.NumeroEmissao = numero;
                cteOs.SerieEmissao = serie;

                new RepositorioCteOs(sessao).Salvar(cteOs);
                new RepositorioEmissorFiscal(sessao).Salvar(emissor.EmissorFiscal);

                transacao.Commit();
            }
        }
    }
}