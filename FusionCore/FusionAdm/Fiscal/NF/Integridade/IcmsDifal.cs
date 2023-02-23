using FusionCore.FusionAdm.Fiscal.PartilhaIcms;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Interestadual;

namespace FusionCore.FusionAdm.Fiscal.NF.Integridade
{
    internal static class IcmsDifal
    {
        public static void Partilhar(Nfeletronica nfe, ISessaoManager sessaoManager)
        {
            try
            {
                var aliquotaInterstadual = BuscaAliquotaInterestadual(nfe, sessaoManager);
                var siglaUfDestino = nfe.Destinatario.Endereco.Localizacao.SiglaUF;

                using (var sessao = sessaoManager.CriaSessao())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorioProduto = new RepositorioProduto(sessao);
                    var repositorioAliquotaInterna = new RepositorioAliquotaInterna(sessao);
                    var repositorioNfe = new RepositorioNfe(sessao);

                    foreach (var item in nfe.Itens)
                    {
                        if (item.PartilharIcms == false)
                        {
                            repositorioNfe.DeletaIcmsInterstadual(item);
                            item.IcmsInterstadual = null;

                            continue;
                        }

                        var aliquotaInterna = repositorioAliquotaInterna.ObterPorSiglaUfDestino(siglaUfDestino);

                        var aliquotaDestino = aliquotaInterna.Aliquota;
                        var aliquotaFcpDestino = aliquotaInterna.AliquotaFcp;

                        var regraDoEstado = repositorioProduto.GetRegraTributacao(item.Produto, siglaUfDestino);

                        if (regraDoEstado != null)
                        {
                            aliquotaDestino = regraDoEstado.Aliquota;
                            aliquotaFcpDestino = regraDoEstado.AliquotaFcp;
                        }

                        if (item.IcmsInterstadual == null)
                        {
                            item.IcmsInterstadual = new IcmsInterstadual(item);
                        }

                        item.IcmsInterstadual.AliquotaInternaDestino = aliquotaDestino;
                        item.IcmsInterstadual.AliquotaInterstadual = aliquotaInterstadual;
                        item.IcmsInterstadual.AliquotaCombatePobreza = aliquotaFcpDestino;
                        item.IcmsInterstadual.PercentualParaDestino = PartilhaProvisoria.PercentualAnoCorrente;

                        item.IcmsInterstadual.Calcular();

                        repositorioNfe.Salvar(item.IcmsInterstadual);
                    }

                    transacao.Commit();
                }
            }
            catch
            {
                foreach (var item in nfe.Itens)
                {
                    item.IcmsInterstadual = null;
                }

                throw;
            }
        }

        private static decimal BuscaAliquotaInterestadual(Nfeletronica nfe, ISessaoManager sessaoManager)
        {
            using (var sessao = sessaoManager.CriaSessao())
            {
                var ufOrigem = nfe.Emitente.Empresa.EstadoDTO;

                var ufs = new RepositorioEstado(sessao);;
                var ufDestino = ufs.PeloIbge(nfe.Destinatario.Endereco.Localizacao.CodigoUF);

                var repositorio = new RepositorioPartilhaIcms(sessao);

                return repositorio.ObtemAliquota(ufOrigem, ufDestino);
            }
        }
    }
}