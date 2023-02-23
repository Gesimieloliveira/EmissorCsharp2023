using FusionCore.FusionAdm.Emissores;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;

namespace FusionCore.FusionAdm.Fiscal.NF.Integridade
{
    public class ReservadorNumeroNfe
    {
        private readonly ISessaoManager _sessaoManager;

        public ReservadorNumeroNfe(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public void ReservarNumero(Nfeletronica nfe, EmissorFiscal emissor)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            using (var transaction = sessao.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                sessao.Refresh(emissor);

                var numero = ++emissor.EmissorFiscalNfe.NumeroAtual;
                var serie = emissor.EmissorFiscalNfe.Serie;

                nfe.NumeroEmissao = numero;
                nfe.SerieEmissao = serie;

                sessao.Update(emissor.EmissorFiscalNfe);
                new RepositorioNfe(sessao).Merge(nfe);

                transaction.Commit();
            }
        }
    }
}