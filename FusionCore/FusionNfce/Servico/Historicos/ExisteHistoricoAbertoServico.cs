using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Sessao;

namespace FusionCore.FusionNfce.Servico.Historicos
{
    public class ExisteHistoricoAbertoServico
    {
        public static bool Existe(Nfce nfce)
        {
            if (nfce == null || nfce.Id == 0)
                return false;

            using (var sessao = new SessaoManagerNfce().CriaSessao())
            {
                if (SessaoSistemaNfce.IsEmissorNFce())
                    return new RepositorioNfce(sessao).ExisteHistoricoEmAbertoNfce(nfce);

                if (SessaoSistemaNfce.IsEmissorSat())
                    return new RepositorioNfce(sessao).ExisteHistoricoEmAbertoSat(nfce);
            }
                

            return false;
        }
    }
}