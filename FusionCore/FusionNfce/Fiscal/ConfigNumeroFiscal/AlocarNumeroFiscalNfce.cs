using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.EmissorFiscal;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Sessao;
using NHibernate;

namespace FusionCore.FusionNfce.Fiscal.ConfigNumeroFiscal
{
    public class AlocarNumeroFiscalNfce
    {
        public void Alocar(Nfce nfce, NfceEmissorFiscalNfce emissorFiscal, TipoEmissao tipoEmissao)
        {
            GetCodigoNumericoNFce(nfce);

            using (var sessao = new SessaoManagerNfce().CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                if (tipoEmissao == TipoEmissao.ContigenciaOfflineNFCe && emissorFiscal.UsaNumeracaoDiferenteContigencia)
                {
                    nfce.Serie = emissorFiscal.SerieContingencia;
                    nfce.NumeroFiscal = ++emissorFiscal.NumeroAtualContingencia;

                    Salvar(nfce, emissorFiscal, sessao);
                    transacao.Commit();
                    return;
                }

                nfce.Serie = emissorFiscal.Serie;
                nfce.NumeroFiscal = ++emissorFiscal.NumeroAtual;

                Salvar(nfce, emissorFiscal, sessao);
                transacao.Commit();
            }
        }

        private void GetCodigoNumericoNFce(Nfce nfce)
        {
            if (nfce.CodigoNumerico != 0) return;

            var random = new Random().Next(1, 99999999);

            if (random == nfce.NumeroDocumento)
            {
                GetCodigoNumericoNFce(nfce);
            }

            nfce.CodigoNumerico = random;
        }

        private static void Salvar(Nfce nfce, NfceEmissorFiscalNfce emissorFiscal, ISession sessao)
        {
            new RepositorioNfce(sessao).SalvarESincronizar(nfce);
            new RepositorioEmissorFiscalNfce(sessao).SalvarESincronizar(emissorFiscal.EmissorFiscal);
        }
    }
}